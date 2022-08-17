using System.Collections.Generic;
using AnimationUprising.Strider;
using MotionCaptureBasic;
using MotionCaptureBasic.Interface;
using MotionCaptureBasic.OSConnector;
using StandTravelModel.Scripts.Runtime.Core;
using StandTravelModel.Scripts.Runtime.Core.Interface;
using StandTravelModel.Scripts.Runtime.FK.Scripts;
using StandTravelModel.Scripts.Runtime.MotionModel;
using StandTravelModel.Scripts.Runtime.WeirdHumanoid;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime
{
    public enum MotionMode
    {
        Travel = 0,
        Stand
    }

    /// <summary>
    /// stand travel sdk的主脚本。该脚本直接挂载到指定角色模型上，连接os后可实现基本的动捕驱动功能
    /// </summary>
    public class StandTravelModelManager : MonoBehaviour
    {
        [Range(0, 1)] public float progress;

        #region Serializable Variables

        [Tooltip("Debug模式开关。如果打开可以打印额外Debug信息，并且显示骨骼点")]
        public bool isDebug;

        [Tooltip("是否启用FK。如果启用IK将无效")]
        public bool isFKEnabled;

        [Tooltip("是否启用非对称映射。如果开启可以匹配非标准人体骨骼的模型")]
        public bool monsterMappingEnable;

        [Tooltip("是否通过外部逻辑控制速度。如果开启，sdk本身对模型的移动控制将失效")]
        public bool hasExController;

        [Tooltip("是否在跑步时自动进入全身动画。如果启用，播跑步动画时会自动使FK无效")] 
        public bool isFullAnimOnRun;

        [Tooltip("是否使用locomotion实现stand模式局部位移")]
        public bool useLocomotion;
        
        [Tooltip("指定Basic SDK的OS通信模式")]
        public MotionDataModelType motionDataModelType;

        [Tooltip("初始Motion Mode")]
        public MotionMode initialMode = MotionMode.Stand;
        
        public AnimationCurve speedCurve;
        public AnimationCurve downCurve;
        public TuningParameterGroup tuningParameters;
        public ModelIKSettingGroup modelIKSettings;
        public AnimatorSettingGroup animatorSettings;
        public Transform selfTransform;
        public StepStateSmoother stepSmoother;
        public StriderBiped striderBiped;
        public StandTravelParamsLoader paramsLoader;
        public float strideScaleRun = 1;
        public float strideScaleWalk = 1;
#if USE_FK_LOCAL_ROTATION
        public FKJpintMap[] travelFkControlPoints;
#else
        public EFKType[] travelFkControlPoints;
#endif

        #endregion
        
         
        #region Unserializable Variables
        private IMotionModel motionModel;
        private IMotionDataModel motionDataModel;
        public IMotionDataModel motionDataModelReference => motionDataModel;
        private IModelIKController modelIKController;

        private AnchorController anchorController;
        private StandModel standModel;
        private TravelModel travelModel;
        private GameObject keyPointsParent;
#if USE_FK_LOCAL_ROTATION
        private FKAnimatorJoints fkAnimatorJointsModel;
#else
        private IFKPoseModel fKPoseModel;
#endif
        

        private MotionMode _currentMode = MotionMode.Stand;

        public MotionMode currentMode
        {
            get => _currentMode;
            set
            {
                if (_currentMode != value)
                {
                    _currentMode = value;
                    ChangeIKModelWeight((int)value);
                    if (value == MotionMode.Stand)
                    {
                        //T强制切回idlestate
                        //travelModel.ChangeState(AnimationList.Idle);
                        //travelModel.StopPrevAnimation("");
                        travelModel?.selfAnimator.Play("Idle");
                    }
                }
            }
        }

        public int currentLeg => travelModel.currentLeg;
        public float currentFrequency => travelModel.currentFrequency;
        public bool isJump => travelModel.isJump;

        private bool enable;

        public bool Enabled
        {
            get { return enabled; }
            set
            {
                enabled = value;
#if USE_FINAL_IK
                modelIKSettings.SetEnable(value);
#else
                modelIKSettings.IKScript.enabled = value;
#endif
#if USE_FK_LOCAL_ROTATION
                // ReSharper disable once Unity.NoNullPropagation
                fkAnimatorJointsModel?.SetEnable(value);
#else
                fKPoseModel?.SetEnable(value);
#endif
            }
        }

        private bool _osConnected = false;
        public bool osConnected => _osConnected;

        public float groundHeight
        {
            get
            {
                if (travelModel != null)
                {
                    return travelModel.GetGroundHeight();
                }

                return 0;
            }
        }

        private Vector3 _initPosition;
        public Vector3 initPosition => _initPosition;

        public bool isRun
        {
            get
            {
                if (travelModel != null)
                {
                    return travelModel.isRun;
                }

                return false;
            }
        }

        #endregion

        private List<Vector3> keyPointsList;
        private IKeyPointsConverter keyPointsConverter;

        public void Awake()
        {
            Application.targetFrameRate = 60;
            _initPosition = this.transform.position;

            InitParamsLoader();
            InitMotionDataModel();
            InitModelIKController();
            InitAnchorController();

            InitMotionModels();
            currentMode = initialMode;

            TryInitWeirdHumanConverter();
            TryInitFKModel();
        }

        public void Start()
        {
#if USE_FINAL_IK
            modelIKSettings.IKScript.enabled = false;
#else
            modelIKSettings.SetEnable(false);
#endif
            transform.rotation = Quaternion.identity;

            modelIKController.InitializeIKTargets(keyPointsParent.transform);

            if (isFKEnabled)
            {
                EnableFK();
            }
            else
            {
                DisableFK();
            }

            if (isDebug)
            {
                motionDataModel.SetDebug(true);
            }

            OnStandTravelSwitch();
        }

        public void FixedUpdate()
        {
            if (motionModel != null)
            {
                motionModel.OnFixedUpdate();
            }
        }

        public void Update()
        {
            keyPointsList = motionDataModel.GetIKPointsData(true, true);
            if (keyPointsList == null)
            {
                return;
            }

            TryConvertKeyPoints(keyPointsList);

            modelIKController.UpdateIKTargetsData(keyPointsList);

            if(motionModel != null)
            {
                motionModel.OnUpdate(keyPointsList);
            }
            
#if USE_FK_LOCAL_ROTATION
            if (fkAnimatorJointsModel != null)
            {
                fkAnimatorJointsModel.UpdateFkInfo(motionDataModel.GetFitting());
            }
#endif

            ChangeFkOnRun();
        }

        public void LateUpdate()
        {
            if(motionModel != null)
            {
                motionModel.OnLateUpdate();
            }
        }

        public void OnValidate()
        {
            UpdateModelParameters();
        }

        public void OnDestroy()
        {
            Destroy(keyPointsParent);
            motionDataModel = null;
            modelIKController?.ClearFakeNodes();
            modelIKController = null;

            if(standModel != null)
            {
                standModel.Clear();
                standModel = null;
            }

            if(travelModel != null)
            {
                travelModel.Clear();
                travelModel = null;
            }
        }

        /// <summary>
        /// 用于提供给外部调用，切换stand travel模式
        /// </summary>
        /// <returns>切换以后的模式</returns>
        public MotionMode SwitchStandTravel()
        {
            switch (currentMode)
            {
                case MotionMode.Stand:
                    currentMode = MotionMode.Travel;
                    break;
                case MotionMode.Travel:
                    currentMode = MotionMode.Stand;
                    break;
            }

            OnStandTravelSwitch();

            return currentMode;
        }

        /// <summary>
        /// 切换模式时需要做的一些处理
        /// </summary>
        private void OnStandTravelSwitch()
        {
            SwitchMotionMode(currentMode);
            SwitchFKBody(currentMode);
        }

        private void SwitchMotionMode(MotionMode mode)
        {
            switch (mode)
            {
                case MotionMode.Stand:
                    motionModel = standModel;
                    standModel.SetGrounding(true);
                    break;
                case MotionMode.Travel:
                    motionModel = travelModel;
                    travelModel.SetGrounding(false);
                    travelModel.FixAvatarHeight();
                    break;
            }
        }

        private void SwitchFKBody(MotionMode mode)
        {
            switch (mode)
            {
                case MotionMode.Stand:
                    FKBodyFull();
                    break;
                case MotionMode.Travel:
                    FKBodyUpper();
                    break;
            }
        }

        /// <summary>
        /// 获取travel锚点transform
        /// </summary>
        /// <returns></returns>
        public Transform GetTravelAnchor()
        {
            if (anchorController != null)
            {
                return anchorController.TravelFollowPoint.transform;
            }

            return null;
        }

        /// <summary>
        /// 获取stand锚点transform
        /// </summary>
        /// <returns></returns>
        public Transform GetStandAnchor()
        {
            if (anchorController != null)
            {
                return anchorController.StandFollowPoint.transform;
            }

            return null;
        }

        /// <summary>
        /// 获取stand注视点
        /// </summary>
        /// <returns></returns>
        public Transform GetStandLookAt()
        {
            if (anchorController != null)
            {
                return anchorController.StandLookAtPoint.transform;
            }

            return null;
        }

        /// <summary>
        /// 获取travel注视点
        /// </summary>
        /// <returns></returns>
        public Transform GetTravelLookAt()
        {
            if (anchorController != null)
            {
                return anchorController.TravelLookAtPoint.transform;
            }

            return null;
        }

        /// <summary>
        /// 旋转锚点，实现模型object的全局转向
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="dt"></param>
        public void TurnCharacter(float angle, float dt)
        {
            if(motionModel != null)
            {
                var deltaRotation = Quaternion.Euler(0,tuningParameters.RotationSensitivity * angle * dt, 0);
                motionModel.GetAnchorController().TurnControlPoints(deltaRotation);
            }
        }

        /// <summary>
        /// 重置ground location
        /// </summary>
        public void ResetGroundLocation()
        {
            if (useLocomotion)
            {
                standModel.ResetLocomotion();
            }
            else
            {
                motionDataModel.ResetGroundLocation();
            }
        }

        /// <summary>
        /// 获取骨骼点信息
        /// </summary>
        /// <returns></returns>
        public List<Vector3> GetKeyPointsList()
        {
            return keyPointsList;
        }

        public float GetRunThrehold()
        {
            return paramsLoader.GetRunThrehold();
        }

        public void SetRunThrehold(float value)
        {
            paramsLoader.SetRunThrehold(value);
        }

        public bool GetUseFrequency()
        {
            return paramsLoader.GetUseFrequency();
        }

        public void SetUseFrequency(bool value)
        {
            paramsLoader.SetUseFrequency(value);
        }

        public void SerializeParams()
        {
            paramsLoader.Serialize();
        }

        private void ChangeIKModelWeight(int weight)
        {
            modelIKController.ChangeLowerBodyIKWeight(weight);
        }
        
        private void UpdateModelParameters()
        {
            if (motionDataModel != null)
            {
                motionDataModel.SetPreprocessorParameters(tuningParameters.ScaleMotionPos);
            }

            if (travelModel != null)
            {
                travelModel.cacheQueueMax = tuningParameters.CacheStepCount;
                travelModel.stepMaxInterval = tuningParameters.StepToRunTimeThreshold;
            }

            if (standModel != null)
            {
                standModel.IsUseLocomotion(useLocomotion);
            }

            if (modelIKController is ModelFinalIKController modelFinalIKController)
            {
                modelFinalIKController.skewCorrection = tuningParameters.SkewCorrection;
            }

            if(progress > 0.0001f)
            {
                GetComponent<Animator>().SetFloat("progress", progress);
            }
        }

        private void TryConvertKeyPoints(List<Vector3> keyPoints)
        {
            if(keyPointsConverter != null)
            {
                keyPointsConverter.ConvertKeyPoints(keyPoints);
            }
        }

        private void TryInitWeirdHumanConverter()
        {
            if(monsterMappingEnable)
            {
                var locater = GetComponent<WeirdHumanoidPointsLocater>();
                if(locater != null)
                {
                    keyPointsConverter = new WeirdHumanoidPointConverter(locater);
                }
            }
        }

        private void InitMotionModels()
        {
            var modelAnimator = this.GetComponent<Animator>();
            var characterHipNode = modelAnimator.GetBoneTransform(HumanBodyBones.Hips);
            var characterHeadNode = modelAnimator.GetBoneTransform(HumanBodyBones.Head);
            InitStandModel(characterHipNode, characterHeadNode);
            InitTravelModel(characterHipNode, characterHeadNode);
        }

        private void InitTravelModel(Transform hip, Transform head)
        {
            stepSmoother = new StepStateSmoother();
            travelModel = new TravelModel(transform, hip, head, keyPointsParent.transform, tuningParameters,
                motionDataModel, anchorController, animatorSettings, hasExController, speedCurve, downCurve, stepSmoother, striderBiped,
                () => paramsLoader.GetRunThrehold(), () => strideScaleWalk, () => strideScaleRun, () => paramsLoader.GetUseFrequency()
            );
        }

        private void InitStandModel(Transform hip, Transform head)
        {
            standModel = new StandModel(transform, hip, head, keyPointsParent.transform, tuningParameters,
                motionDataModel, anchorController);
            standModel.IsUseLocomotion(useLocomotion);
        }

        private void InitAnchorController()
        {
            anchorController = new AnchorController(transform.position);
            keyPointsParent = new GameObject("KeyPointsParent");
            //keyPointsParent.transform.parent = anchorController.TravelFollowPoint.transform;
            //TODO: 暂时将keyPoints父节点设置为角色模型的transform。后续还需要测试优化确认有没其他问题
            keyPointsParent.transform.parent = transform;
        }

        private void InitModelIKController()
        {
            GameObject fakeNodeObj;
            if (isDebug)
            {
                fakeNodeObj = Resources.Load<GameObject>("FakeNode");
            }
            else
            {
                fakeNodeObj = new GameObject("FakeNodeObj");
            }

#if USE_FINAL_IK
            modelIKController = new ModelFinalIKController(fakeNodeObj, modelIKSettings.FinalIKComponent,
                modelIKSettings.FinalIKLookAtComponent);
#else
            modelIKController = new ModelNativeIKController(fakeNodeObj, modelIKSettings.IKScript);
#endif
            if (modelIKController is ModelFinalIKController modelFinalIKController)
            {
                modelFinalIKController.skewCorrection = tuningParameters.SkewCorrection;
            }

            if (!isDebug)
            {
                Object.Destroy(fakeNodeObj);
            }
        }

        public bool IsFKEnabled()
        {
#if USE_FK_LOCAL_ROTATION
            if (fkAnimatorJointsModel != null)
            {
                return fkAnimatorJointsModel.IsEnabled();
            }
#else
            if(fKPoseModel != null)
            {
                return fKPoseModel.IsEnabled();
            }
#endif
            return false;
        }

        public void EnableFK()
        {
#if USE_FK_LOCAL_ROTATION
            if(fkAnimatorJointsModel != null)
            {
                fkAnimatorJointsModel.SetEnable(true);
                modelIKSettings.SetEnable(false);
            }
#else
            if(fKPoseModel != null)
            {
                fKPoseModel.SetEnable(true);
                modelIKSettings.SetEnable(false);
            }
#endif
        }

        public void DisableFK()
        {
#if USE_FK_LOCAL_ROTATION
            if(fkAnimatorJointsModel != null)
            {
                fkAnimatorJointsModel.SetEnable(false);
                modelIKSettings.SetEnable(true);
            }
#else
            if(fKPoseModel != null)
            {
                fKPoseModel.SetEnable(false);
                modelIKSettings.SetEnable(true);
            }
#endif
            
        }

        private void TryInitFKModel()
        {
#if USE_FK_LOCAL_ROTATION
            if(fkAnimatorJointsModel == null)
            {
                fkAnimatorJointsModel = gameObject.AddComponent<FKAnimatorJoints>();
                fkAnimatorJointsModel.SetEnable(false);
            }
#else
            if(fKPoseModel == null)
            {
                fKPoseModel = gameObject.AddComponent<FKPoseModel>();
                fKPoseModel.SetEnable(false);
                fKPoseModel.Initialize();
            }
#endif
        }

        private void FKBodyUpper()
        {
#if USE_FK_LOCAL_ROTATION
            fkAnimatorJointsModel.SetActiveFKJpintTypes(travelFkControlPoints);
#else
            fKPoseModel.SetActiveEFKTypes(travelFkControlPoints);
#endif
        }

        private void FKBodyFull()
        {
#if USE_FK_LOCAL_ROTATION
            fkAnimatorJointsModel.SetFullFKJpintTypes();
#else
            fKPoseModel.SetFullBodyEFKTypes();
#endif
        }

        private void ChangeFkOnRun()
        {
            if (!isFullAnimOnRun)
            {
                return;
            }

            if (travelModel != null)
            {
                if (travelModel.isRun)
                {
#if USE_FK_LOCAL_ROTATION
                    fkAnimatorJointsModel.SetActiveFKJpintTypes(new FKJpintMap[] {});
#else
                    fKPoseModel.SetActiveEFKTypes();
#endif
                }
                else
                {
                    FKBodyUpper();
                }
            }
        }

        /// <summary>
        /// 初始化basic sdk的数据基础模块。所有动捕基础数据都会从motionDataModel里面取。并且通过该类的方法实现和os交互
        /// </summary>
        private void InitMotionDataModel()
        {
            motionDataModel = MotionDataModelFactory.Create(motionDataModelType);
            motionDataModel.SetPreprocessorParameters(tuningParameters.ScaleMotionPos);
            switch (motionDataModelType)
            {
                case MotionDataModelType.Http:
                    motionDataModel.AddConnectEvent(SubscribeMessage);
                    break;
                case MotionDataModelType.Cpp:
                    Debug.LogError("cpp方式的os通信还没有实现");
                    break;
                case MotionDataModelType.Mobile:
                    _osConnected = true;
                    break;
            }
        }

        private void SubscribeMessage()
        {
            motionDataModel.SubscribeActionDetection();
            motionDataModel.SubscribeGroundLocation();
            motionDataModel.SubscribeFitting();
            _osConnected = true;
        }

        public void ResetAnchorPosition()
        {
            GetTravelAnchor().position = selfTransform.position;
            GetStandAnchor().position = selfTransform.position;
        }

        public Vector3 GetMoveVelocity()
        {
            if (hasExController)
            {
                return travelModel.moveVelocity;
            }
            else
            {
                return Vector3.zero;
            }
        }

        public float GetRunSpeedScale()
        {
            return paramsLoader.GetRunSpeedScale();
        }

        public void SetRunSpeedScale(float value)
        {
            paramsLoader.SetRunSpeedScale(value);
        }

        private void InitParamsLoader()
        {
            paramsLoader = new StandTravelParamsLoader();
            paramsLoader.Deserialize();
        }
    }
}
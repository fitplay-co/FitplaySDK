using System.Collections.Generic;
using MotionCaptureBasic;
using MotionCaptureBasic.Interface;
using StandTravelModel.Core;
using StandTravelModel.Core.Interface;
using StandTravelModel.MotionModel;
using UnityEngine;
using WeirdHumanoid;
using MotionCaptureBasic.OSConnector;
using FK;


namespace StandTravelModel
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
        #region Serializable Variables

        [Tooltip("Debug模式开关。如果打开可以打印额外Debug信息，并且显示骨骼点")]
        public bool isDebug;
        [Tooltip("是否启用FK。如果启用IK将无效")]
        public bool isFKEnabled;
        [Tooltip("是否启用非对称映射。如果开启可以匹配非标准人体骨骼的模型")]
        public bool monsterMappingEnable;
        [Tooltip("是否通过外部逻辑控制速度。如果开启，sdk本身对模型的移动控制将失效")]
        public bool hasExController;
        public MotionMode initialMode = MotionMode.Stand;
        public TuningParameterGroup tuningParameters;
        public ModelIKSettingGroup modelIKSettings;
        public AnimatorSettingGroup animatorSettings;
        public Transform selfTransform;
        #endregion
        
         
        #region Unserializable Variables
        private IMotionModel motionModel;
        private IMotionDataModel motionDataModel;
        private IModelIKController modelIKController;

        private AnchorController anchorController;
        private StandModel standModel;
        private TravelModel travelModel;
        private GameObject keyPointsParent;
        private IFKPoseModel fKPoseModel;

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
                        travelModel.ChangeState(AnimationList.Idle);
                        //travelModel.StopPrevAnimation("");
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
            set { enabled = value;
#if USE_FINAL_IK
                fKPoseModel?.SetEnable(value);
                modelIKSettings.SetEnable(value);
#else
                modelIKSettings.IKScript.enabled = value;
#endif
            }
        }

        #endregion

        private List<Vector3> keyPointsList;
        private IKeyPointsConverter keyPointsConverter;

        public void Awake()
        {
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
                    break;
                case MotionMode.Travel:
                    motionModel = travelModel;
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
            if(anchorController != null)
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
            if(anchorController != null)
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
            motionDataModel.ResetGroundLocation();
        }

        /// <summary>
        /// 获取骨骼点信息
        /// </summary>
        /// <returns></returns>
        public List<Vector3> GetKeyPointsList()
        {
            return keyPointsList;
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
            
            if (modelIKController is ModelFinalIKController modelFinalIKController)
            {
                modelFinalIKController.skewCorrection = tuningParameters.SkewCorrection;
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
            InitStandModel(characterHipNode);
            InitTravelModel(characterHipNode);
        }

        private void InitTravelModel(Transform characterHipNode)
        {
            travelModel = new TravelModel(transform, characterHipNode, keyPointsParent.transform, tuningParameters, motionDataModel, anchorController, animatorSettings, hasExController);
        }

        private void InitStandModel(Transform characterHipNode)
        {
            standModel = new StandModel(transform, characterHipNode, keyPointsParent.transform, tuningParameters, motionDataModel, anchorController);
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
            modelIKController = new ModelFinalIKController(fakeNodeObj, modelIKSettings.FinalIKComponent, modelIKSettings.FinalIKLookAtComponent);
#else
            modelIKController = new ModelNativeIKController(fakeNodeObj, modelIKSettings.IKScript);
#endif
            if (modelIKController is ModelFinalIKController modelFinalIKController)
            {
                modelFinalIKController.skewCorrection = tuningParameters.SkewCorrection;
            }
        }

        public bool IsFKEnabled()
        {
            if(fKPoseModel != null)
            {
                return fKPoseModel.IsEnabled();
            }
            return false;
        }

        public void EnableFK()
        {
            if(fKPoseModel != null)
            {
                fKPoseModel.SetEnable(true);
                modelIKSettings.SetEnable(false);
            }
        }

        public void DisableFK()
        {
            if(fKPoseModel != null)
            {
                fKPoseModel.SetEnable(false);
                modelIKSettings.SetEnable(true);
                MotionDataModelHttp.GetInstance().ReleaseFitting();
            }
        }

        private void TryInitFKModel()
        {
            if(fKPoseModel == null)
            {
                fKPoseModel = gameObject.AddComponent<FKPoseModel>();
                fKPoseModel.SetEnable(false);
                fKPoseModel.Initialize();
            }
        }

        private void FKBodyUpper()
        {
            fKPoseModel.SetActiveEFKTypes(
                EFKType.Neck,
                //EFKType.Head,
                EFKType.LShoulder,
                EFKType.RShoulder,
                EFKType.LArm,
                EFKType.RArm,
                EFKType.LWrist,
                EFKType.RWrist
            );
        }

        private void FKBodyFull()
        {
            fKPoseModel.SetFullBodyEFKTypes();
        }

        /// <summary>
        /// 初始化basic sdk的数据基础模块。所有动捕基础数据都会从motionDataModel里面取。并且通过该类的方法实现和os交互
        /// </summary>
        private void InitMotionDataModel()
        {
            motionDataModel = MotionDataModelFactory.Create(MotionDataModelType.Http);
            motionDataModel.SetPreprocessorParameters(tuningParameters.ScaleMotionPos);
            motionDataModel.AddConnectEvent(SubscribeMessage);
        }

        private void SubscribeMessage()
        {
            motionDataModel.SubscribeActionDetection();
            motionDataModel.SubscribeGroundLocation();
            motionDataModel.SubscribeFitting();
        }
        
        public void ResetAnchorPosition()
        {
            GetTravelAnchor().position = selfTransform.position;
            GetStandAnchor().position =  selfTransform.position;
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
    }
}
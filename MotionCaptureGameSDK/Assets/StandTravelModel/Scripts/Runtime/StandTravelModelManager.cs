using System.Collections.Generic;
using System.Threading.Tasks;
using MotionCaptureBasic;
using MotionCaptureBasic.Interface;
using MotionCaptureBasic.OSConnector;
using StandTravelModel.Scripts.Runtime.Core;
using StandTravelModel.Scripts.Runtime.Core.Interface;
using StandTravelModel.Scripts.Runtime.FK.Scripts;
using StandTravelModel.Scripts.Runtime.MotionModel;
using StandTravelModel.Scripts.Runtime.WeirdHumanoid;
using StandTravelModel.Scripts.Runtime.Core.AnimationStates.Components;
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
        #region Serializable Variables
        [Range(0, 1)] public float progress;

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

        [Tooltip("是否手动输入身高。如果开启，在游戏进入时需要先输入身高")]
        public bool manualInputTall;

        [Tooltip("是否启用stand模式下turn character")]
        public bool turnCharaStandMode;

        [Tooltip("是否使用json与os交互")] 
        public bool isUseJson;

        [Tooltip("是否在stand模式进行贴地计算")]
        public bool groundCheckOnStand = true;
        
        [Tooltip("指定Basic SDK的OS通信模式")]
        public MotionDataModelType motionDataModelType;

        [Tooltip("是否自动连接OS")]
        public bool connectOsAuto;
        
        [Tooltip("如果用http模式OS，启用该开关会自动连接localhost的OS")]
        public bool connectDefaultIp;

        [Tooltip("初始Motion Mode")]
        public MotionMode initialMode = MotionMode.Stand;
        
        public AnimationCurve speedCurve;
        public AnimationCurve downCurve;
        public TuningParameterGroup tuningParameters;
        public ModelIKSettingGroup modelIKSettings;
        public AnimatorSettingGroup animatorSettings;
        public Transform selfTransform;
        public StepStateSmoother stepSmoother;
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
        private StandTravelTestUI standTravelTestUI;
        private IModelIKController modelIKController;

        private Animator modelAnimator;
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

        //用于判断是否ui呼出，呼出中需要停止sdk功能
        private bool _noUiActive = true;
        public bool noUiActive
        {
            get => _noUiActive;
            set
            {
                if (_noUiActive != value)
                {
                    _noUiActive = value;
                }
            }
        }

        //用于判断os识别是否有效，os识别无效时需要停止sdk功能
        private bool _osValidCheck = true;
        public bool osValidCheck => _osValidCheck;
        //用于辅助os数据有效性判断的timeout
        private const int osTimeOut = 1;
        private long prevOsTime;
        private float osTimeOutProgress = 0;

        //供上层游戏业务层调用的sdk控制开关
        private bool _Enabled = true;
        public bool Enabled
        {
            get => _Enabled;
            set
            {
                if (_Enabled != value)
                {
                    _Enabled = value;
                }
            }
        }

        //用于整体判断sdk是否启用的开关
        private bool _overallEnable = true;
        public bool overallEnable
        {
            get => _overallEnable;
            set
            {
                if (_overallEnable == value)
                {
                    return;
                }

                _overallEnable = value;
                if (!isFKEnabled)
                {
                    modelIKSettings.SetEnable(value);
                }
                else
                {
#if USE_FK_LOCAL_ROTATION
                    // ReSharper disable once Unity.NoNullPropagation
                    fkAnimatorJointsModel?.SetEnable(value);
#else
                    fKPoseModel?.SetEnable(value);
#endif
                }

                //如果检测到sdk不启用，需要强制重置动画状态机到idle
                if (!value)
                {
                    travelModel?.ChangeState(AnimationList.Idle);
                    travelModel?.selfAnimator.Play("Idle");
                    standModel?.AdjustTransformHeight();
                }
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

        private Quaternion _initRotation;
        public Quaternion initRotation => _initRotation;

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
        
        private bool destroyed;
        private List<Vector3> keyPointsList;
        private PlayerHeightUI playerHeightUI;
        private IKeyPointsConverter keyPointsConverter;
        private bool isWaitForReconnect;
        private const int DelayTime = 3000;

        #endregion

        public void Awake()
        {
            //Application.targetFrameRate = 60;
            if (selfTransform == null)
            {
                selfTransform = transform;
            }
            
            _initPosition = selfTransform.position;
            _initRotation = selfTransform.rotation;

            InitParamsLoader();
            InitMotionDataModel();
            InitModelIKController();
            InitAnchorController();

            InitMotionModels();
            currentMode = initialMode;

            TryInitWeirdHumanConverter();
            TryInitFKModel();
            InitStandTravelTestUI();
        }

        public void Start()
        {
            if (motionDataModelType != MotionDataModelType.Network)
            {
#if USE_FINAL_IK
                modelIKSettings.IKScript.enabled = false;
#else
                modelIKSettings.SetEnable(false);
#endif
                //transform.rotation = Quaternion.identity;

                modelIKController.InitializeIKTargets(keyPointsParent.transform);
            }
            
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

            if (connectOsAuto)
            {
                TryToConnectOs();
            }
        }

        public void FixedUpdate()
        {
            /*if (!overallEnable)
            {
                return;
            }

            if (motionModel != null)
            {
                motionModel.OnFixedUpdate();
            }*/
        }

        public void Update()
        {
            var deltaTime = Time.deltaTime;
            if (motionDataModelType != MotionDataModelType.Network)
            {
                _osValidCheck = GeneralCheck(deltaTime);
                //检查sdk是否启用
                overallEnable = _noUiActive && _osValidCheck && _Enabled;
            }

            if (!overallEnable)
            {
                return;
            }

            if (motionDataModelType != MotionDataModelType.Network)
            {
                keyPointsList = motionDataModel.GetIKPointsData(true, true);
                if (keyPointsList == null)
                {
                    return;
                }

                TryConvertKeyPoints(keyPointsList);

                modelIKController.UpdateIKTargetsData(keyPointsList);
            }

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
            TryToggleStandTravelTestUI();
        }

        public void LateUpdate()
        {
            if (!overallEnable || motionDataModelType == MotionDataModelType.Network)
            {
                return;
            }

            if (motionModel != null)
            {
                motionModel.OnLateUpdate();
            }
        }

        public void OnValidate()
        {
            if (motionDataModelType == MotionDataModelType.Network)
            {
                return;
            }

            UpdateModelParameters();
        }

        public void OnDestroy()
        {
            Destroy(keyPointsParent);
            if (motionDataModel.GetMotionDataModelType() != MotionDataModelType.Network && _osConnected)
            {
                ReleaseMessage();
                HttpProtocolHandler.GetInstance().ReleaseWebSocket();
            }
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
            destroyed = true;
        }

        private void TryToConnectOs()
        {
            if (connectDefaultIp)
            {
                PlayerPrefs.SetString(HttpProtocolHandler.OsIpKeyName, "127.0.0.1");
            }
            HttpProtocolHandler.GetInstance().StartWebSocket(PlayerPrefs.GetString(HttpProtocolHandler.OsIpKeyName), isUseJson);
        }

        private bool GeneralCheck(float dt)
        {
            var generalData = motionDataModel.GetGeneralDetectionData();
            if (generalData == null)
            {
                return false;
            }

            var timeProfiling = motionDataModel.GetTimeProfilingData();
            if (timeProfiling == null) 
            {
                return false;
            }

            var currentOsTime = timeProfiling.beforeSendTime;
            if (currentOsTime == prevOsTime)
            {
                osTimeOutProgress += dt;
            }
            else 
            {
                prevOsTime = currentOsTime;
                osTimeOutProgress = 0;
            }

            int generalConfidence = generalData.confidence;

            return generalConfidence == 1 && osTimeOutProgress < osTimeOut;
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
                    travelModel.FixAvatarHorizon();
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
            if (!turnCharaStandMode && currentMode == MotionMode.Stand) 
            {
                return;
            }

            if(motionModel != null)
            {
                var deltaRotation = Quaternion.Euler(0,tuningParameters.RotationSensitivity * angle * dt, 0);
                anchorController.TurnControlPoints(deltaRotation);
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

        public float GetSprintThrehold()
        {
            return paramsLoader.GetSprintThrehold();
        }

        public float GetSprintSpeedScale()
        {
            return paramsLoader.GetSprintSpeedScale();
        }

        public float GetRunThreholdLow()
        {
            return paramsLoader.GetRunThreholdLow();
        }

        public void SetRunThrehold(float value)
        {
            paramsLoader.SetRunThrehold(value);
        }

        public void SetSprintThrehold(float value)
        {
            paramsLoader.SetSprintThrehold(value);
        }

        public void SetSprintSpeedScale(float value)
        {
            paramsLoader.SetSprintSpeedScale(value);
        }

        public void SetRunThreholdLow(float value)
        {
            paramsLoader.SetRunThreholdLow(value);
        }

        public bool GetUseFrequency()
        {
            return paramsLoader.GetUseFrequency();
        }

        public void SetUseFrequency(bool value)
        {
            paramsLoader.SetUseFrequency(value);
        }

        public bool GetUseSmoothSwitch()
        {
            return paramsLoader.GetUseSmoothSwitch();
        }

        public void SetUseSmoothSwitch(bool value)
        {
            paramsLoader.SetUseSmoothSwitch(value);
        }

        public bool GetUseOSStepRate()
        {
            return paramsLoader.GetUseOSStepRate();
        }

        public void SetUseOSStepRate(bool value)
        {
            paramsLoader.SetUseOSStepRate(value);
        }

        public bool GetUseOSStepRateSeparate()
        {
            return paramsLoader.GetUseOSStepRateSeparate();
        }

        public void SetUseOSStepRateSeparate(bool value)
        {
            paramsLoader.SetUseOSStepRateSeparate(value);
        }

        public bool GetUseLegActTime()
        {
            return paramsLoader.GetUseLegActTime();
        }

        public void SetUseLegActTime(bool value)
        {
            paramsLoader.SetUseLegActTime(value);
        }

        public void SerializeParams()
        {
            paramsLoader.Serialize();
        }

        public void SwitchWalkRunAnimator(bool useSmoothSwitch)
        {
            if(useSmoothSwitch)
            {
                modelAnimator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animators/Girl Smooth");
            }
            else
            {
                modelAnimator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animators/Girl");
            }
        }

        private void ChangeIKModelWeight(int weight)
        {
            modelIKController?.ChangeLowerBodyIKWeight(weight);
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
                modelAnimator.SetFloat("progress", progress);
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
            if (motionDataModelType == MotionDataModelType.Network)
            {
                return;
            }

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
            keyPointsParent = new GameObject("KeyPointsParent");
            //keyPointsParent.transform.parent = anchorController.TravelFollowPoint.transform;
            //TODO: 暂时将keyPoints父节点设置为角色模型的transform。后续还需要测试优化确认有没其他问题
            keyPointsParent.transform.parent = selfTransform;
            this.modelAnimator = this.GetComponent<Animator>();
            var characterHipNode = modelAnimator.GetBoneTransform(HumanBodyBones.Hips);
            var characterHeadNode = modelAnimator.GetBoneTransform(HumanBodyBones.Head);
            MotionModelInteractData interactData = new MotionModelInteractData
            {
                localShift = Vector3.zero,
                groundHeight = 0,
                isUseLocomotion = false,
                isGroundCheckOnStand = groundCheckOnStand
            };
            InitStandModel(characterHipNode, characterHeadNode, interactData);
            InitTravelModel(characterHipNode, characterHeadNode, interactData);
        }

        private void InitTravelModel(Transform hip, Transform head, MotionModelInteractData interactData)
        {
            stepSmoother = new StepStateSmoother();
            travelModel = new TravelModel(
                selfTransform : transform,
                characterHipNode : hip,
                characterHeadNode : head,
                keyPointsParent : keyPointsParent.transform,
                tuningParameters : tuningParameters,
                motionDataModel : motionDataModel,
                anchorController : anchorController,
                interactData : interactData,
                animatorSettingGroup : animatorSettings,
                isExControl : hasExController,
                speedCurve : speedCurve,
                downCurve : downCurve,
                stepSmoother : stepSmoother, 
                getRunThrehold : paramsLoader.GetRunThrehold,
                getThreholdRunLow : paramsLoader.GetRunThreholdLow,
                strideScale : () => strideScaleWalk,
                strideScaleRun : () => strideScaleRun,
                useFrequency : paramsLoader.GetUseFrequency,
                getSprintThrehold : paramsLoader.GetSprintThrehold,
                getRunThresholdScale : paramsLoader.GetRunThresholdScale,
                getRunThresholdScaleLow : paramsLoader.GetRunThreholdScaleLow,
                useOSStepRate: paramsLoader.GetUseOSStepRate,
                useOSStepRateSeparate: paramsLoader.GetUseOSStepRateSeparate,
                useLegActTime: paramsLoader.GetUseLegActTime
            );
        }

        private void InitStandModel(Transform hip, Transform head, MotionModelInteractData interactData)
        {
            standModel = new StandModel(transform, hip, head, keyPointsParent.transform, tuningParameters,
                motionDataModel, anchorController, interactData);
            standModel.IsUseLocomotion(useLocomotion);
        }

        private void InitAnchorController()
        {
            if (motionDataModelType == MotionDataModelType.Network)
            {
                return;
            }

            anchorController = new AnchorController(_initPosition, _initRotation);
        }

        private void InitModelIKController()
        {
            if (motionDataModelType == MotionDataModelType.Network)
            {
                return;
            }
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
                    motionDataModel.AddConnectEvent(SubscribeMessage, ReConnectOs, WaitToReConnectOs);
                    break;
                case MotionDataModelType.Cpp:
                    Debug.LogError("cpp方式的os通信还没有实现");
                    break;
                case MotionDataModelType.Mobile:
                    _osConnected = true;
                    break;
                case MotionDataModelType.Network:
                    _osConnected = true;
                    break;
            }
        }

        private void SubscribeMessage()
        {
            if(!destroyed)
            {
                motionDataModel.SubscribeActionDetection();
                motionDataModel.SubscribeGroundLocation();
                motionDataModel.SubscribeFitting();
                motionDataModel.SubscribeGeneral();
                _osConnected = true;
                //OS连接成功后开启输入身高的UI
                if (manualInputTall)
                {
                    ShowPlayerHeightUI();
                }
                else 
                {
                    motionDataModel.SetPlayerHeight(175);
                }
            }
        }

        private void ReleaseMessage()
        {
            if (!destroyed)
            {
                motionDataModel.ReleaseActionDetection();
                motionDataModel.ReleaseGroundLocation();
                motionDataModel.ReleaseFitting();
                motionDataModel.ReleaseGeneral();
            }
        }

        private void ReConnectOs()
        {
            Debug.Log("Os disconnected, try to connect again");
            TryToConnectOs();
        }

        private void WaitToReConnectOs()
        {
            if (isWaitForReconnect)
            {
                return;
            }

            Debug.Log("Os connect error, try to connect again later");
            Task.Run(() => DelayToReConnectOs());
            isWaitForReconnect = true;
        }

        async Task DelayToReConnectOs()
        {
            await Task.Delay(DelayTime);
            Debug.Log("Try to reconnect os");
            TryToConnectOs();
            isWaitForReconnect = false;
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

        public float GetRunThresholdScale()
        {
            return paramsLoader.GetRunThresholdScale();
        }

        public float GetRunThresholdScaleLow()
        {
            return paramsLoader.GetRunThreholdScaleLow();
        }

        public bool GetUseOSSpeed()
        {
            return paramsLoader.GetUseOSSpeed();
        }

        public void SetRunThresholdScaleLow(float value)
        {
            paramsLoader.SetRunThreholdScaleLow(value);
        }

        public void SetRunThresholdScale(float value)
        {
            paramsLoader.SetRunThresholdScale(value);
        }

        public void SetUseOSSpeed(bool value)
        {
            paramsLoader.SetUseOSSpeed(value);
        }

        public RunConditioner GetRunConditioner()
        {
            if(travelModel != null)
            {
                return travelModel.GetRunConditioner();
            }
            return null;
        }

        public float GetWalkSpeedScale()
        {
            return paramsLoader.GetWalkSpeedScale();
        }

        public void SetWalkSpeedScale(float value)
        {
            paramsLoader.SetWalkSpeedScale(value);
        }

        public void ShowPlayerHeightUI()
        {
            // ReSharper disable once Unity.NoNullPropagation
            playerHeightUI?.Show();
            noUiActive = false;
        }

        private void InitParamsLoader()
        {
            paramsLoader = new StandTravelParamsLoader();
            paramsLoader.Deserialize();
        }

        private void OnPlayerHeightInput(int height)
        {
            motionDataModel?.SetPlayerHeight(height);
            // ReSharper disable once Unity.NoNullPropagation
            playerHeightUI?.Hide();
            noUiActive = true;
        }

        public void InitPlayerHeightUI()
        {
            if(playerHeightUI == null)
            {
                var canvas = GameObject.Find("Canvas");
                var prefab = Resources.Load<GameObject>("PlayerHeightUI");
                var newobj = GameObject.Instantiate(prefab, canvas.transform);
                playerHeightUI = newobj.GetComponent<PlayerHeightUI>();
                playerHeightUI.Initialize(OnPlayerHeightInput);
                playerHeightUI.Hide();
            }
        }

        private void InitStandTravelTestUI()
        {
            if (motionDataModelType == MotionDataModelType.Network)
            {
                return;
            }
            if(standTravelTestUI == null)
            {
                standTravelTestUI = GameObject.FindObjectOfType<StandTravelTestUI>(true);
                if (standTravelTestUI != null)
                    standTravelTestUI.OccupyTandTravelModelManager(this);
            }
        }

        private void TryToggleStandTravelTestUI()
        {
            if(standTravelTestUI != null && Input.GetKeyDown(KeyCode.Tab))
            {
                standTravelTestUI.Toggle();
            }
        }
    }
}
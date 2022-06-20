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

    public class StandTravelModelManager : MonoBehaviour
    {
        #region Serializable Variables
        
        public bool isDebug;
        public bool monsterMappingEnable;
        public MotionMode initialMode = MotionMode.Stand;
        public TuningParameterGroup tuningParameters;
        public ModelIKSettingGroup modelIKSettings;
        public AnimatorSettingGroup animatorSettings;

        #endregion
        
         
        #region Unserializable Variables
        private IMotionModel motionModel;
        private IMotionDataModel motionDataModel;
        private IModelIKController modelIKController;

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
                fKPoseModel.SetEnable(value);
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
            var anchorController = InitTravelAnchorController();

            InitMotionModels(anchorController);
            currentMode = initialMode;
            SwitchMotionMode(currentMode);

            TryInitWeirdHumanConverter();
        }

        public void Start()
        {
#if USE_FINAL_IK
            modelIKSettings.IKScript.enabled = false;
#else
            modelIKSettings.FinalIKComponent.enabled = false;
            modelIKSettings.FinalIKLookAtComponent.enabled = false;
#endif
            transform.rotation = Quaternion.identity;

            modelIKController.InitializeIKTargets(keyPointsParent.transform);

            TryInitFKModel();
            SubscribeMessage();
            EnableFK();
        }

        public void Update()
        {
            if(isDebug)
            {
                if(motionDataModel.GetGazeTrackingData() != null)
                {
                    Debug.Log("gaze ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ -> " + (motionDataModel.GetGazeTrackingData()));
                }

                if(motionDataModel.GetGroundLocationData() != null)
                {
                    Debug.Log("ground --- " + motionDataModel.GetGroundLocationData().tracing);
                }

                if(motionDataModel.GetActionDetectionData() != null)
                {
                    Debug.Log("action --- " + motionDataModel.GetActionDetectionData().version);
                }
            }

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

            SwitchMotionMode(currentMode);

            return currentMode;
        }

        private void SwitchMotionMode(MotionMode mode)
        {
            switch (currentMode)
            {
                case MotionMode.Stand:
                    motionModel = standModel;
                    break;
                case MotionMode.Travel:
                    motionModel = travelModel;
                    break;
            }
        }

        public MotionMode GetCurrentMode()
        {
            return currentMode;
        }

        public Transform GetTravelAnchor()
        {
            if(motionModel != null)
            {
                return motionModel.GetAnchorController().TravelFollowPoint.transform;
            }
            return null;
        }

        public Transform GetStandAnchor()
        {
            if(motionModel != null)
            {
                return motionModel.GetAnchorController().StandFollowPoint.transform;
            }
            return null;
        }

        public void TurnCharacter(float angle, float dt)
        {
            if(motionModel != null)
            {
                var deltaRotation = Quaternion.Euler(0,tuningParameters.RotationSensitivity * angle * dt, 0);
                motionModel.GetAnchorController().TurnControlPoints(deltaRotation);
            }
        }

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

        private void InitMotionModels(AnchorController anchorController)
        {
            var modelAnimator = this.GetComponent<Animator>();
            var characterHipNode = modelAnimator.GetBoneTransform(HumanBodyBones.Hips);
            InitStandModel(characterHipNode, anchorController);
            InitTravelModel(characterHipNode, anchorController);
        }

        private void InitTravelModel(Transform characterHipNode, AnchorController anchorController)
        {
            travelModel = new TravelModel(transform, characterHipNode, keyPointsParent.transform, tuningParameters, motionDataModel, anchorController, animatorSettings);
        }

        private void InitStandModel(Transform characterHipNode, AnchorController anchorController)
        {
            standModel = new StandModel(transform, characterHipNode, keyPointsParent.transform, tuningParameters, motionDataModel, anchorController);
        }

        private AnchorController InitTravelAnchorController()
        {
            var anchorController = new AnchorController(transform.position);
            keyPointsParent = new GameObject("KeyPointsParent");
            //keyPointsParent.transform.parent = anchorController.TravelFollowPoint.transform;
            //TODO: 暂时将keyPoints父节点设置为角色模型的transform。后续还需要测试优化确认有没其他问题
            keyPointsParent.transform.parent = transform;
            return anchorController;
        }

        private void InitModelIKController()
        {
#if USE_FINAL_IK
            modelIKController = new ModelFinalIKController(modelIKSettings.NodePrefab, modelIKSettings.FinalIKComponent, modelIKSettings.FinalIKLookAtComponent);
#else
            modelIKController = new ModelNativeIKController(modelIKSettings.NodePrefab, modelIKSettings.IKScript);
#endif
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
                MotionDataModelHttp.GetInstance().SubscribeFitting();
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
                fKPoseModel.SetActiveEFKTypes(
                    EFKType.Neck,
                    EFKType.Head,
                    EFKType.LShoulder,
                    EFKType.RShoulder,
                    EFKType.LArm,
                    EFKType.RArm,
                    EFKType.LWrist,
                    EFKType.RWrist/* ,
                    EFKType.LHand,
                    EFKType.RHand,
                    EFKType.RHip,
                    EFKType.LHip,
                    EFKType.RKnee,
                    EFKType.LKnee,
                    EFKType.RAnkle,
                    EFKType.LAnkle */
                );

                fKPoseModel.Initialize();
            }
        }

        private void InitMotionDataModel()
        {
            motionDataModel = MotionDataModelHttp.GetInstance();
            motionDataModel.SetPreprocessorParameters(tuningParameters.ScaleMotionPos);
        }

        private void SubscribeMessage()
        {
            MotionDataModelHttp.GetInstance().SubscribeGazeTracking();
            MotionDataModelHttp.GetInstance().SubscribeGroundLocation();
        }
    }
}
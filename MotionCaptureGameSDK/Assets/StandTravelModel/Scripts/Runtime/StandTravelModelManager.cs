using System;
using System.Collections.Generic;
using MotionCaptureBasic;
using MotionCaptureBasic.Interface;
using MotionCaptureBasic.OSConnector;
using StandTravelModel.Core;
using StandTravelModel.Core.Interface;
using UnityEngine;
using WeirdHumanoid;

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
        
        public MotionMode initialMode = MotionMode.Stand;

        public TuningParameterGroup tuningParameters;

        public ModelIKSettingGroup modelIKSettings;

        public AnimatorSettingGroup animatorSettings;

        #endregion
        
        
         
        #region Unserializable Variables

        private IMotionDataModel motionDataModel;

        private CharacterAnimatorController characterAnimatorController;

        private IModelIKController modelIKController;

        private StandTravelAnchorController standTravelAnchorController;

        private Dictionary<int, AnimationStatement> animationDict;

        private Vector3 initialPosition;

        private Transform characterHipNode;

        private Transform selfTransform;

        private GameObject keyPointsParent;

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
                        StopPrevAnimation("");
                    }
                }
            }
        }

        private Vector3 predictHipPos = Vector3.zero;

        private Vector3 localShift = Vector3.zero;

        private String prevAnimationTransitionState;

        private Quaternion predictBodyRotation;

        private float _currentFrequency;

        public float currentFrequency => _currentFrequency;

        private int _currentLeg;

        public int currentLeg => _currentLeg;

        private bool enable;
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value;
#if USE_FINAL_IK
                modelIKSettings.FinalIKComponent.enabled = value;
                modelIKSettings.FinalIKLookAtComponent.enabled = value;
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
            motionDataModel = MotionDataModelHttp.GetInstance();
            motionDataModel.SetPreprocessorParameters(tuningParameters.ScaleMotionPos);
            
            var modelAnimator = this.GetComponent<Animator>();
            characterAnimatorController = new CharacterAnimatorController(modelAnimator);
#if USE_FINAL_IK
            modelIKController = new ModelFinalIKController(modelIKSettings.NodePrefab,
                modelIKSettings.FinalIKComponent, modelIKSettings.FinalIKLookAtComponent);
#else
            modelIKController = new ModelNativeIKController(modelIKSettings.NodePrefab, modelIKSettings.IKScript);
#endif
            animationDict = new Dictionary<int, AnimationStatement>()
            {
                {
                    0,
                    new AnimationStatement
                    {
                        AnimationState = "BasicMotions@Run01 - Forwards", TransitionParameter = "isRun",
                        AnimationPlaySpeedMultiplier = "runPlaySpeed"
                    }
                },
                {
                    1,
                    new AnimationStatement
                    {
                        AnimationState = "BasicMotions@Sprint01 - Forwards", TransitionParameter = "isDash",
                        AnimationPlaySpeedMultiplier = "dashPlaySpeed"
                    }
                }
            };
            
            characterHipNode = modelAnimator.GetBoneTransform(HumanBodyBones.Hips);

            selfTransform = this.transform;
            
            initialPosition = selfTransform.position;
            
            standTravelAnchorController = new StandTravelAnchorController(initialPosition);

            keyPointsParent = new GameObject("KeyPointsParent");

            keyPointsParent.transform.parent = standTravelAnchorController.TravelFollowPoint.transform;

            currentMode = initialMode;

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
            selfTransform.position = initialPosition;
            selfTransform.rotation = Quaternion.identity;

            modelIKController.InitializeIKTargets(keyPointsParent.transform);
        }

        public void Update()
        {
            var currentDeltaTime = Time.deltaTime;
            
            keyPointsList = motionDataModel.GetIKPointsData(true, true);

            if (keyPointsList == null)
            {
                return;
            }

            TryConvertKeyPoints(keyPointsList);

            modelIKController.UpdateIKTargetsData(keyPointsList);

            CalculateBodyRotation(keyPointsList);

            var groundLocationData = motionDataModel.GetGroundLocationData();
            //Debug.Log($"Ground Location: x = {groundLocationData.x}, y = {groundLocationData.y}, z = {groundLocationData.z}");

            predictHipPos.y = groundLocationData.y * tuningParameters.LocalShiftScale.y;
            keyPointsParent.transform.localPosition = predictHipPos;

            var planeShift = new Vector3(-groundLocationData.x * tuningParameters.LocalShiftScale.x, 0,
                -groundLocationData.z * tuningParameters.LocalShiftScale.z);

            localShift = standTravelAnchorController.TravelFollowPoint.transform.rotation * planeShift;

            var actionDetectionData = motionDataModel.GetActionDetectionData();

            if (currentMode == MotionMode.Travel)
            {
                UpdateCharacterAnimation(actionDetectionData, currentDeltaTime);
            }
        }

        public void LateUpdate()
        {
            switch (currentMode)
            {
                case MotionMode.Stand:
                    standTravelAnchorController.TravelFollowPoint.transform.position =
                        standTravelAnchorController.StandFollowPoint.transform.position + localShift;
                    selfTransform.rotation = standTravelAnchorController.TravelFollowPoint.transform.rotation *
                                             predictBodyRotation;
                    break;
                case MotionMode.Travel:
                    standTravelAnchorController.StandFollowPoint.transform.position =
                        standTravelAnchorController.TravelFollowPoint.transform.position - localShift;
                    selfTransform.rotation = standTravelAnchorController.TravelFollowPoint.transform.rotation;
                    break;
            }

            selfTransform.position += Vector3.Scale(predictHipPos, tuningParameters.ScaleMotionPos) +
                                      tuningParameters.HipPosOffset - characterHipNode.position +
                                      standTravelAnchorController.TravelFollowPoint.transform.position;
        }

        public void OnValidate()
        {
            UpdateModelParameters();
        }

        public void OnDestroy()
        {
            Destroy(keyPointsParent);
            motionDataModel = null;
            characterAnimatorController = null;
            standTravelAnchorController?.DestroyObject();
            standTravelAnchorController = null;
            modelIKController?.ClearFakeNodes();
            modelIKController = null;
            animationDict?.Clear();
            animationDict = null;
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
            return currentMode;
        }

        public MotionMode GetCurrentMode()
        {
            return currentMode;
        }

        public Transform GetTravelAnchor()
        {
            return standTravelAnchorController.TravelFollowPoint.transform;
        }

        public Transform GetStandAnchor()
        {
            return standTravelAnchorController.StandFollowPoint.transform;
        }

        public void TurnCharacter(float angle, float dt)
        {
            var deltaRotation = Quaternion.Euler(0,tuningParameters.RotationSensitivity * angle * dt, 0);
            standTravelAnchorController.TurnControlPoints(deltaRotation);
        }

        public List<Vector3> GetKeyPointsList()
        {
            return keyPointsList;
        }

        private void ChangeIKModelWeight(int weight)
        {
            modelIKController.ChangeLowerBodyIKWeight(weight);
        }

        private void UpdateCharacterAnimation(ActionDetectionItem actionDetectionData, float dt)
        {
            
            var walkDetectionData = actionDetectionData.walk;

            if (walkDetectionData != null)
            {
                int runLevels = animatorSettings.runAnimationSettings.Count;

                string animationToPlay = "";
                string animationTransition = "";
                string animationPlaySpeedParam = "";
                float animationPlaySpeed = 0f;
                float movingSpeed = 0f;

                _currentLeg = walkDetectionData.legUp;
                _currentFrequency = walkDetectionData.frequency / 60f;

                for (int i = 0; i < runLevels; i++)
                {
                    var setting = animatorSettings.runAnimationSettings[i];

                    if (_currentFrequency < setting.cadenceThreshold)
                    {
                        break;
                    }

                    animationToPlay = animationDict[(int)setting.animation].AnimationState;
                    animationTransition = animationDict[(int) setting.animation].TransitionParameter;
                    animationPlaySpeedParam = animationDict[(int) setting.animation].AnimationPlaySpeedMultiplier;

                    animationPlaySpeed = setting.playBackSpeed;
                    movingSpeed = setting.movingSpeed;
                }

                if (animationToPlay != "")
                {
                    characterAnimatorController.SetAnimationPlaySpeed(animationPlaySpeedParam, animationPlaySpeed);
                    characterAnimatorController.PlayAnimation(animationToPlay, animationTransition);
                    StopPrevAnimation(animationTransition);

                    MoveCharacter(movingSpeed, dt);
                }
                else
                {
                    StopPrevAnimation("");
                }
            }
        }

        private void StopPrevAnimation(string currentState)
        {
            if (prevAnimationTransitionState != currentState)
            {
                characterAnimatorController.StopAnimation(prevAnimationTransitionState);
                prevAnimationTransitionState = currentState;
            }
        }

        private void MoveCharacter(float speed, float dt)
        {
            var deltaMovement = standTravelAnchorController.TravelFollowPoint.transform.rotation * Vector3.forward *
                                speed * dt;
            standTravelAnchorController.MoveTravelPoint(deltaMovement);
        }

        private void UpdateModelParameters()
        {
            if (motionDataModel != null)
            {
                motionDataModel.SetPreprocessorParameters(tuningParameters.ScaleMotionPos);
            }
        }

        private void CalculateBodyRotation(List<Vector3> keyPoints)
        {
            Vector3 bodyForwardOriginal =
                Vector3.Cross(
                    keyPoints[(int) GameKeyPointsType.LeftShoulder] - keyPoints[(int) GameKeyPointsType.RightHip],
                    keyPoints[(int) GameKeyPointsType.LeftHip] - keyPoints[(int) GameKeyPointsType.RightShoulder]);
            bodyForwardOriginal = Vector3.Normalize(bodyForwardOriginal);

            predictBodyRotation = Quaternion.FromToRotation(Vector3.forward, bodyForwardOriginal);
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
            var locater = GetComponent<WeirdHumanoidPointsLocater>();
            if(locater != null)
            {
                keyPointsConverter = new WeirdHumanoidPointConverter(locater);
            }
        }
    }
}
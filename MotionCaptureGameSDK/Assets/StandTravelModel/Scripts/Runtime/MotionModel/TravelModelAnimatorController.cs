using System.Collections.Generic;
using UnityEngine;
using MotionCaptureBasic.Interface;
using MotionCaptureBasic.OSConnector;

namespace StandTravelModel.Core
{
    public class TravelModelAnimatorController
    {
        public int currentLeg => _currentLeg;
        public float currentFrequency => _currentFrequency;

        private int _currentLeg;
        private float _currentFrequency;
        private string prevAnimationTransitionState;
        private IMotionDataModel motionDataModel;
        private AnimatorSettingGroup animatorSettings;
        private AnchorController anchorController;
        private CharacterAnimatorController characterAnimatorController;
        private Dictionary<int, AnimationStatement> animationDict;

        public TravelModelAnimatorController(Animator modelAnimator, IMotionDataModel motionDataModel, AnimatorSettingGroup animatorSettings)
        {
            this.motionDataModel = motionDataModel;
            this.animatorSettings = animatorSettings;
            characterAnimatorController = new CharacterAnimatorController(modelAnimator);

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
        }

        public void UpdateTravelAnimator()
        {
            UpdateCharacterAnimation(motionDataModel.GetActionDetectionData(), Time.deltaTime);
        }

        public void Clear()
        {
            animationDict?.Clear();
            animationDict = null;
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

        private void MoveCharacter(float speed, float dt)
        {
            var deltaMovement = anchorController.TravelFollowPoint.transform.rotation * Vector3.forward *
                                speed * dt;
            anchorController.MoveTravelPoint(deltaMovement);
        }

        public void StopPrevAnimation(string currentState)
        {
            if (prevAnimationTransitionState != currentState)
            {
                characterAnimatorController.StopAnimation(prevAnimationTransitionState);
                prevAnimationTransitionState = currentState;
            }
        }
    }
}
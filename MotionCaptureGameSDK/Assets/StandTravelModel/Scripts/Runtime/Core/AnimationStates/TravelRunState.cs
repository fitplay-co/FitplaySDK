using StandTravelModel.Scripts.Runtime.MotionModel;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.Core.AnimationStates.Components
{
    public class TravelRunState : AnimationStateBase
    {
        private int animIdIsRun;
        private int animIdRunFreq;
        private RunConditioner runConditioner;
        private ITravelStrideSetter strideSetter;

        private StepStateAnimatorParametersSetter parametersSetter;

        public TravelRunState(MotionModelBase owner, StepStateAnimatorParametersSetter parametersSetter, ITravelStrideSetter strideSetter, RunConditioner runConditioner) : base(owner)
        {
            this.strideSetter = strideSetter;
            this.runConditioner = runConditioner;
            this.animIdIsRun = Animator.StringToHash("isRun");
            this.animIdRunFreq = Animator.StringToHash("runFrequency");
            this.parametersSetter = parametersSetter;
            InitFields(AnimationList.Run);
        }

        public override void Enter()
        {
            base.Enter();
            travelOwner.selfAnimator.SetBool(animIdIsRun, true);
        }

        public override void Tick(float deltaTime)
        {
            var actionDetectionData = travelOwner.selfMotionDataModel.GetActionDetectionData();
            if (actionDetectionData.walk != null)
            {
                
                travelOwner.EnqueueStep(actionDetectionData.walk.legUp);
                travelOwner.currentLeg = actionDetectionData.walk.legUp;
                travelOwner.currentFrequency = actionDetectionData.walk.leftFrequency;
                travelOwner.UpdateAnimatorCadence();
                
                
                var isRunReady = runConditioner.IsEnterRunReady(actionDetectionData.walk, true);
                if (!isRunReady)
                {
                    travelOwner.isRun = false;
                    if (actionDetectionData.walk.leftLeg != 0)
                    {
                        OnTransitionToIdleEnd(AnimationList.LeftStep);
                        return;
                    }
                
                    if (actionDetectionData.walk.rightLeg != 0)
                    {
                        OnTransitionToIdleEnd(AnimationList.RightStep);
                        return;
                    }

                    if (actionDetectionData.walk.leftLeg == 0 && actionDetectionData.walk.rightLeg == 0)
                    {
                        OnTransitionToIdleEnd(AnimationList.Idle);
                        return;
                    }
                }

                //UpdateRunSpeed(actionDetectionData.walk.leftFrequency);
                parametersSetter.TrySetStepParameters();
            }
        }

        public override void Exit()
        {
            base.Exit();
            travelOwner.selfAnimator.SetBool(animIdIsRun, false);
            travelOwner.selfAnimator.SetTrigger("runFade");
        }

        private void UpdateRunSpeed(float stepFrequency)
        {
            travelOwner.selfAnimator.SetFloat(animIdRunFreq, stepFrequency * 0.12f);
        }

        private void OnTransitionToIdleEnd(AnimationList nextState)
        {
            travelOwner.ChangeState(nextState);
        }
    }
}
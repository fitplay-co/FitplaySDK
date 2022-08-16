using StandTravelModel.Scripts.Runtime.MotionModel;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.Core.AnimationStates.Components
{
    public class TravelRunState : AnimationStateBase
    {
        private int animIdIsRun;
        private float exitDelayed;
        private RunConditioner runConditioner;
        private ITravelStrideSetter strideSetter;

        private StepStateAnimatorParametersSetter parametersSetter;

        public TravelRunState(MotionModelBase owner, StepStateAnimatorParametersSetter parametersSetter, ITravelStrideSetter strideSetter, RunConditioner runConditioner) : base(owner)
        {
            this.strideSetter = strideSetter;
            this.runConditioner = runConditioner;
            this.animIdIsRun = Animator.StringToHash("isRun");
            this.parametersSetter = parametersSetter;
            InitFields(AnimationList.Run);
        }

        public override void Enter()
        {
            base.Enter();
            exitDelayed = 0;
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
                    exitDelayed += Time.deltaTime;

                    if(exitDelayed > 0.3f)
                    {
                        travelOwner.isRun = false;
                        if (actionDetectionData.walk.GetLeftLeg() != 0)
                        {
                            OnTransitionToIdleEnd(AnimationList.LeftStep);
                            return;
                        }
                    
                        if (actionDetectionData.walk.GetRightLeg() != 0)
                        {
                            OnTransitionToIdleEnd(AnimationList.RightStep);
                            return;
                        }

                        if (actionDetectionData.walk.GetLeftLeg() == 0 && actionDetectionData.walk.GetRightLeg() == 0)
                        {
                            OnTransitionToIdleEnd(AnimationList.Idle);
                            return;
                        }
                    }
                }

                //UpdateRunSpeed(actionDetectionData.walk.leftFrequency);
                parametersSetter.TrySetStepParameters(true);
            }
        }

        public override void Exit()
        {
            base.Exit();
            exitDelayed = 0;
            travelOwner.selfAnimator.SetBool(animIdIsRun, false);
            travelOwner.selfAnimator.SetTrigger("runFade");
        }

        private void OnTransitionToIdleEnd(AnimationList nextState)
        {
            travelOwner.ChangeState(nextState);
        }
    }
}
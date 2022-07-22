using UnityEngine;
using StandTravelModel.Scripts.Runtime.MotionModel;

namespace StandTravelModel.Scripts.Runtime.Core.AnimationStates.Components
{
    public abstract class TravelBaseState : AnimationStateBase
    {
        private int animStrideId;
        private RunConditioner runConditioner;

        public TravelBaseState(MotionModelBase owner, RunConditioner runConditioner) : base(owner)
        {
            this.runConditioner = runConditioner;
            this.animStrideId = Animator.StringToHash("stride");
            InitFields(GetLegAnimationListSelf());
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

                if (actionDetectionData.walk.leftLeg == 0 && actionDetectionData.walk.rightLeg == 0)
                {
                    travelOwner.ChangeState(AnimationList.Idle);
                    return;
                }
                
                if (runConditioner.IsEnterRunReady(actionDetectionData.walk, false))
                {
                    travelOwner.ChangeState(AnimationList.Run);
                    return;
                }

                if (actionDetectionData.walk.legUp != GetCurrentLeg())
                {
                    travelOwner.ChangeState(GetLegAnimationListOpps());
                    return;
                }

                travelOwner.selfAnimator.SetFloat(animStrideId, Mathf.Clamp01(actionDetectionData.walk.strength * 10));
            }
        }

        protected abstract AnimationList GetLegAnimationListSelf();
        protected abstract AnimationList GetLegAnimationListOpps();
        protected abstract int GetCurrentLeg();
    }
}
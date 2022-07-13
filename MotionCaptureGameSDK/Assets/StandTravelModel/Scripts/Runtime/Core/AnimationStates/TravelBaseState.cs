using StandTravelModel.Scripts.Runtime.MotionModel;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.Core.AnimationStates
{
    public abstract class TravelBaseState : AnimationStateBase
    {
        private int animStrideId;
        private Vector3 velocity;
        private float speedMultipler = 0.6f;

        public TravelBaseState(MotionModelBase owner) : base(owner)
        {
            animStrideId = Animator.StringToHash("stride");
            InitFields(GetLegAnimationListSelf());
        }

        public override void Tick(float deltaTime)
        {
            var actionDetectionData = travelOwner.selfMotionDataModel.GetActionDetectionData();

            if (actionDetectionData.walk != null)
            {
                travelOwner.EnqueueStep(actionDetectionData.walk.legUp);
                travelOwner.currentLeg = actionDetectionData.walk.legUp;
                travelOwner.currentFrequency = actionDetectionData.walk.frequency / 60f;
                travelOwner.UpdateAnimatorCadence();

                if (actionDetectionData.walk.legUp == 0)
                {
                    travelOwner.ChangeState(AnimationList.Idle);
                    return;
                }
                
                var isRunReady = travelOwner.IsEnterRunReady();
                if (actionDetectionData.walk.legUp != 0 && isRunReady)
                {
                    travelOwner.ChangeState(AnimationList.Run);
                    return;
                }

                if (actionDetectionData.walk.legUp != GetCurrentLeg())
                {
                    travelOwner.ChangeState(GetLegAnimationListOpps());
                    return;
                }

                velocity = travelOwner.GetAnchorController().TravelFollowPoint.transform.rotation * Vector3.forward * 
                           travelOwner.currentFrequency * speedMultipler;
                
                travelOwner.UpdateVelocity(velocity);

                //Debug.Log("strength -> " + actionDetectionData.walk.strength);
                travelOwner.selfAnimator.SetFloat(animStrideId, Mathf.Clamp01(actionDetectionData.walk.strength * 10));
            }
        }

        protected abstract AnimationList GetLegAnimationListSelf();
        protected abstract AnimationList GetLegAnimationListOpps();
        protected abstract int GetCurrentLeg();
    }
}
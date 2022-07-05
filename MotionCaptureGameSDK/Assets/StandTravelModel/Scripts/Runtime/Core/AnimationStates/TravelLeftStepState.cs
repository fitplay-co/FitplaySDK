using StandTravelModel.MotionModel;
using UnityEngine;

namespace StandTravelModel.Core.AnimationStates
{
    public class TravelLeftStepState : TravelBaseState
    {
        public TravelLeftStepState(MotionModelBase owner) : base(owner)
        {
        }

        protected override AnimationList GetLegAnimationListOpps()
        {
            return AnimationList.RightStep;
        }

        protected override AnimationList GetLegAnimationListSelf()
        {
            return AnimationList.LeftStep;
        }

        protected override int GetCurrentLeg()
        {
            return -1;
        }
    }
}
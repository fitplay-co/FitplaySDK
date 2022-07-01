using StandTravelModel.MotionModel;
using UnityEngine;

namespace StandTravelModel.Core.AnimationStates
{
    public class TravelRightStepState : TravelBaseState
    {
        public TravelRightStepState(MotionModelBase owner) : base(owner)
        {
        }

        protected override AnimationList GetLegAnimationListOpps()
        {
            return AnimationList.LeftStep;
        }

        protected override AnimationList GetLegAnimationListSelf()
        {
            return AnimationList.RightStep;
        }

        protected override int GetCurrentLeg()
        {
            return 1;
        }
    }
}
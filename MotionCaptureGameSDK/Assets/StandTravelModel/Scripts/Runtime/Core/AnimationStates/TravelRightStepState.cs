using StandTravelModel.MotionModel;

namespace StandTravelModel.Core.AnimationStates
{
    public class TravelRightStepState : TravelStepBase
    {
        public TravelRightStepState(MotionModelBase owner, StepStateAnimatorParametersSetter parametersSetter) : base(owner, parametersSetter)
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
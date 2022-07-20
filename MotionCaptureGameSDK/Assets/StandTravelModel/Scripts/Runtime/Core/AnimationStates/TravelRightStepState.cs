using StandTravelModel.Scripts.Runtime.Core.AnimationStates.Components;
using StandTravelModel.Scripts.Runtime.MotionModel;

namespace StandTravelModel.Scripts.Runtime.Core.AnimationStates
{
    public class TravelRightStepState : TravelStepBase
    {
        public TravelRightStepState(MotionModelBase owner, StepStateAnimatorParametersSetter parametersSetter, ITravelStrideSetter strideSetter) : base(owner, parametersSetter, strideSetter)
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
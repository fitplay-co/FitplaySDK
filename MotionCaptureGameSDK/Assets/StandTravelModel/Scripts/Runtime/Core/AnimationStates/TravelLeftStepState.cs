using StandTravelModel.Scripts.Runtime.Core.AnimationStates.Components;
using StandTravelModel.Scripts.Runtime.MotionModel;

namespace StandTravelModel.Scripts.Runtime.Core.AnimationStates
{
    public class TravelLeftStepState : TravelStepBase
    {
        public TravelLeftStepState(MotionModelBase owner, StepStateAnimatorParametersSetter parametersSetter, RunConditioner runConditioner) : base(owner, parametersSetter, runConditioner)
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
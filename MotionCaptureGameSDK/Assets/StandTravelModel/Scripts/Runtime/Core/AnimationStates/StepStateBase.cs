using StandTravelModel.Scripts.Runtime.Core.AnimationStates.Components;
using StandTravelModel.Scripts.Runtime.MotionModel;

namespace StandTravelModel.Scripts.Runtime.Core.AnimationStates
{
    public abstract class StepStateBase : TravelBaseState
    {
        private StepStateAnimatorParametersSetter parametersSetter;

        protected StepStateBase(MotionModelBase owner, StepStateAnimatorParametersSetter parametersSetter) : base(owner)
        {
            this.parametersSetter = parametersSetter;
        }

        protected void TrySetStepParameters()
        {
            parametersSetter.TrySetParametersLegs();
            parametersSetter.TrySetParametersHipAngles();
            parametersSetter.TrySetParammeterFootHeightDiff();
        }
    }
}
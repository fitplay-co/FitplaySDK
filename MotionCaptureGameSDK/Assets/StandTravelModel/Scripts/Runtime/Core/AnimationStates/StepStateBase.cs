using StandTravelModel.Core.AnimationStates;
using StandTravelModel.MotionModel;

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
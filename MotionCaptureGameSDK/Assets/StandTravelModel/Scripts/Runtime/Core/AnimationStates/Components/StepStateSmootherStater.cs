using UnityEngine;

[System.Serializable]
public class StepStateSmootherStater
{
    [SerializeField] private StepState stepState;
    [SerializeField] private StepState backState;

    public StepState GetStepState()
    {
        return stepState;
    }

    public bool TrySwitchState(int legLeft, int legRight)
    {
        UpdateStepState(legLeft, legRight);

        if(stepState != backState)
        {
            backState = stepState;
            return true;
        }
        return false;
    }

    private void UpdateStepState(int legLeft, int legRight)
    {
        if(stepState == StepState.Idle)
        {
            if(legLeft == 1)
            {
                stepState = StepState.LeftUp;
            }
            else if(legRight == 1)
            {
                stepState = StepState.RightUp;
            }
            return;
        }

        if(stepState == StepState.LeftUp)
        {
            if(legLeft == 0)
            {
                stepState = StepState.Idle;
            }
            else if(legLeft == -1)
            {
                stepState = StepState.LeftDown;
            }
            return;
        }

        if(stepState == StepState.LeftDown)
        {
            if(legRight == 1)
            {
                stepState = StepState.RightUp;
            }
            else if(legLeft == 0)
            {
                stepState = StepState.Idle;
            }
            else if(legLeft == 1)
            {
                stepState = StepState.LeftUp;
            }
            return;
        }

        if(stepState == StepState.RightUp)
        {
            if(legRight == 0)
            {
                stepState = StepState.Idle;
            }
            else if(legRight == -1)
            {
                stepState = StepState.RightDown;
            }
            return;
        }

        if(stepState == StepState.RightDown)
        {
            if(legLeft == 1)
            {
                stepState = StepState.LeftUp;
            }
            else if(legRight == 0)
            {
                stepState = StepState.Idle;
            }
            else if(legRight == 1)
            {
                stepState = StepState.RightUp;
            }
            return;
        }
    }
}
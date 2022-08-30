using UnityEngine;

[System.Serializable]
public class StepStateSmootherStater
{
    [SerializeField] private StepState stepState;
    [SerializeField] private StepState backState;
    [SerializeField] private StepState lastState;

    private StepStateConvertToEvent convertToEvent;

    public StepStateSmootherStater()
    {
        convertToEvent = new StepStateConvertToEvent();
    }

    public StepState GetStepState()
    {
        return stepState;
    }

    public StepState GetLastState()
    {
        return lastState;
    }

    public bool TrySwitchState(int legLeft, int legRight)
    {
        UpdateStepState(legLeft, legRight);

        /* convertToEvent.OnUpdate(legLeft, legRight);
        stepState = convertToEvent.GetState(); */

        //Debug.Log(stepState + " | " + backState + "|||" + Time.frameCount);
        //Debug.Log("-----------------------------------------------------" + Time.frameCount);

        if(stepState != backState)
        {
            lastState = backState;
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
using UnityEngine;

[System.Serializable]
public class StepStateSmoother
{
    private enum StepState
    {
        Idle,
        LeftUp,
        LeftDown,
        RightUp,
        RightDown
    }

    private const float frameCount = 30;
    private const float catchupSpeed = 10f;

    private float backup;

    [SerializeField] private float frameCurr;
    [SerializeField] private float frameTarget;
    [SerializeField] private float frameTargetStart;
    [SerializeField] private float frameTargetEnd;
    [SerializeField] private StepState stepState;

    public void UpdateTargetFrameArea(int legLeft, int legRight)
    {
        UpdateStepState(legLeft, legRight);

        switch(stepState)
        {
            case StepState.LeftUp:
            {
                frameTargetStart = 8;
                frameTargetEnd = 16;
                break;
            }
            case StepState.LeftDown:
            {
                frameTargetStart = 16;
                frameTargetEnd = 24;
                break;
            }
            case StepState.RightUp:
            {
                frameTargetStart = 24;
                frameTargetEnd = 1;
                break;
            }
            case StepState.RightDown:
            {
                frameTargetStart = 1;
                frameTargetEnd = 8;
                break;
            }
        }
    }
    
    public void OnUpdate(float stepProgressLeftUp, float stepProgressLeftDown, float stepProgressRightUp, float stepProgressRightDown)
    {
        if(stepState == StepState.LeftUp)
        {
            frameTarget = LerpFrame(frameTargetStart, frameTargetEnd, stepProgressLeftUp);
            //Debug.Log(Time.frameCount + "left up " + stepProgressLeftUp + "|" + frameTarget);
        }
        else if(stepState == StepState.LeftDown)
        {
            frameTarget = LerpFrame(frameTargetStart, frameTargetEnd, stepProgressLeftDown);
            //Debug.Log(Time.frameCount + "left down " + stepProgressLeftDown + "|" + frameTarget);
        }
        else if(stepState == StepState.RightUp)
        {
            frameTarget = LerpFrame(frameTargetStart, frameTargetEnd, stepProgressRightUp);
            //Debug.Log(Time.frameCount + "right up " + stepProgressRightUp + "|" + frameTarget);
        }
        else if(stepState == StepState.RightDown)
        {
            frameTarget = LerpFrame(frameTargetStart, frameTargetEnd, stepProgressRightDown);
            //Debug.Log(Time.frameCount + "right down " + stepProgressRightDown + "|" + frameTarget);
        }

/*         if(frameTargetStart > frameTargetEnd)
        {
            frameTarget = Mathf.Min(frameTarget, frameCurr);
        } */

        frameCurr = LerpFrame(frameCurr, frameTarget, Time.deltaTime * catchupSpeed, false);

        if(frameTargetStart < frameTargetEnd)
        {
            //frameCurr = Mathf.Min(frameCurr, frameTarget);
        }
        else
        {
            if(frameCurr > frameTargetStart)
            {
                frameCurr = Mathf.Min(frameCurr, frameCount);
            }
            else
            {
                frameCurr = Mathf.Min(frameCurr, frameTargetStart);
            }
        }

        var delta = frameCurr - backup;
        backup = frameCurr;

        /* if(Mathf.Abs(delta) > 20)
        {
            Debug.Break();
        } */
    }

    public float GetStepProgress()
    {
        return frameCurr / frameCount;
    }

    private float LerpFrame(float start, float end, float percent, bool canExceed)
    {
        var frame = LerpFrame(start, end, percent);

        /* if(!canExceed)
        {
            if(start <= end)
            {
                return Mathf.Min(frame, end);
            }
            else
            {
                if(frame < frameCount)
                {
                    return frame;
                }
                return Mathf.Min(frame, end);
            }
        } */
        return frame;
    }

    private float LerpFrame(float start, float end, float percent)
    {
        if(start <= end)
        {
            return start + (end - start) * percent;
        }
        else
        {
            var length = (frameCount - start) + end;
            var target = start + length * percent;

            if(target < frameCount)
            {
                return target;
            }
            return target - frameCount;
        }
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
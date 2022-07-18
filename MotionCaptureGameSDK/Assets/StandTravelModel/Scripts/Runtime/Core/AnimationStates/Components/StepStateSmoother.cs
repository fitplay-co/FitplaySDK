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

    [SerializeField] private float frameCurr;
    [SerializeField] private float frameTarget;
    [SerializeField] private float frameTargetStart;
    [SerializeField] private float frameTargetEnd;
    [SerializeField] private StepState stepState;
    [SerializeField] private StepState bakcState;

    public void UpdateTargetFrameArea(int legLeft, int legRight)
    {
        UpdateStepState(legLeft, legRight);

        if(stepState != bakcState)
        {
            bakcState = stepState;
        }

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

        frameCurr = LerpCurFrame(frameCurr, frameTarget, Time.deltaTime * catchupSpeed);

        /* var delta = Mathf.Abs(frameCurr - frameTarget);
        if(frameCurr > frameTarget && delta < 0.1f)
        {
            frameTarget = frameCurr;
        } */
    }

    public float GetStepProgress()
    {
        return frameCurr / frameCount;
    }

    public float GetTargetProgress()
    {
        return frameTarget / frameCount;
    }

    private float LerpCurFrame(float start, float end, float percent)
    {
        if(start <= end)
        {
            var length = end - start;
            var delta = length * percent;
            delta = Mathf.Clamp(delta, -0.1f, 0.1f);
            var value = start + delta;
            //return Mathf.Min(value, end);
            return value;
        }
        else
        {
            var length = (frameCount - start) + end;
            var delta = length * percent;
            delta = Mathf.Clamp(delta, -0.1f, 0.1f);
            var target = start + delta;

            if(target < frameCount)
            {
                return target;
            }
            return target - frameCount;
        }
    }

    private float LerpFrame(float start, float end, float percent)
    {
        if(start <= end)
        {
            var value = start + (end - start) * percent;
            //return Mathf.Min(value, end);
            return value;
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
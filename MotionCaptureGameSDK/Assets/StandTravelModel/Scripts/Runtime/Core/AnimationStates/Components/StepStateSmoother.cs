using UnityEngine;

[System.Serializable]
public class StepStateSmoother
{
    private const float frameCount = 30;
    private const float catchupSpeed = 10f;

    [SerializeField] private float frameCurr;
    [SerializeField] private float frameTarget;
    [SerializeField] private float frameTargetStart;
    [SerializeField] private float frameTargetEnd;

    private StepStateSmootherStater stater;

    public StepStateSmoother()
    {
        stater = new StepStateSmootherStater();
    }

    public void UpdateTargetFrameArea(int legLeft, int legRight)
    {
        if(stater.TrySwitchState(legLeft, legRight))
        {
            switch(stater.GetStepState())
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
    }
    
    public void OnUpdate(float stepProgressLeftUp, float stepProgressLeftDown, float stepProgressRightUp, float stepProgressRightDown)
    {
        var stepState = stater.GetStepState();
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
        //frameCurr = frameTarget;

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
            //delta = Mathf.Clamp(delta, -0.15f, 0.15f);
            var value = start + delta;
            //return Mathf.Min(value, end);
            return value;
        }
        else
        {
            var length = (frameCount - start) + end;
            var delta = length * percent;
            //delta = Mathf.Clamp(delta, -0.15f, 0.15f);
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
}
using UnityEngine;

public class StepProgressCacher
{
    private float lastAngle;
    private float cacheAngle;
    private AnimationCurve downCurve;
    private AnimationCurve speedCurve;

    public StepProgressCacher(AnimationCurve speedCurve, AnimationCurve downCurve)
    {
        this.downCurve = downCurve;
        this.speedCurve = speedCurve;
    }

    public void GetLegProgress(float hipAngle, out float progressUp, out float progressDown, out float angleDelta)
    {
        progressUp = ConvertHipAngleToProgress(hipAngle);
        //progressDown = downCurve.Evaluate(1 - progressUp);
        //progressUp = downCurve.Evaluate(progressUp);

        progressDown = ConvertHipAngleToProgress(Mathf.Max(hipAngle - 120, 0));
        //progressDown = downCurve.Evaluate(progressDown);
        angleDelta = Mathf.Abs(lastAngle - hipAngle) * 0.1f;
        lastAngle = hipAngle;
    }

    private float ConvertHipAngleToProgress(float angle)
    {
        cacheAngle = Mathf.LerpAngle(cacheAngle, angle, Time.deltaTime * 10);

        var progress = (180f - cacheAngle) / 90f;
        return speedCurve.Evaluate(progress);
    }
}
using UnityEngine;

namespace StandTravelModel.Core.AnimationStates
{
    public class StepProgressCacher
    {
        private float lastAngle;
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
            progressDown = ConverHipAngleToProgressDown(hipAngle);
            angleDelta = Mathf.Abs(lastAngle - hipAngle) * 0.1f;
            lastAngle = hipAngle;
        }

        private float ConvertHipAngleToProgress(float angle)
        {
            return (180f - angle) / 90f;
        }

        private float ConverHipAngleToProgressDown(float angle)
        {
            return Mathf.Max((angle - 120f) / 60f, 0);
        }
    }
}
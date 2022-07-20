using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.Core.AnimationStates.Components
{
    public class StepProgressCacher
    {
        private float lastAngle;
        private float lastProgressUp;
        private float lastProgressDown;
        private AnimationCurve downCurve;
        private AnimationCurve speedCurve;

        public StepProgressCacher(AnimationCurve speedCurve, AnimationCurve downCurve)
        {
            this.downCurve = downCurve;
            this.speedCurve = speedCurve;
        }

        public void GetLegProgress(int leg, float hipAngle, out float progressUp, out float progressDown, out float angleDelta)
        {
            angleDelta = lastAngle - hipAngle;

            if(
                (leg > 0 && angleDelta <= 0) ||
                (leg < 0 && angleDelta >= 0)
                )
            {
                lastAngle = hipAngle;
                progressUp = ConvertHipAngleToProgress(hipAngle);
                progressDown = ConverHipAngleToProgressDown(hipAngle);
                lastProgressUp = progressUp;
                lastProgressDown = progressDown;
            }
            else
            {
                //lastProgressUp += 0.01f * Time.deltaTime;
                //lastProgressDown += 0.01f * Time.deltaTime;
                lastProgressUp += 0.2f * Time.deltaTime;
                lastProgressDown += 0.2f * Time.deltaTime;

                progressUp = lastProgressUp;
                progressDown = lastProgressDown;
            }
        }

        private float ConvertHipAngleToProgress(float angle)
        {
            return (180f - angle) / 90f;
        }

        private float ConverHipAngleToProgressDown(float angle)
        {
            //return (angle - 120f) / 120f;
            return 1 - (180f - angle) / 90f;
        }
    }
}
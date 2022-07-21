using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.Core.AnimationStates.Components
{
    public class StepProgressCacher
    {
        /* private float lastAngle;
        private float lastProgressUp;
        private float lastProgressDown; */
        private AnimationCurve downCurve;
        private AnimationCurve speedCurve;
        private HipAngleSmoother angleSmoother;

        public StepProgressCacher(AnimationCurve speedCurve, AnimationCurve downCurve)
        {
            this.downCurve = downCurve;
            this.speedCurve = speedCurve;
            this.angleSmoother = new HipAngleSmoother();
        }

        public void GetLegProgress(int leg, float hipAngle, out float progressUp, out float progressDown, out float angleDelta)
        {
            /* angleDelta = lastAngle - hipAngle;

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
            } */
            if(leg == 2)
            {
                progressUp = 1;
                progressDown = 0;
            }
            else
            {
                angleSmoother.SwitchLift(leg == 1);
                angleSmoother.OnUpdate(hipAngle);
                progressUp = ConvertHipAngleToProgress(angleSmoother.GetAngleCache());
                progressDown = ConverHipAngleToProgressDown(angleSmoother.GetAngleCache());

                progressUp = speedCurve.Evaluate(progressUp);
                progressDown = downCurve.Evaluate(progressDown);
            }
            
            angleDelta = 0;
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
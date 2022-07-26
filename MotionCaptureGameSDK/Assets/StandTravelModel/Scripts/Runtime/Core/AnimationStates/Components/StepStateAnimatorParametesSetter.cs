using UnityEngine;
using MotionCaptureBasic.OSConnector;
using StandTravelModel.Scripts.Runtime.MotionModel;

namespace StandTravelModel.Scripts.Runtime.Core.AnimationStates.Components
{
    public class StepStateAnimatorParametersSetter
    {
        private int animIdLegLeft;
        private int animIdLegRight;
        private int animIdZeroDelayed;
        private int animIdStepProgress;
        private int animIdStridePercent;
        private int animIdFootHeightDiff;
        private int animIdStepProgressUpLeft;
        private int animIdStepProgressDownLeft;
        private int animIdStepProgressUpRight;
        private int animIdStepProgressDownRight;

        private float zeroDelayed;

        private TravelModel travelOwner;
        private StepStrideCacher strideCacher;
        private StepStateSmoother stepSmoother;
        private StepProgressCacher progressLeft;
        private StepProgressCacher progressRight;
        private ActionDetectionItem actionDetectionItem;

        public StepStateAnimatorParametersSetter(TravelModel travelOwner, AnimationCurve speedCurve, AnimationCurve downCurve, StepStateSmoother stepSmoother, StepStrideCacher strideCacher)
        {
            this.travelOwner = travelOwner;
            this.stepSmoother = stepSmoother;
            this.strideCacher = strideCacher;
            this.progressLeft = new StepProgressCacher(speedCurve, downCurve);
            this.progressRight = new StepProgressCacher(speedCurve, downCurve);
            this.animIdLegLeft = Animator.StringToHash("leftLeg");
            this.animIdLegRight = Animator.StringToHash("rightLeg");
            this.animIdZeroDelayed = Animator.StringToHash("zeroDelayed");
            this.animIdStepProgress = Animator.StringToHash("stepProgress");
            this.animIdStridePercent = Animator.StringToHash("stridePercent");
            this.animIdFootHeightDiff = Animator.StringToHash("footHeightDiff");
            this.animIdStepProgressUpLeft = Animator.StringToHash("progressUpLeft");
            this.animIdStepProgressDownLeft = Animator.StringToHash("progressDownLeft");
            this.animIdStepProgressUpRight = Animator.StringToHash("progressUpRight");
            this.animIdStepProgressDownRight = Animator.StringToHash("progressDownRight");
        }

        public void TrySetParametersLegs()
        {
            actionDetectionItem = travelOwner.selfMotionDataModel.GetActionDetectionData();
            if(actionDetectionItem != null && actionDetectionItem.walk != null)
            {
                var leftLeg = actionDetectionItem.walk.leftLeg;
                var rightLeg = actionDetectionItem.walk.rightLeg;
                travelOwner.selfAnimator.SetInteger(animIdLegLeft, leftLeg);
                travelOwner.selfAnimator.SetInteger(animIdLegRight, rightLeg);

                if(leftLeg == 0 && rightLeg == 0)
                {
                    zeroDelayed += Time.deltaTime;
                }
                else
                {
                    zeroDelayed = 0;
                }

                travelOwner.selfAnimator.SetFloat(animIdZeroDelayed, zeroDelayed);
                stepSmoother.UpdateTargetFrameArea(leftLeg, rightLeg);
            }
        }

        public void TrySetParametersHipAngles()
        {
            if(actionDetectionItem != null && actionDetectionItem.walk != null)
            {
                var angleDeltaLeft = 0f;
                var angleDeltaRight = 0f;
                SetLegParameters(actionDetectionItem.walk.leftLeg, actionDetectionItem.walk.leftHipAng, animIdStepProgressUpLeft, animIdStepProgressDownLeft, true, out angleDeltaLeft);
                SetLegParameters(actionDetectionItem.walk.rightLeg, actionDetectionItem.walk.rightHipAng, animIdStepProgressUpRight, animIdStepProgressDownRight, false, out angleDeltaRight);
                SetStepStateParameters(actionDetectionItem.walk.leftLeg, actionDetectionItem.walk.rightLeg, actionDetectionItem.walk.leftHipAng, actionDetectionItem.walk.rightHipAng);
            }
        }

        public void TrySetParammeterFootHeightDiff()
        {
            if(actionDetectionItem != null && actionDetectionItem.walk != null)
            {
                var diff = Mathf.Abs(actionDetectionItem.walk.leftHipAng - actionDetectionItem.walk.rightHipAng);
                travelOwner.selfAnimator.SetFloat(animIdFootHeightDiff, diff);
            }
        }

        public void TrySetStepParameters()
        {
            TrySetStridePercent();
            TrySetParametersLegs();
            TrySetParametersHipAngles();
            TrySetParammeterFootHeightDiff();
        }

        private void TrySetStridePercent()
        {
            var stridePercent = Mathf.Clamp01(strideCacher.GetStrideSmooth() * 0.5f);
            travelOwner.selfAnimator.SetFloat(animIdStridePercent, stridePercent);
        }

        private void SetLegParameters(int leg, float hipAngle, int idUp, int idDown, bool isLeft, out float angleDelta)
        {
            var progressUp = 0f;
            var progressDown = 0f;

            if(isLeft)
            {
                progressLeft.GetLegProgress(leg, hipAngle, out progressUp, out progressDown, out angleDelta);
            }
            else
            {
                progressRight.GetLegProgress(leg, hipAngle, out progressUp, out progressDown, out angleDelta);
            }

            if(leg == 1)
            {
                travelOwner.selfAnimator.SetFloat(idUp, progressUp);
            }

            if(leg == -1)
            {
                travelOwner.selfAnimator.SetFloat(idDown, progressDown);
            }
        }

        private void SetStepStateParameters(int legLeft, int legRight, float hipAngleLeft, float hipAngleRight)
        {
            var angleDelta = 0f;
            var progressUpLeft = 0f;
            var progressDownLeft = 0f;
            var progressUpRight = 0f;
            var progressDownRight = 0f;

            progressLeft.GetLegProgress(legLeft, hipAngleLeft, out progressUpLeft, out progressDownLeft, out angleDelta);
            progressRight.GetLegProgress(legRight, hipAngleRight, out progressUpRight, out progressDownRight, out angleDelta);

            stepSmoother.OnUpdate(progressUpLeft, progressDownLeft, progressUpRight, progressDownRight);

            travelOwner.selfAnimator.SetFloat(animIdStepProgress, stepSmoother.GetStepProgress());
        }
    }
}
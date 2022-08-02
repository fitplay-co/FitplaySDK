using System;
using UnityEngine;
using MotionCaptureBasic.OSConnector;
using StandTravelModel.Scripts.Runtime.MotionModel;

namespace StandTravelModel.Scripts.Runtime.Core.AnimationStates.Components
{
    public class StepStateAnimatorParametersSetter
    {
        private int animIdLegLeft;
        private int animIdRunSpeed;
        private int animIdLegRight;
        private int animIdZeroDelayed;
        private int animIdStepProgress;
        private int animIdStridePercent;
        private int animIdStrideRunPercent;
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

        private Func<float> strideScale;
        private Func<float> strideScaleRun;

        public StepStateAnimatorParametersSetter(TravelModel travelOwner, AnimationCurve speedCurve, AnimationCurve downCurve, StepStateSmoother stepSmoother, StepStrideCacher strideCacher, Func<float> strideScale, Func<float> strideScaleRun)
        {
            this.travelOwner = travelOwner;
            this.stepSmoother = stepSmoother;
            this.strideCacher = strideCacher;
            this.strideScale = strideScale;
            this.strideScaleRun = strideScaleRun;
            this.progressLeft = new StepProgressCacher(speedCurve, downCurve);
            this.progressRight = new StepProgressCacher(speedCurve, downCurve);
            this.animIdLegLeft = Animator.StringToHash("leftLeg");
            this.animIdRunSpeed = Animator.StringToHash("runSpeed");
            this.animIdLegRight = Animator.StringToHash("rightLeg");
            this.animIdZeroDelayed = Animator.StringToHash("zeroDelayed");
            this.animIdStepProgress = Animator.StringToHash("stepProgress");
            this.animIdStridePercent = Animator.StringToHash("stridePercent");
            this.animIdStrideRunPercent = Animator.StringToHash("stridePercentRun");
            this.animIdFootHeightDiff = Animator.StringToHash("footHeightDiff");
            this.animIdStepProgressUpLeft = Animator.StringToHash("progressUpLeft");
            this.animIdStepProgressDownLeft = Animator.StringToHash("progressDownLeft");
            this.animIdStepProgressUpRight = Animator.StringToHash("progressUpRight");
            this.animIdStepProgressDownRight = Animator.StringToHash("progressDownRight");
        }

        private void TrySetParametersLegs()
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

        private void TrySetParametersHipAngles(bool isRun)
        {
            if(actionDetectionItem != null && actionDetectionItem.walk != null)
            {
                var angleDeltaLeft = 0f;
                var angleDeltaRight = 0f;
                SetLegParameters(actionDetectionItem.walk.leftLeg, actionDetectionItem.walk.leftHipAng, animIdStepProgressUpLeft, animIdStepProgressDownLeft, true, out angleDeltaLeft, isRun);
                SetLegParameters(actionDetectionItem.walk.rightLeg, actionDetectionItem.walk.rightHipAng, animIdStepProgressUpRight, animIdStepProgressDownRight, false, out angleDeltaRight, isRun);
                SetStepStateParameters(actionDetectionItem.walk.leftLeg, actionDetectionItem.walk.rightLeg, actionDetectionItem.walk.leftHipAng, actionDetectionItem.walk.rightHipAng, isRun);
            }
        }

        private void TrySetParammeterFootHeightDiff()
        {
            if(actionDetectionItem != null && actionDetectionItem.walk != null)
            {
                var diff = Mathf.Abs(actionDetectionItem.walk.leftHipAng - actionDetectionItem.walk.rightHipAng);
                travelOwner.selfAnimator.SetFloat(animIdFootHeightDiff, diff);
            }
        }

        public void TrySetIdleParameters()
        {
            TrySetStridePercent();
            TrySetParametersLegs();
            TrySetParammeterFootHeightDiff();
        }

        public void TrySetStepParameters(bool isRun)
        {
            TrySetStridePercent();
            TrySetParametersLegs();
            TrySetParametersHipAngles(isRun);
            TrySetParammeterFootHeightDiff();
            TrySetRunSpeed();
        }

        private void TrySetRunSpeed()
        {
            actionDetectionItem = travelOwner.selfMotionDataModel.GetActionDetectionData();
            if(actionDetectionItem != null && actionDetectionItem.walk != null)
            {
                travelOwner.selfAnimator.SetFloat(animIdRunSpeed, actionDetectionItem.walk.leftFrequency);
            }
        }

        private void TrySetStridePercent()
        {
            //var stridePercent = Mathf.Clamp01(strideCacher.GetStrideSmooth() * 0.75f);
            var stridePercent = strideCacher.GetStrideSmooth() * 0.75f;
            travelOwner.selfAnimator.SetFloat(animIdStridePercent, stridePercent * strideScale());
            travelOwner.selfAnimator.SetFloat(animIdStrideRunPercent, stridePercent * strideScaleRun());
        }

        private void SetLegParameters(int leg, float hipAngle, int idUp, int idDown, bool isLeft, out float angleDelta, bool isRun)
        {
            var progressUp = 0f;
            var progressDown = 0f;

            if(isLeft)
            {
                progressLeft.GetLegProgress(leg, hipAngle, isRun, out progressUp, out progressDown, out angleDelta);
            }
            else
            {
                progressRight.GetLegProgress(leg, hipAngle, isRun, out progressUp, out progressDown, out angleDelta);
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

        private void SetStepStateParameters(int legLeft, int legRight, float hipAngleLeft, float hipAngleRight, bool isRun)
        {
            var angleDelta = 0f;
            var progressUpLeft = 0f;
            var progressDownLeft = 0f;
            var progressUpRight = 0f;
            var progressDownRight = 0f;

            progressLeft.GetLegProgress(legLeft, hipAngleLeft, isRun, out progressUpLeft, out progressDownLeft, out angleDelta);
            progressRight.GetLegProgress(legRight, hipAngleRight, isRun, out progressUpRight, out progressDownRight, out angleDelta, true);

            stepSmoother.OnUpdate(progressUpLeft, progressDownLeft, progressUpRight, progressDownRight);

            travelOwner.selfAnimator.SetFloat(animIdStepProgress, stepSmoother.GetStepProgress());
        }
    }
}
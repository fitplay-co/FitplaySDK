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
        private int animIdStableLeftLeg;
        private int animIdStableRightLeg;
        private int animIdSprintPercent;
        private int animIdWalkRunPercent;
        private int animIdLegRight;
        private int animIdLastLegUp;
        private int animIdZeroDelayed;
        private int animIdZeroDelayedStable;
        private int animIdStepProgress;
        private int animIdStridePercent;
        private int animIdStrideRunPercent;
        private int animIdFootHeightDiff;
        private int animIdStepProgressUpLeft;
        private int animIdStepProgressDownLeft;
        private int animIdStepProgressUpRight;
        private int animIdStepProgressDownRight;

        private int lastUpLeg;
        private float zeroDelayed;
        private float groundedTime;
        private float zeroDelayedStable;

        private TravelModel travelOwner;
        private RunConditioner runConditioner;
        private StepStrideCacher strideCacher;
        private StepStateSmoother stepSmoother;
        private StepProgressCacher progressLeft;
        private StepProgressCacher progressRight;
        private ActionDetectionItem actionDetectionItem;

        private Func<bool> useFreqSprint;
        private Func<float> strideScale;
        private Func<float> strideScaleRun;
        private Func<float> getSprintThrehold;

        public StepStateAnimatorParametersSetter(TravelModel travelOwner, AnimationCurve speedCurve, AnimationCurve downCurve, StepStateSmoother stepSmoother, StepStrideCacher strideCacher, Func<float> strideScale, Func<float> strideScaleRun, Func<bool> useFreqSprint, Func<float> getSprintThrehold, RunConditioner runConditioner)
        {
            WalkActionItem.useRealtimeData = true;
            this.strideScale = strideScale;
            this.travelOwner = travelOwner;
            this.stepSmoother = stepSmoother;
            this.strideCacher = strideCacher;
            this.useFreqSprint = useFreqSprint;
            this.runConditioner = runConditioner;
            this.strideScaleRun = strideScaleRun;
            this.getSprintThrehold = getSprintThrehold;
            this.progressLeft = new StepProgressCacher(speedCurve, downCurve);
            this.progressRight = new StepProgressCacher(speedCurve, downCurve);
            this.animIdLegLeft = Animator.StringToHash("leftLeg");
            this.animIdRunSpeed = Animator.StringToHash("runSpeed");
            this.animIdLegRight = Animator.StringToHash("rightLeg");
            this.animIdLastLegUp = Animator.StringToHash("lastLegUp");
            this.animIdStableLeftLeg = Animator.StringToHash("stableLeftLeg");
            this.animIdStableRightLeg = Animator.StringToHash("stableRightLeg");
            this.animIdZeroDelayed = Animator.StringToHash("zeroDelayed");
            this.animIdStepProgress = Animator.StringToHash("stepProgress");
            this.animIdSprintPercent = Animator.StringToHash("sprintPercent");
            this.animIdWalkRunPercent = Animator.StringToHash("walkRunPercent");
            this.animIdStridePercent = Animator.StringToHash("stridePercent");
            this.animIdStrideRunPercent = Animator.StringToHash("stridePercentRun");
            this.animIdFootHeightDiff = Animator.StringToHash("footHeightDiff");
            this.animIdZeroDelayedStable = Animator.StringToHash("zeroDelayedStable");
            this.animIdStepProgressUpLeft = Animator.StringToHash("progressUpLeft");
            this.animIdStepProgressDownLeft = Animator.StringToHash("progressDownLeft");
            this.animIdStepProgressUpRight = Animator.StringToHash("progressUpRight");
            this.animIdStepProgressDownRight = Animator.StringToHash("progressDownRight");
        }

        public void TryPredicateIdle()
        {
            /* actionDetectionItem = travelOwner.selfMotionDataModel.GetActionDetectionData();
            if(actionDetectionItem != null && actionDetectionItem.walk != null)
            {
                var idleAngle = 159f;
                if(actionDetectionItem.walk.leftHipAng < idleAngle || actionDetectionItem.walk.rightHipAng < idleAngle)
                {
                    groundedTime = 0;
                }
            }
            groundedTime += Time.deltaTime;
            if(IsGrounded())
            {
                travelOwner.selfAnimator.SetInteger(animIdLegLeft, 0);
                travelOwner.selfAnimator.SetInteger(animIdLegRight, 0);
                travelOwner.selfAnimator.SetFloat(animIdZeroDelayed, groundedTime);
            } */
        }

        private void TrySetParametersLegs()
        {
            actionDetectionItem = travelOwner.selfMotionDataModel.GetActionDetectionData();
            if(actionDetectionItem != null && actionDetectionItem.walk != null)
            {
                var leftLeg = actionDetectionItem.walk.GetLeftLeg();
                var rightLeg = actionDetectionItem.walk.GetRightLeg();
                var stableLeftLeg = actionDetectionItem.walk.leftLeg;
                var stableRightLeg = actionDetectionItem.walk.rightLeg;
                travelOwner.selfAnimator.SetInteger(animIdLegLeft, leftLeg);
                travelOwner.selfAnimator.SetInteger(animIdLegRight, rightLeg);
                travelOwner.selfAnimator.SetInteger(animIdStableLeftLeg, stableLeftLeg);
                travelOwner.selfAnimator.SetInteger(animIdStableRightLeg, stableRightLeg);

                if(leftLeg == 0 && rightLeg == 0)
                {
                    zeroDelayed += Time.deltaTime;
                }
                else
                {
                    zeroDelayed = 0;
                }

                if(stableLeftLeg == 0 && stableRightLeg == 0)
                {
                    zeroDelayedStable += Time.deltaTime;
                }
                else
                {
                    zeroDelayedStable = 0;
                }

                travelOwner.selfAnimator.SetFloat(animIdZeroDelayed, zeroDelayed);
                travelOwner.selfAnimator.SetFloat(animIdZeroDelayedStable, zeroDelayedStable);
                stepSmoother.UpdateTargetFrameArea(leftLeg, rightLeg);
                SetLastUpLeg(leftLeg, rightLeg);
            }
        }

        private void SetLastUpLeg(int leftLeg, int rightLeg)
        {
            if(leftLeg == 1)
            {
                lastUpLeg = -1;
            }
            else if(rightLeg == 1)
            {
                lastUpLeg = 1;
            }

            travelOwner.selfAnimator.SetInteger(animIdLastLegUp, lastUpLeg);
        }

        private void TrySetParametersHipAngles(bool isRun)
        {
            if(actionDetectionItem != null && actionDetectionItem.walk != null)
            {
                var angleDeltaLeft = 0f;
                var angleDeltaRight = 0f;
                SetLegParameters(actionDetectionItem.walk.GetLeftLeg(), actionDetectionItem.walk.leftHipAng, animIdStepProgressUpLeft, animIdStepProgressDownLeft, true, out angleDeltaLeft, isRun);
                SetLegParameters(actionDetectionItem.walk.GetRightLeg(), actionDetectionItem.walk.rightHipAng, animIdStepProgressUpRight, animIdStepProgressDownRight, false, out angleDeltaRight, isRun);
                SetStepStateParameters(actionDetectionItem.walk.GetLeftLeg(), actionDetectionItem.walk.GetRightLeg(), actionDetectionItem.walk.leftHipAng, actionDetectionItem.walk.rightHipAng, isRun);
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
            TrySetRunParameters();
        }

        private void TrySetRunParameters()
        {
            actionDetectionItem = travelOwner.selfMotionDataModel.GetActionDetectionData();
            if(actionDetectionItem != null && actionDetectionItem.walk != null)
            {
                travelOwner.selfAnimator.SetFloat(animIdRunSpeed, actionDetectionItem.walk.velocity);
                SetSprintPercent(actionDetectionItem.walk);
                SetWalkRunPercent(actionDetectionItem.walk);
            }
        }

        private void SetSprintPercent(WalkActionItem walk)
        {
            var min = 0f;
            var max = 0f;
            var sprintValue = 0f;
            var sprintPercent = 0f;

            if(!useFreqSprint())
            {
                sprintValue = walk.velocity;
                min = getSprintThrehold();
                max = getSprintThrehold() * 2;
                sprintValue = Mathf.Clamp(sprintValue, min, max) - min;
                sprintPercent = sprintValue / (max - min);
            }
            else
            {
                sprintValue = (runConditioner.GetActTimeLeft() + runConditioner.GetActTimeRight()) * 0.5f;
                min = getSprintThrehold();
                max = 8;
                sprintValue = Mathf.Clamp(sprintValue, min, max) - min;
                sprintPercent = 1f - sprintValue / (max - min);
            }

            travelOwner.selfAnimator.SetFloat(animIdSprintPercent, sprintPercent);
        }

        private void SetWalkRunPercent(WalkActionItem walk)
        {
            travelOwner.selfAnimator.SetFloat(animIdWalkRunPercent, runConditioner.GetWalkRunPercent(walk));
        }

        private void TrySetStridePercent()
        {
            //var stridePercent = Mathf.Clamp01(strideCacher.GetStrideSmooth() * 0.75f);
            var stridePercent = strideCacher.GetStride() * 0.75f;
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

        private bool IsGrounded()
        {
            return groundedTime > 0.15f;
        }
    }
}
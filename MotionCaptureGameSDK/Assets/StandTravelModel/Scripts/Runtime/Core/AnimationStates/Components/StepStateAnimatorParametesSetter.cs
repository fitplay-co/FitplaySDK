using UnityEngine;
using MotionCaptureBasic.OSConnector;
using StandTravelModel.MotionModel;

public class StepStateAnimatorParametersSetter
{
    private int animIdLegLeft;
    private int animIdLegRight;
    private int animIdFootHeightDiff;
    private int animIdStepProgressUpLeft;
    private int animIdStepProgressDownLeft;
    private int animIdStepProgressUpRight;
    private int animIdStepProgressDownRight;

    private TravelModel travelOwner;
    private StepStateSmoother stepSmoother;
    private StepProgressCacher progressLeft;
    private StepProgressCacher progressRight;
    private ActionDetectionItem actionDetectionItem;

    public StepStateAnimatorParametersSetter(TravelModel travelOwner, AnimationCurve speedCurve, AnimationCurve downCurve, StepStateSmoother stepSmoother)
    {
        this.travelOwner = travelOwner;
        this.stepSmoother = stepSmoother;
        this.progressLeft = new StepProgressCacher(speedCurve, downCurve);
        this.progressRight = new StepProgressCacher(speedCurve, downCurve);
        this.animIdLegLeft = Animator.StringToHash("leftLeg");
        this.animIdLegRight = Animator.StringToHash("rightLeg");
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
            //Debug.Log(actionDetectionItem.walk.leftFrequency);
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

            var angleDelta = Mathf.Max(angleDeltaLeft, angleDeltaRight);
            //travelOwner.selfAnimator.Update(Time.deltaTime * angleDelta);
            //Debug.Log("angleDelta -> " + angleDelta);

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

    private void SetLegParameters(int leg, float hipAngle, int idUp, int idDown, bool isLeft, out float angleDelta)
    {
        var progressUp = 0f;
        var progressDown = 0f;

        if(isLeft)
        {
            progressLeft.GetLegProgress(hipAngle, out progressUp, out progressDown, out angleDelta);
        }
        else
        {
            progressRight.GetLegProgress(hipAngle, out progressUp, out progressDown, out angleDelta);
        }

        if(leg == 1)
        {
            travelOwner.selfAnimator.SetFloat(idUp, progressUp);
        }

        if(leg == -1)
        {
            travelOwner.selfAnimator.SetFloat(idDown, progressDown);
        }
        //travelOwner.selfAnimator.SetFloat("speedScale", angleDelta);
    }

    private void SetStepStateParameters(int legLeft, int legRight, float hipAngleLeft, float hipAngleRight)
    {
        var angleDelta = 0f;
        var progressUpLeft = 0f;
        var progressDownLeft = 0f;
        var progressUpRight = 0f;
        var progressDownRight = 0f;

        progressLeft.GetLegProgress(hipAngleLeft, out progressUpLeft, out progressDownLeft, out angleDelta);
        progressRight.GetLegProgress(hipAngleRight, out progressUpRight, out progressDownRight, out angleDelta);

        stepSmoother.OnUpdate(progressUpLeft, progressDownLeft, progressUpRight, progressDownRight);

        var stepProgress = stepSmoother.GetStepProgress();
        travelOwner.selfAnimator.SetFloat("stepProgress", stepProgress);
    }

    public float GetStepProgress()
    {
        return stepSmoother.GetStepProgress();
    }
}
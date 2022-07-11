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
    private AnimationCurve speedCurve;
    private ActionDetectionItem actionDetectionItem;

    private float lastProgress;

    public StepStateAnimatorParametersSetter(TravelModel travelOwner, AnimationCurve speedCurve)
    {
        this.speedCurve = speedCurve;
        this.travelOwner = travelOwner;
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
        }
    }

    public void TrySetParametersHipAngles()
    {
        if(actionDetectionItem != null && actionDetectionItem.walk != null)
        {
            SetLegParameters(actionDetectionItem.walk.leftLeg, actionDetectionItem.walk.leftHipAng, animIdStepProgressUpLeft, animIdStepProgressDownLeft);
            SetLegParameters(actionDetectionItem.walk.rightLeg, actionDetectionItem.walk.rightHipAng, animIdStepProgressUpRight, animIdStepProgressDownRight);
        }
    }

    public void TrySetParammeterFootHeightDiff()
    {
        if(actionDetectionItem != null && actionDetectionItem.walk != null)
        {
            var diff = Mathf.Abs(actionDetectionItem.walk.leftHipAng - actionDetectionItem.walk.rightHipAng) * 0.03f;
            travelOwner.selfAnimator.SetFloat(animIdFootHeightDiff, diff);
        }
    }

    private void SetLegParameters(int leg, float hipAngle, int idUp, int idDown)
    {
        var progressUp = 0f;
        var progressDown = 0f;
        GetLegProgress(leg, hipAngle, out progressUp, out progressDown);

        if(leg == 1)
        {
            travelOwner.selfAnimator.SetFloat(idUp, progressUp);

            if(progressUp <= lastProgress)
            {
                lastProgress = progressUp;
            }
        }

        if(leg == -1)
        {
            travelOwner.selfAnimator.SetFloat(idDown, progressDown);
        }
    }

    private void GetLegProgress(int leg, float hipAngle, out float progressUp, out float progressDown)
    {
        progressUp = ConvertHipAngleToProgress(hipAngle);
        progressDown = 1 - progressUp;

        /* if(leg == 1)
        {
            progressDown = 0;
        }
        else if(leg == -1)
        {
            progressUp = 0;
        } */
    }

    private float ConvertHipAngleToProgress(float angle)
    {
        var progress = (190f - angle) / 90f;
        if(speedCurve != null)
        {
            return speedCurve.Evaluate(progress);
        }  
        return progress;
    }
}
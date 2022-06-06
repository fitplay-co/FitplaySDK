using System;

public abstract class ActionReconArmExtend : ActionReconArm
{
    protected const float foreArmThrehold = 25;
    protected const float upperArmThrehold = 45;

    protected bool isDebug;

    private float lastForearmAngle;

    public ActionReconArmExtend(bool isLeft, Action<ActionId> onAction, bool isDebug) : base(isLeft, onAction)
    {
        this.isDebug = isDebug;
    }

    protected override void CheckExtensionHorizon()
    {
        var inCondArea = InConditionArea();
        var forearmAngle = GetForearmAngle();
        if(!lastState && IsForearmExpanding(forearmAngle) && inCondArea)
        {
            SendEvent(true);
        }
        /* else
        {
            if(lastState && !inCondArea)
            {
                SendEvent(false);
            }
        } */

        lastState = inCondArea;
    }

    protected bool IsRaising()
    {
        return GetUpperarmAngle() < 90;
    }

    protected bool IsForearmExtend()
    {
        return GetForearmAngle() < foreArmThrehold;
    }

    protected bool IsUpperarmRaised()
    {
        return GetUpperarmAngle() < upperArmThrehold;
    }

    protected bool IsUpperarmHorizon()
    {
        var upperAngle = GetUpperarmAngle();

        return upperAngle > 50 && upperAngle < 100;
    }

    protected bool IsForearmExpanding(float forearmAngle)
    {
        var res = forearmAngle < lastForearmAngle;
        lastForearmAngle = forearmAngle;
        return res;
    }

    protected abstract bool InConditionArea();
}
using UnityEngine;

public class HipAngleSmoother
{
    private bool isLifting;
    private float angleCache;
    private const float angleTop = 90;
    private const float angleBottom = 180; 

    public void SwitchLift(bool isLifting)
    {
        this.isLifting = isLifting;
    }

    public void OnUpdate(float curAngle)
    {
        var target = curAngle;

        if(isLifting)
        {
            /* if(curAngle > angleCache)
            {
                target = angleTop;
            } */
            target = angleTop;
        }
        else
        {
            /* if(curAngle < angleCache)
            {
                target = angleBottom;
            } */
            target = angleBottom;
        }

        angleCache = Mathf.Lerp(angleCache, target, Time.deltaTime);
    }

    public float GetAngleCache()
    {
        return angleCache;
    }
}
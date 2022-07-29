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

        //angleCache = Mathf.Lerp(angleCache, target, Time.deltaTime * 0.5f);
        var newAngle = Mathf.Lerp(angleCache, target, Time.deltaTime * 0.35f);
        var deltaMax = Mathf.Min(50, Mathf.Abs(newAngle - angleCache));
        angleCache = Mathf.Clamp(newAngle, newAngle - deltaMax, newAngle + 5);
    }

    public float GetAngleCache()
    {
        return angleCache;
    }
}
using UnityEngine;

public class HipAngleSmoother
{
    private bool isLifting;
    private float angleCache;
    private float cacheSpeed;
    private const float angleTop = 90;
    private const float angleBottom = 180; 

    public void SwitchLift(bool isLifting)
    {
        if(this.isLifting != isLifting)
        {
            this.cacheSpeed = 8f;
        }
        this.isLifting = isLifting;
    }

    public void OnUpdate(float curAngle, bool isRun)
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

        
        /* var newAngle = Mathf.Lerp(angleCache, target, Time.deltaTime * 0.35f);
        var deltaMax = Mathf.Min(50, Mathf.Abs(newAngle - angleCache));
        angleCache = Mathf.Clamp(newAngle, newAngle - deltaMax, newAngle + deltaMax); */
        /* if(Mathf.Abs(angleCache - target) < 1)
        {
            cacheSpeed = 0.1f;
            target = target == angleTop ? angleBottom : angleTop;
        } */

        if(isLifting)
        {
            if(angleCache < angleTop + 1)
            {
                cacheSpeed = 0.1f;
                target = angleBottom;
            }
        }
        else
        {
            if(angleCache > angleBottom - 1)
            {
                cacheSpeed = 0.1f;
                target = angleTop;
            }
        }

        //var percent = isLifting ? cacheSpeed : cacheSpeed * 0.85f * (isRun ? 0.2f : 1);
        //var percent = isLifting ? cacheSpeed : cacheSpeed * 0.85f;
        var percent = isLifting ? cacheSpeed * 1.25f : cacheSpeed;
        angleCache = Mathf.Lerp(angleCache, target, Time.deltaTime * cacheSpeed);
    }

    public float GetAngleCache()
    {
        return angleCache;
    }
}
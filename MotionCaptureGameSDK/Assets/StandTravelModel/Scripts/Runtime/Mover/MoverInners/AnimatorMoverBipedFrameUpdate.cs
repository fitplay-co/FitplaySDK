using UnityEngine;

public class AnimatorMoverBipedFrameUpdate : AnimatorMoverBiped
{
    private float leftFootStart;
    private float leftFootEnd;

    public AnimatorMoverBipedFrameUpdate(Transform transform, float leftFootStart, float leftFootEnd) : base(transform)
    {
        this.leftFootStart = leftFootStart;
        this.leftFootEnd = leftFootEnd;
    }

    protected override int GetTouchingFoot(AnimatorStateInfo stateInfo)
    {
        var isLeft = false;
        var normalizedTime = stateInfo.normalizedTime;
        normalizedTime -= (int)normalizedTime;

        if(leftFootStart < leftFootEnd)
        {
            isLeft = normalizedTime <= leftFootEnd && normalizedTime >= leftFootStart;
        }
        else
        {
            isLeft = normalizedTime >= leftFootEnd && normalizedTime <= leftFootStart;
        }
        return isLeft ? footIndexLeft : footIndexRight;
    }
}
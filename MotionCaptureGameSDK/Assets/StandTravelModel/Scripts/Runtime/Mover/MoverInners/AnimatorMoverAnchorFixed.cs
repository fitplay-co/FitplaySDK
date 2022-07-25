using UnityEngine;

public class AnimatorMoverAnchorFixed : AnimatorMoverBiped
{
    private int anchorFoot;

    public AnimatorMoverAnchorFixed(int anchorFoot, Transform transform) : base(transform)
    {
        this.anchorFoot = anchorFoot;
    }

    protected override int GetTouchingFoot(AnimatorStateInfo stateInfo)
    {
        return anchorFoot;
    }
}
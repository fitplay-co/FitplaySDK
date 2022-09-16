using System;
using StandTravelModel.Scripts.Runtime.Mover.MoverInners;
using UnityEngine;

public class AnimatorMoverBipedAdaption : AnimatorMoverBiped
{
    private Vector3 lastFootLocalPosLeft;
    private Vector3 lastFootLocalPosRight;
    private Vector3 curFootLocalPosLeft;
    private Vector3 curFootLocalPosRight;

    public AnimatorMoverBipedAdaption(Transform transform) : base(transform)
    {
    }

    protected override void UpdatePosWithAnchor()
    {
        base.UpdatePosWithAnchor();

        lastFootLocalPosLeft = curFootLocalPosLeft;
        lastFootLocalPosRight = curFootLocalPosRight;
        curFootLocalPosLeft = GetFootPos(-1);
        curFootLocalPosRight = GetFootPos(1);
    }

    protected override Vector3 GetMoveDelta()
    {
        var deltaLeft = curFootLocalPosLeft - lastFootLocalPosLeft;
        var deltaRight = curFootLocalPosRight - lastFootLocalPosRight;
        var delta = Mathf.Min(deltaLeft.z, deltaRight.z) * GetRunSpeedScale();
        return Vector3.forward * delta;
    }
}
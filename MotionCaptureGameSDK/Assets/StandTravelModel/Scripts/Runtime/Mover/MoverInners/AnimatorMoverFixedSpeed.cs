using System;
using StandTravelModel.Scripts.Runtime.Mover.MoverInners;
using UnityEngine;

public class AnimatorMoverFixedSpeed : AnimatorMoverBiped
{
    private Func<float> getSpeed;

    public AnimatorMoverFixedSpeed(Func<float> getSpeed, Transform transform) : base(transform)
    {
        this.getSpeed = getSpeed;
    }

    protected override Vector3 GetMoveDelta()
    {
        return Vector3.back * getSpeed() * Time.deltaTime;
    }
}
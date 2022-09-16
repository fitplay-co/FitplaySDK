using StandTravelModel.Scripts.Runtime.Mover.MoverInners;
using UnityEngine;

public class AnimatorMoverBipedStepProgressWalk : AnimatorMoverBipedStepProgress
{
    public AnimatorMoverBipedStepProgressWalk(Transform transform, float leftFootStart, float leftFootEnd, AnimatorMoverCompensator[] compensators, float speedScale, bool speedScaleFromPanel) : base(transform, leftFootStart, leftFootEnd, compensators, speedScale, speedScaleFromPanel)
    {
    }

    protected override float GetSpeedScaleFromPanel()
    {
        if(UseSpeedScaleFromPanel())
        {
            return GetWalkSpeedScale();
        }
        return 1;
    }
}
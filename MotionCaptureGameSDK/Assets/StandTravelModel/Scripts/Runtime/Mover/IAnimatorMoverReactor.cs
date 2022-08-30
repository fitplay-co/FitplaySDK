using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.Mover
{
    public interface IAnimatorMoverReactor
    {
        void SetAnimatorDelta(Vector3 moveDest);
        void OnAnimatorMoveStart();
        float GetRunSpeedScale();
    }
}
using UnityEngine;

public interface IAnimatorMoverReactor
{
    void SetAnimatorDelta(Vector3 moveDest);
    void OnAnimatorMoveStart();
}
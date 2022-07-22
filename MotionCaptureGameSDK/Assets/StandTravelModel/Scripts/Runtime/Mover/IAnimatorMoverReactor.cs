using UnityEngine;

public interface IAnimatorMoverReactor
{
    void SetAnimatorDest(Vector3 moveDest);
    void OnAnimatorMoveStart();
}
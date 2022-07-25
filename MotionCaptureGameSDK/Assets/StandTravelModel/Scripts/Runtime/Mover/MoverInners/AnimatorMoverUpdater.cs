using UnityEngine;

public class AnimatorMoverUpdater
{
    private Vector3? moveDelta;
    private CharacterController characterController;

    public AnimatorMoverUpdater(CharacterController characterController)
    {
        this.characterController = characterController;
    }

    public void OnUpdate() {
        if(moveDelta.HasValue)
        {
            characterController.Move(moveDelta.Value);
            moveDelta = null;
        }
    }

    public void SetMoveDelta(Vector3 moveDelta)
    {
        this.moveDelta = moveDelta;
    }

    public Vector3 GetMoveSpeed()
    {
        if(moveDelta.HasValue)
        {
            return moveDelta.Value / Time.deltaTime;
        }
        return Vector3.zero;
    }
}
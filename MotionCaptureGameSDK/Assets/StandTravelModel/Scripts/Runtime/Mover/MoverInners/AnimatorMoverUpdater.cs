using UnityEngine;

public class AnimatorMoverUpdater
{
    private Vector3? moveDest;
    private CharacterController characterController;

    public AnimatorMoverUpdater(CharacterController characterController)
    {
        this.characterController = characterController;
    }

    public void OnUpdate() {
        if(moveDest.HasValue)
        {
            characterController.Move(moveDest.Value);
            moveDest = null;
        }
    }

    public void SetMoveDest(Vector3 moveDest)
    {
        this.moveDest = moveDest;
    }
}
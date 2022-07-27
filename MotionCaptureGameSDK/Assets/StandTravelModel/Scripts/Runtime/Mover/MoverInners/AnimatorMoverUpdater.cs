using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.Mover.MoverInners
{
    public class AnimatorMoverUpdater
    {
        private float asignFrame;
        private Vector3 moveDelta;

        private CharacterController characterController;

        public AnimatorMoverUpdater(CharacterController characterController)
        {
            this.characterController = characterController;
        }

        public void OnUpdate() {
            if(IsValide())
            {
                characterController.Move(moveDelta);
            }
        }

        public void SetMoveDelta(Vector3 moveDelta)
        {
            this.moveDelta = moveDelta;
            this.asignFrame = Time.frameCount;
        }

        public Vector3 GetMoveSpeed()
        {
            if(IsValide())
            {
                return moveDelta / Time.deltaTime;
            }
            return Vector3.zero;
        }

        private bool IsValide()
        {
            return Time.frameCount <= asignFrame + 1;
        }
    }
}
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.Mover.MoverInners
{
    public class AnimatorMoverUpdater
    {
        private float asignFrame;
        private Vector3 moveDelta;
        
        private bool _isSelfMoveControl;
        private CharacterController characterController;

        public AnimatorMoverUpdater(CharacterController characterController)
        {
            this.characterController = characterController;
        }

        public void SetParameter(bool isSelfMoveControl)
        {
            _isSelfMoveControl = isSelfMoveControl;
        }

        public void OnUpdate() {
            if(IsValide() && _isSelfMoveControl)
            {
                characterController.Move(moveDelta);
            }
        }

        public void SetMoveDelta(Vector3 moveDelta)
        {
            this.moveDelta = moveDelta;
            this.asignFrame = Time.frameCount;
        }

        public Vector3 GetMoveSpeed(float dt)
        {
            if(IsValide())
            {
                return moveDelta / dt;
            }
            return Vector3.zero;
        }

        private bool IsValide()
        {
            return Time.frameCount <= asignFrame + 1;
        }
    }
}
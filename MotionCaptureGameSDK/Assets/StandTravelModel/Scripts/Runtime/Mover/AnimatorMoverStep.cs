using StandTravelModel.Scripts.Runtime.Mover.MoverInners;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.Mover
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorMoverStep : MonoBehaviour
    {
        private AnimatorMoverBiped moverBiped;

        private void Awake() {
            this.moverBiped = new AnimatorMoverBiped(transform);
        }

        private void LateUpdate() {
            moverBiped.OnUpdate(default(AnimatorStateInfo));
        }

        /// <summary>
        /// Callback for processing animation movements for modifying root motion.
        /// </summary>
        void OnAnimatorMove()
        {
            //override the root motion
        }
    }
}

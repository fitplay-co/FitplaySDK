using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.TestDemo
{
    public class ExMoveController : MonoBehaviour
    {
        private CharacterController charaControl;
        private StandTravelModelManager standTravelModelManager;
        private AnimatorMover animatorMover;
        private Vector3 deltaMovement;

        public void Awake()
        {
            charaControl = GetComponent<CharacterController>();
            standTravelModelManager = GetComponent<StandTravelModelManager>();
            animatorMover = GetComponent<AnimatorMover>();
            standTravelModelManager.InitPlayerHeightUI();
        }

        public void Update()
        {
            /*if (charaControl != null && standTravelModelManager != null)
            {
                deltaMovement = standTravelModelManager.GetMoveVelocity() * Time.deltaTime;
                charaControl.Move(deltaMovement);
            }*/

            if (charaControl != null && animatorMover != null)
            {
                deltaMovement = animatorMover.velocity * Time.deltaTime;
                charaControl.Move(deltaMovement);
            }
        }
    }
}
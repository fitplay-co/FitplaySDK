using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.TestDemo
{
    public class ExMoveController : MonoBehaviour
    {
        private CharacterController charaControl;
        private StandTravelModelManager standTravelModelManager;
        private AnimatorMover animatorMover;
        private Vector3 deltaMovement;
        
        private bool _isSdkEnable = true;

        private bool isSdkEnable
        {
            get => _isSdkEnable;
            set
            {
                if (_isSdkEnable != value)
                {
                    standTravelModelManager.Enabled = value;
                }
            }
        }

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

            isSdkEnable = standTravelModelManager.GeneralCheck();

            if (charaControl != null && animatorMover != null)
            {
                deltaMovement = animatorMover.velocity * Time.deltaTime;
                charaControl.Move(deltaMovement);
            }
        }
    }
}
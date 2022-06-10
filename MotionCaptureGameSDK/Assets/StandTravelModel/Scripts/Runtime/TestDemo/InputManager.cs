using UnityEngine;

namespace StandTravelModel.TestDemo
{
    public class InputManager : MonoBehaviour
    {
        public StandTravelModelManager standTravelModelManager;

        public void Awake()
        {
            
        }

        public void Start()
        {
            
        }

        public void FixedUpdate()
        {
            
        }

        public void Update()
        {
            var changed = false;

            if(!changed)
            {
                changed = ProcessInput();
            }

            if(!changed)
            {
                changed = ProcessFK();
            }
        }

        public void LateUpdate()
        {
            
        }

        private bool ProcessInput()
        {
            bool isChangeMode = false;
            float deltaTime = Time.deltaTime;

            isChangeMode |= Input.GetKeyDown("joystick button 0");
            isChangeMode |= Input.GetKeyDown(KeyCode.Z);

            if (isChangeMode)
            {
                standTravelModelManager.SwitchStandTravel();
            }

            var mode = standTravelModelManager.GetCurrentMode();

            if (mode == MotionMode.Travel)
            {
                float horizontalAngle = 0;
                var lsh = Input.GetAxis("L_Stick_H");
                var horizontal = Input.GetAxis("Horizontal");
                if (lsh != 0)
                {
                    horizontalAngle = lsh;
                }
                else if (horizontal != 0)
                {
                    horizontalAngle = horizontal;
                }
            
                standTravelModelManager.TurnCharacter(horizontalAngle, deltaTime);
            }

            return isChangeMode;
        }

        private bool ProcessFK()
        {
            var changed = Input.GetKeyDown(KeyCode.X);
            if(changed)
            {
                if(standTravelModelManager.IsFKEnabled())
                {
                    standTravelModelManager.DisableFK();
                }
                else
                {
                    standTravelModelManager.EnableFK();
                }
            }
            return changed;
        }
    }
}
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.TestDemo
{
    public class InputManager : MonoBehaviour
    {
        private const string L_Stick_H = "L_Stick_H";
        private const string Horizontal = "Horizontal";
        private const string joystick_button_0 = "joystick button 0";
        private const string joystick_button_1 = "joystick button 1";
        private const string joystick_button_2 = "joystick button 2";

        public StandTravelModelManager standTravelModelManager;

        public void Update()
        {
            var changed = false;

            if(!changed)
            {
                changed = ProcessInput();
            }
        }

        private bool ProcessInput()
        {
            bool isChangeMode = false;
            float deltaTime = Time.deltaTime;

            isChangeMode |= Input.GetKeyDown(joystick_button_0);
            isChangeMode |= Input.GetKeyDown(KeyCode.Z);

            if (isChangeMode)
            {
                standTravelModelManager.SwitchStandTravel();
            }

            var mode = standTravelModelManager.currentMode;

            if (mode == MotionMode.Travel)
            {
                float horizontalAngle = 0;
                var lsh = Input.GetAxis(L_Stick_H);
                var horizontal = Input.GetAxis(Horizontal);
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
            else if (mode == MotionMode.Stand)
            {
                bool isResetLocalShift = false;
                
                isResetLocalShift |= Input.GetKeyDown(joystick_button_2);
                isResetLocalShift |= Input.GetKeyDown(KeyCode.X);
                
                if (isResetLocalShift)
                {
                    standTravelModelManager.ResetGroundLocation();
                }
            }

            return isChangeMode;
        }
    }
}
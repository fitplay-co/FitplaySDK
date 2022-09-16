using UnityEngine;

using KeyCode = IMU.KeyCode;
using Input = IMU.Input;
namespace StandTravelModel.Scripts.Runtime.TestDemo
{
    public class InputManagerTest : MonoBehaviour
    {
        private const string L_Stick_H = "L_Stick_H";
        private const string Horizontal = "Horizontal";
        private const string joystick_button_0 = "joystick button 0";
        private const string joystick_button_1 = "joystick button 1";
        private const string joystick_button_2 = "joystick button 2";

        public StandTravelModelManager standTravelModelManager;

        public void Update()
        {
            ProcessInput();
        }

        private void ProcessInput()
        {
            float deltaTime = Time.deltaTime;
            var mode = standTravelModelManager.currentMode;

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
            else if (Input.MCTurnValue != 0)
            {
                horizontalAngle = Input.MCTurnValue;
            }
            if (standTravelModelManager.osValidCheck)
            {
                standTravelModelManager.TurnCharacter(horizontalAngle, deltaTime);
            }
            
            if (mode == MotionMode.Stand)
            {
                bool isResetLocalShift = false;
                
                isResetLocalShift |= Input.GetKeyDown(joystick_button_2);
                isResetLocalShift |= Input.GetKeyDown(KeyCode.X);
                
                if (isResetLocalShift)
                {
                    standTravelModelManager.ResetGroundLocation();
                }
            }

            if(Input.GetKeyDown(KeyCode.R))
            {
                standTravelModelManager.ShowPlayerHeightUI();
            }
        }
    }
}
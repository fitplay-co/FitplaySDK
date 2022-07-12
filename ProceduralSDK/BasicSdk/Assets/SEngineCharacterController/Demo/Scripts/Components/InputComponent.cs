using System.Collections.Generic;
using SEngineBasic;
using UnityEngine;

namespace SEngineCharacterController
{
    public class InputComponent : Component
    {
        public IMessage Message { set; private get; }
        public Vector2 LJoystick { private set; get; }
        public Vector2 RJoystick { private set; get; }
        public bool IsJump { private set; get; }

        public float Speed { private set; get; }

        public List<FittingRotationItem> Rotations { private set; get; }

        public override void OnInit()
        {
            Launcher.Instance.RegisterTick(OnTick);
            Launcher.Instance.RegisterFixedTick(OnFixedTick);
            base.OnInit();
        }

        private void OnTick(float dt)
        {
            LJoystick = FitBar.GetInputVector2(EInputKey.Joystick);
            RJoystick = FitBar.GetInputVector2(EInputKey.Joystick, EHandleType.RightHandle);
            if (Message is IKBodyMessage bodyMessage)
            {
                Rotations = bodyMessage.fitting.rotation;
                var walk = bodyMessage.action_detection.walk;
                Speed = walk.legUp == 0 ? 0 : 1;
                
                var jump = bodyMessage.action_detection.jump;
                IsJump = jump.up == 1;
            }
        }

        private void OnFixedTick(float dt)
        {
            
        }
    }
}
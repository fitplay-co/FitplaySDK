using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.TestDemo
{
    public class DisplayHud : MonoBehaviour
    {
        private StandTravelModelManager standTravelModelManager;
        private AnimatorMover animatorMover;
        private CharacterController characterController;

        public void Awake()
        {
            standTravelModelManager = this.GetComponent<StandTravelModelManager>();
            animatorMover = this.GetComponent<AnimatorMover>();
            characterController = this.GetComponent<CharacterController>();
        }

        public void OnGUI()
        {
            GUIStyle labelStyle = new GUIStyle("label");
            labelStyle.fontSize = 30;
            labelStyle.normal.textColor = Color.red;
            GUI.Label(new Rect(20, 40, 300, 40), (standTravelModelManager.Enabled ? "动捕模式": "非动捕模式"), labelStyle);

            switch (standTravelModelManager.currentMode)
            {
                case MotionMode.Stand:
                    GUI.Label(new Rect(20, 80, 300, 40), "Stand模式", labelStyle);
                    break;
                case MotionMode.Travel:
                    GUI.Label(new Rect(20, 80, 300, 40), "Travel模式", labelStyle);
                    break;
            }

            var currentVelocity = animatorMover.velocity;
            var velocityX = Mathf.Round(currentVelocity.x * 100) / 100;
            var velocityY = Mathf.Round(currentVelocity.y * 100) / 100;
            var velocityZ = Mathf.Round(currentVelocity.z * 100) / 100;
            GUI.Label(new Rect(20, 120, 600, 40), $"当速度矢量: x={velocityX}, y={velocityY}, z={velocityZ}", labelStyle);

            var currentSpeed = Mathf.Round(currentVelocity.magnitude * 1000) / 1000;
            GUI.Label(new Rect(20, 160, 300, 40), $"当前速度: {currentSpeed}m/s", labelStyle);

            var isGrounded = animatorMover.isGrounded;
            GUI.Label(new Rect(20, 200, 300, 40), isGrounded ? "在地上" : "悬空", labelStyle);
        }
    }
}
using MotionCaptureBasic;
using MotionCaptureBasic.Interface;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.TestDemo
{
    public class DisplayHudTest : MonoBehaviour
    {
        private StandTravelModelManager standTravelModelManager;
        private AnimatorMover animatorMover;
        private IMotionDataModel motionDataModel;
        private float _currentOsSR = 0;
        private float _currentOsSL = 0;
        private float _currentOsSpeed = 0;
        private float _currentAvatarSpeed = 0;
        private float _SmoothRate = 0.5f;

        public void Awake()
        {
            standTravelModelManager = this.GetComponent<StandTravelModelManager>();
            animatorMover = this.GetComponent<AnimatorMover>();
        }

        public void OnGUI()
        {
            if (motionDataModel == null)
            {
                motionDataModel = standTravelModelManager.motionDataModelReference;
            }

            GUIStyle labelStyle = new GUIStyle("label");
            labelStyle.fontSize = 30;
            labelStyle.normal.textColor = Color.red;
            //GUI.Label(new Rect(20, 40, 300, 40), (standTravelModelManager.Enabled ? "动捕模式": "非动捕模式"), labelStyle);

            switch (standTravelModelManager.currentMode)
            {
                case MotionMode.Stand:
                    GUI.Label(new Rect(20, 40, 300, 40), "Stand模式", labelStyle);
                    break;
                case MotionMode.Travel:
                    GUI.Label(new Rect(20, 40, 300, 40), "Travel模式", labelStyle);
                    break;
            }

            var actionData = motionDataModel.GetActionDetectionData();
            if (actionData != null)
            {
                _currentOsSR = actionData.walk.stepRate;
                _currentOsSL = actionData.walk.stepLen;
                _currentOsSpeed = actionData.walk.stepRate * actionData.walk.stepLen;
            }
            GUI.Label(new Rect(20, 80, 600, 40), $"OS步频: {Mathf.Round(_currentOsSR * 100) / 100}Hz", labelStyle);
            GUI.Label(new Rect(20, 120, 600, 40), $"OS步幅: {Mathf.Round(_currentOsSL * 100) / 100}m", labelStyle);
            GUI.Label(new Rect(20, 160, 600, 40), $"OS速度: {Mathf.Round(_currentOsSpeed * 100) / 100}m/s", labelStyle);

            var currentVelocity = animatorMover.velocity;
            var velocityX = Mathf.Round(currentVelocity.x * 100) / 100;
            var velocityY = Mathf.Round(currentVelocity.y * 100) / 100;
            var velocityZ = Mathf.Round(currentVelocity.z * 100) / 100;
            //GUI.Label(new Rect(20, 160, 600, 40), $"Avatar速度矢量: x={velocityX}, y={velocityY}, z={velocityZ}", labelStyle);

            var target = new Vector3(velocityX, 0, velocityZ).magnitude;
            _currentAvatarSpeed = Mathf.Lerp(_currentAvatarSpeed, target, Time.deltaTime * _SmoothRate);
            GUI.Label(new Rect(20, 200, 600, 40), $"Avatar水平速度: {Mathf.Round(_currentAvatarSpeed * 100) / 100}m/s", labelStyle);

            var isGrounded = animatorMover.isGrounded;
            GUI.Label(new Rect(20, 240, 300, 40), isGrounded ? "在地上" : "悬空", labelStyle);

            var isRun = standTravelModelManager.isRun;
            GUI.Label(new Rect(20, 280, 300, 40), isRun ? "跑起来了" : "没跑", labelStyle);
        }
    }
}
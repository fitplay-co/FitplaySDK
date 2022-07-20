using System;
using UnityEngine;
using UnityWebSocket;

namespace SEngineBasic
{
    public class SEngineBasicDemo : MonoBehaviour
    {
        private static SEngineBasicDemo instance;

        public static SEngineBasicDemo Instance
        {
            get
            {
                return instance;
            }
        }
        private WebsocketOSClient os;
        private void Start()
        {
            instance = this;
            
            os = new WebsocketOSClient();
            FitBar.Clear();
            os.ConnectAsync(state =>
            {
                if (state == WebSocketState.Open)
                {
                    os
                        .SubscribeApplicationClient()
                        .SubscribeGroundLocation(EOSActionType.Subscribe)
                        .SubscribeFitting(EOSActionType.Subscribe, EFittingType.Dual)
                        .SubscribeActionDetection(EOSActionType.Subscribe)
                        .SubscribeGazeTracking(EOSActionType.Subscribe)
                        .SetImuFPS();
                    //.HeartCommand(EHandleType.LeftHandle,EHeartCommandType.OpenHeartRate)
                    //.HeartCommand(EHandleType.RightHandle, EHeartCommandType.OpenBloodOxygen);
                }
            });
        }

        public bool IsStart;
        public float Time;
        private void Update()
        {
            if (IsStart)
            {
                Time -= UnityEngine.Time.deltaTime;
                if (Time <= 0)
                {
                    IsStart = false;
                }
            }
            var message = os?.IMessage as IKBodyMessage;
            if (message != null)
            {
                print($"message:{message == null} {JsonUtility.ToJson(message.action_detection.walk)}");
            }

            var lKeyA = FitBar.GetInputButtonState(EInputKey.KeyA);
            var lKeyACon = FitBar.GetInputButtonState(EInputKey.KeyA, EHandleType.LeftHandle, true);
            
            var lKeyB = FitBar.GetInputButtonState(EInputKey.KeyB);
            var lKeyBCon = FitBar.GetInputButtonState(EInputKey.KeyB, EHandleType.LeftHandle, true);

            var rKeyA = FitBar.GetInputButtonState(EInputKey.KeyA, EHandleType.RightHandle);
            var rKeyCon = FitBar.GetInputButtonState(EInputKey.KeyA, EHandleType.RightHandle, true);
            var lRotation = FitBar.GetImuQuaternion(EImuKey.Rotation);
            var rRotation = FitBar.GetImuQuaternion(EImuKey.Rotation, EHandleType.RightHandle);

            var Joystick = FitBar.GetInputVector2(EInputKey.Joystick);

            var heartRate = FitBar.GetInputValue(EInputValue.HeartRate);
            print(Joystick);
        }

        private void OnDisable()
        {
            os?.CloseAsync();
        }
    }
}
using System.Text;
using UnityEngine;
using UnityWebSocket;

namespace MotionCaptureBasic.MessageSend
{
    public class MessageSender
    {
        private IWebSocket socket;

        public MessageSender(IWebSocket socket)
        {
            this.socket = socket;
        }

        public bool SendMessageRegister()
        {
            return SendAsync(MessageFactory.CreateMessageRegister());
        }

        public bool SubscribeGazeTracking(bool active)
        {
            return SendAsync(MessageFactory.CreateMessageControl(MessageControlFeatureId.gaze_tracking, active));
        }

        public bool SubscribeGroundLocation(bool active)
        {
            return SendAsync(MessageFactory.CreateMessageControl(MessageControlFeatureId.ground_location, active));
        }

        public bool SubscribeActionDetection(bool active)
        {
            return SendAsync(MessageFactory.CreateMessageControl(MessageControlFeatureId.action_detection, active));
        }

        public bool SubscribeFitting(bool active)
        {
            return SendAsync(MessageFactory.CreateMessageFitting(active));
        }
        
        /// <summary>
        /// 控制帧率
        /// </summary>
        /// <param name="fps"></param>
        /// <returns></returns>
        public bool SendFrameRateControl(int fps)
        {
            return SendAsync(MessageFactory.CreateConfigMessage(fps));
        }
        
        /// <summary>
        /// 手柄震动
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="vibrationType"></param>
        /// <param name="strength"></param>
        /// <returns></returns>
        public bool SendVibrationControl(int deviceId, int vibrationType, int strength)
        {
            return SendAsync(MessageFactory.CreateVibrationMessage(deviceId, vibrationType, strength));
        }
        
        /// <summary>
        /// imu重置
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public bool SendImuResetControl(int deviceId)
        {
            return SendAsync(MessageFactory.CreateImuResetMessage(deviceId));
        }
        
        /// <summary>
        /// 心率计控制命令
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool SendHeartControl(int deviceId, int command)
        {
            return SendAsync(MessageFactory.CreateHeartMessage(deviceId, command));
        }

        private bool SendAsync(object message)
        {
            if (socket != null)
            {
                Debug.Log(JsonUtility.ToJson(message));
                socket.SendAsync(Encoding.UTF8.GetBytes(JsonUtility.ToJson(message)));
                return true;
            }

            return false;
        }
    }
}
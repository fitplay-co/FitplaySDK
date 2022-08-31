using System.Text;
using UnityEngine;
using UnityWebSocket;
using Newtonsoft.Json;

namespace MotionCaptureBasic.MessageSend
{
    public class MessageSender
    {
        private IWebSocket socket;

        public MessageSender(IWebSocket socket)
        {
            this.socket = socket;
        }

        /// <summary>
        /// 发送注册帧
        /// </summary>
        /// <returns></returns>
        public bool SendMessageRegister()
        {
            return SendAsync(MessageFactory.CreateMessageRegister());
        }

        /// <summary>
        /// 订阅GazeTracking的控制帧
        /// </summary>
        /// <param name="active">true: 订阅 false: 释放</param>
        /// <returns></returns>
        public bool SubscribeGazeTracking(bool active)
        {
            return SendAsync(MessageFactory.CreateMessageControl(MessageControlFeatureId.gaze_tracking, active));
        }

        /// <summary>
        /// 订阅Ground Location的控制帧
        /// </summary>
        /// <param name="active">true: 订阅 false: 释放</param>
        /// <returns></returns>
        public bool SubscribeGroundLocation(bool active)
        {
            return SendAsync(MessageFactory.CreateMessageControl(MessageControlFeatureId.ground_location, active));
        }

        /// <summary>
        /// 订阅Action Detection的控制帧
        /// </summary>
        /// <param name="active">true: 订阅 false: 释放</param>
        /// <returns></returns>
        public bool SubscribeActionDetection(bool active)
        {
            return SendAsync(MessageFactory.CreateMessageControl(MessageControlFeatureId.action_detection, active));
        }

        /// <summary>
        /// 订阅FK数据的控制帧
        /// </summary>
        /// <param name="active">true: 订阅 false: 释放</param>
        /// <returns></returns>
        public bool SubscribeFitting(bool active)
        {
            return SendAsync(MessageFactory.CreateMessageFitting(active));
        }

        public bool SubscribeGeneral(bool active)
        {
            return SendAsync(MessageFactory.CreateMessageControl(MessageControlFeatureId.general_detection, active));
        }

        public bool ResetGroundLocation()
        {
            return SendAsync(MessageFactory.CreateMessageControl(MessageControlFeatureId.ground_location,
                MessageControlAction.reset));
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
        /// 发送身高设置控制帧
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        public bool SendHeightSetting(int h)
        {
            return SendAsync(MessageFactory.CreateHeightSetMessage(h));
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
                var strMsg = JsonConvert.SerializeObject(message);
                Debug.Log(strMsg);
                socket.SendAsync(Encoding.UTF8.GetBytes(strMsg));
                return true;
            }

            return false;
        }
    }
}
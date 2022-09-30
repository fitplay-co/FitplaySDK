using System.Text;
using UnityEngine;
using UnityWebSocket;
using Newtonsoft.Json;

namespace MotionCaptureBasic.MessageSend
{
    public class MessageSender
    {
        private IWebSocket socket;
        private bool isUseJson;
        private bool isDebug;

        public MessageSender(IWebSocket socket, bool useJson)
        {
            this.socket = socket;
            isUseJson = useJson;
        }

        public void SetDebug(bool isDebug)
        {
            this.isDebug = isDebug;
        }
        
        /// <summary>
        /// 发送注册帧
        /// </summary>
        /// <returns></returns>
        public bool SendMessageRegister()
        {
            //TODO: 实现flatbuffers版发送数据
            // if (isUseJson)
            if (true)
            {
                return SendAsync(MessageFactory.CreateMessageRegister(isUseJson));
            }
            else
            {
                
            }
        }

        /// <summary>
        /// 订阅GazeTracking的控制帧
        /// </summary>
        /// <param name="active">true: 订阅 false: 释放</param>
        /// <returns></returns>
        public bool SubscribeGazeTracking(bool active)
        {
            //TODO: 实现flatbuffers版发送数据
            // if (isUseJson)
            if (true)
            {
                return SendAsync(MessageFactory.CreateMessageControl(MessageControlFeatureId.gaze_tracking, active));
            }
            else
            {
                
            }
        }

        /// <summary>
        /// 订阅Ground Location的控制帧
        /// </summary>
        /// <param name="active">true: 订阅 false: 释放</param>
        /// <returns></returns>
        public bool SubscribeGroundLocation(bool active)
        {
            //TODO: 实现flatbuffers版发送数据
            // if (isUseJson)
            if (true)
            {
                return SendAsync(MessageFactory.CreateMessageControl(MessageControlFeatureId.ground_location, active));
            }
            else
            {
                
            }
        }

        /// <summary>
        /// 订阅Action Detection的控制帧
        /// </summary>
        /// <param name="active">true: 订阅 false: 释放</param>
        /// <returns></returns>
        public bool SubscribeActionDetection(bool active)
        {
            //TODO: 实现flatbuffers版发送数据
            // if (isUseJson)
            if (true)
            {
                return SendAsync(MessageFactory.CreateMessageControl(MessageControlFeatureId.action_detection, active));
            }
            else
            {
                
            }
        }

        /// <summary>
        /// 订阅FK数据的控制帧
        /// </summary>
        /// <param name="active">true: 订阅 false: 释放</param>
        /// <returns></returns>
        public bool SubscribeFitting(bool active)
        {
            //TODO: 实现flatbuffers版发送数据
            // if (isUseJson)
            if (true)
            {
                return SendAsync(MessageFactory.CreateMessageFitting(active));
            }
            else
            {
                
            }
        }

        public bool SubscribeGeneral(bool active)
        {
            //TODO: 实现flatbuffers版发送数据
            // if (isUseJson)
            if (true)
            {
                return SendAsync(MessageFactory.CreateMessageControl(MessageControlFeatureId.general_detection, active));
            }
            else
            {
                
            }
        }

        public bool ResetGroundLocation()
        {
            //TODO: 实现flatbuffers版发送数据
            // if (isUseJson)
            if (true)
            {
                return SendAsync(MessageFactory.CreateMessageControl(MessageControlFeatureId.ground_location,
                    MessageControlAction.reset));
            }
            else
            {
                
            }
        }

        /// <summary>
        /// 控制帧率
        /// </summary>
        /// <param name="fps"></param>
        /// <returns></returns>
        public bool SendFrameRateControl(int fps)
        {
            //TODO: 实现flatbuffers版发送数据
            // if (isUseJson)
            if (true)
            {
                return SendAsync(MessageFactory.CreateConfigMessage(fps));
            }
            else
            {
                
            }
        }

        /// <summary>
        /// 发送身高设置控制帧
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        public bool SendHeightSetting(int h)
        {
            //TODO: 实现flatbuffers版发送数据
            // if (isUseJson)
            if (true)
            {
                return SendAsync(MessageFactory.CreateHeightSetMessage(h));
            }
            else
            {
                
            }
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
            //TODO: 实现flatbuffers版发送数据
            // if (isUseJson)
            if (true)
            {
                return SendAsync(MessageFactory.CreateVibrationMessage(deviceId, vibrationType, strength));
            }
            else
            {
                
            }
        }
        
        /// <summary>
        /// imu重置
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public bool SendImuResetControl(int deviceId)
        {
            //TODO: 实现flatbuffers版发送数据
            // if (isUseJson)
            if (true)
            {
                return SendAsync(MessageFactory.CreateImuResetMessage(deviceId));
            }
            else
            {
                
            }
        }
        
        /// <summary>
        /// 心率计控制命令
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool SendHeartControl(int deviceId, int command)
        {
            //TODO: 实现flatbuffers版发送数据
            // if (isUseJson)
            if (true)
            {
                return SendAsync(MessageFactory.CreateHeartMessage(deviceId, command));
            }
            else
            {
                
            }
        }

        private bool SendAsync(object message)
        {
            if (socket != null)
            {
                var strMsg = JsonConvert.SerializeObject(message);
                if (isDebug)
                {
                    Debug.Log(strMsg);
                }
                
                socket.SendAsync(Encoding.UTF8.GetBytes(strMsg));
                return true;
            }

            return false;
        }
    }
}
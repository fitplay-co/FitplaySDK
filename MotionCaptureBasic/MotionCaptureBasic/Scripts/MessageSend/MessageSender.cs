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

        public MessageSender(IWebSocket socket, bool useJson, bool isDebug)
        {
            this.socket = socket;
            this.isUseJson = useJson;
            this.isDebug = isDebug;
        }

        /// <summary>
        /// 发送注册帧
        /// </summary>
        /// <returns></returns>
        public bool SendMessageRegister()
        {
            if (isUseJson)
            //if (true)
            {
                return SendAsync(MessageFactory.CreateMessageRegister(isUseJson));
            }
            else
            {
                return SendAsyncFlatbuffer(MessageFlatbufferFactory.CreateFlatbufferRegister(isUseJson));
            }
        }

        /// <summary>
        /// 订阅GazeTracking的控制帧
        /// </summary>
        /// <param name="active">true: 订阅 false: 释放</param>
        /// <returns></returns>
        public bool SubscribeGazeTracking(bool active)
        {
            if (isUseJson)
            //if (true)
            {
                return SendAsync(MessageFactory.CreateMessageControl(MessageControlFeatureId.gaze_tracking, active));
            }
            else
            {
                Debug.Log("订阅GazeTracking的控制帧未实现");
                return false;
            }
        }

        /// <summary>
        /// 订阅Ground Location的控制帧
        /// </summary>
        /// <param name="active">true: 订阅 false: 释放</param>
        /// <returns></returns>
        public bool SubscribeGroundLocation(bool active)
        {
            if (isUseJson)
            //if (true)
            {
                return SendAsync(MessageFactory.CreateMessageControl(MessageControlFeatureId.ground_location, active));
            }
            else
            {
                return SendAsyncFlatbuffer(
                    MessageFlatbufferFactory.CreateFlatbufferControl(MessageControlFeatureId.ground_location, active));
            }
        }

        /// <summary>
        /// 订阅Action Detection的控制帧
        /// </summary>
        /// <param name="active">true: 订阅 false: 释放</param>
        /// <returns></returns>
        public bool SubscribeActionDetection(bool active)
        {
            if (isUseJson)
            //if (true)
            {
                return SendAsync(MessageFactory.CreateMessageControl(MessageControlFeatureId.action_detection, active));
            }
            else
            {
                return SendAsyncFlatbuffer(
                    MessageFlatbufferFactory.CreateFlatbufferControl(MessageControlFeatureId.action_detection, active));
            }
        }

        /// <summary>
        /// 订阅FK数据的控制帧
        /// </summary>
        /// <param name="active">true: 订阅 false: 释放</param>
        /// <returns></returns>
        public bool SubscribeFitting(bool active)
        {
            if (isUseJson)
            //if (true)
            {
                return SendAsync(MessageFactory.CreateMessageFitting(active));
            }
            else
            {
                return SendAsyncFlatbuffer(
                    MessageFlatbufferFactory.CreateFlatbufferControl(MessageControlFeatureId.fitting, active));
            }
        }

        public bool SubscribeGeneral(bool active)
        {
            if (isUseJson)
            //if (true)
            {
                return SendAsync(MessageFactory.CreateMessageControl(MessageControlFeatureId.general_detection, active));
            }
            else
            {
                return SendAsyncFlatbuffer(
                    MessageFlatbufferFactory.CreateFlatbufferControl(MessageControlFeatureId.general_detection,
                        active));
            }
        }

        public bool ResetGroundLocation()
        {
            if (isUseJson)
            //if (true)
            {
                return SendAsync(MessageFactory.CreateMessageControl(MessageControlFeatureId.ground_location,
                    MessageControlAction.reset));
            }
            else
            {
                return SendAsyncFlatbuffer(
                    MessageFlatbufferFactory.CreateFlatbufferControl(MessageControlFeatureId.ground_location, MessageControlAction.reset));
            }
        }

        /// <summary>
        /// 控制帧率
        /// </summary>
        /// <param name="fps"></param>
        /// <returns></returns>
        public bool SendFrameRateControl(int fps)
        {
            if (isUseJson)
            //if (true)
            {
                return SendAsync(MessageFactory.CreateConfigMessage(fps));
            }
            else
            {
                Debug.Log("控制帧率未实现");
                return false;
            }
        }

        /// <summary>
        /// 发送身高设置控制帧
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        public bool SendHeightSetting(int h)
        {
            if (isUseJson) 
            //if (true)
            {
                return SendAsync(MessageFactory.CreateHeightSetMessage(h));
            }
            else
            {
                return SendAsyncFlatbuffer(MessageFlatbufferFactory.CreateHeightSetFlatbuffer(h));
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
            if (isUseJson)
            //if (true)
            {
                return SendAsync(MessageFactory.CreateVibrationMessage(deviceId, vibrationType, strength));
            }
            else
            {
                Debug.Log("手柄震动未实现");
                return false;
            }
        }
        
        /// <summary>
        /// imu重置
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public bool SendImuResetControl(int deviceId)
        {
            if (isUseJson)
            //if (true)
            {
                return SendAsync(MessageFactory.CreateImuResetMessage(deviceId));
            }
            else
            {
                Debug.Log("imu重置为实现");
                return false;
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
            if (isUseJson)
            //if (true)
            {
                return SendAsync(MessageFactory.CreateHeartMessage(deviceId, command));
            }
            else
            {
                Debug.Log("心率计控制命令未实现");
                return false;
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

        private bool SendAsyncFlatbuffer(byte[] buf)
        {
            if (socket != null)
            {
                socket.SendAsync(buf);
                return true;
            }

            return false;
        }
    }
}
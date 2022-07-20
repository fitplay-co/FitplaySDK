using System.Text;
using UnityEngine;
using UnityWebSocket;

namespace SEngineBasic
{
    public static class WebsocketOSClientExtend
    {
        /// <summary>
        /// 引擎数据注册
        /// </summary>
        /// <returns></returns>
        public static WebsocketOSClient SubscribeApplicationClient(this WebsocketOSClient websocketOsClient)
        {
            return Send(websocketOsClient, new ApplicationClient(null));
        }

        /// <summary>
        /// 控制帧:ground_location
        /// </summary>
        /// <returns></returns>
        public static WebsocketOSClient SubscribeGroundLocation(this WebsocketOSClient websocketOsClient, EOSActionType actionType)
        {
            return Send(websocketOsClient, new GroundLocationControl(actionType));
        }
        
        /// <summary>
        /// 控制帧:gaze_tracking
        /// </summary>
        /// <returns></returns>
        public static WebsocketOSClient SubscribeGazeTracking(this WebsocketOSClient websocketOsClient, EOSActionType actionType)
        {
            return Send(websocketOsClient, new GazeTrackingControl(actionType));
        }

        /// <summary>
        /// 控制帧:action_detection
        /// </summary>
        /// <returns></returns>
        public static WebsocketOSClient SubscribeActionDetection(this WebsocketOSClient websocketOsClient, EOSActionType actionType)
        {
            return Send(websocketOsClient, new ActionDetectionControl(actionType));
        }
        
        /// <summary>
        /// 控制帧:fitting
        /// </summary>
        /// <returns></returns>
        public static WebsocketOSClient SubscribeFitting(this WebsocketOSClient websocketOsClient, EOSActionType actionType, EFittingType fittingType)
        {
            return Send(websocketOsClient, new FittingControl(actionType, fittingType));
        }
        
        /// <summary>
        /// 设置imu fps
        /// </summary>
        /// <param name="websocketOsClient"></param>
        /// <param name="fps"></param>
        /// <returns></returns>
        public static WebsocketOSClient SetImuFPS(this WebsocketOSClient websocketOsClient,int fps = 60)
        {
            return Send(websocketOsClient, new ImuFPS(fps));
        }

        /// <summary>
        /// imu 置零
        /// </summary>
        /// <param name="websocketOsClient"></param>
        /// <param name="handleType"></param>
        /// <returns></returns>
        public static WebsocketOSClient SetImuZero(this WebsocketOSClient websocketOsClient, EHandleType handleType)
        {
            return Send(websocketOsClient, new ImuZero(handleType));
        }

        /// <summary>
        /// imu 振动
        /// </summary>
        /// <param name="websocketOsClient"></param>
        /// <param name="handleType"></param>
        /// <param name="vibration_type">振动效果ID：0~10</param>
        /// <param name="strength">振动强度 0~100</param>
        /// <returns></returns>
        public static WebsocketOSClient SetImuVibration(this WebsocketOSClient websocketOsClient, EHandleType handleType, int vibration_type, int strength)
        {
            return Send(websocketOsClient, new ImuVibration(handleType, vibration_type, strength));
        }

        /// <summary>
        /// 心率控制命令
        /// </summary>
        /// <param name="websocketOsClient"></param>
        /// <param name="handleType"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static WebsocketOSClient HeartCommand(this WebsocketOSClient websocketOsClient, EHandleType handleType, EHeartCommandType commandType)
        {
            return Send(websocketOsClient, new InputHeart(handleType, commandType));
        }

        private static WebsocketOSClient Send(this WebsocketOSClient websocketOsClient, OSApplicationBase data)
        {
            if (websocketOsClient?.Socket != null && websocketOsClient.Socket.ReadyState == WebSocketState.Open)
            {
                var encoded = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
                websocketOsClient.Socket.SendAsync(encoded);
            }
            //Debug.Log($"send:{JsonUtility.ToJson(data)}");
            return websocketOsClient;
        }
    }
}
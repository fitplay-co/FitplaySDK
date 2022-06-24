using System;
using UnityWebSocket;
using MotionCaptureBasic.MessageSend;
using UnityEngine;

namespace MotionCaptureBasic.OSConnector
{
    public class WebsocketOSClient
    {
        public delegate void ReceiveAction(string message);

        public event Action OnConnect;
        public event ReceiveAction OnReceived;
        

        private string url = "ws://127.0.0.1:8181/";
        private IWebSocket socket;
        private MessageSender messageSubscriber;  

        private WebsocketOSClient()
        {
            InitConnect();
            InitMessageSubscriber(socket);
        }
        
        private static WebsocketOSClient instance;
        
        private static readonly object _Synchronized = new object();

        public static WebsocketOSClient GetInstance()
        {
            if (instance == null)
            {
                lock(_Synchronized)
                {
                    if(instance == null)
                    {
                        instance = new WebsocketOSClient();
                    }
                }
            }

            return instance;
        }
        
        ~WebsocketOSClient()
        {
            if (socket != null && socket.ReadyState != UnityWebSocket.WebSocketState.Closed)
            {
                socket.CloseAsync();
            }
            Console.WriteLine("WebSocket closed.");
        }

        public bool SendMessageRegister()
        {
            if (messageSubscriber != null)
            {
                return messageSubscriber.SendMessageRegister();
            }
            return false;
        }

        public bool SubscribeGazeTracking(bool active)
        {
            if (messageSubscriber != null)
            {
                return messageSubscriber.SubscribeGazeTracking(active);
            }
            return false;
        }

        public bool SubscribeGroundLocation(bool active)
        {
            if (messageSubscriber != null)
            {
                return messageSubscriber.SubscribeGroundLocation(active);
            }
            return false;
        }

        public bool SubscribeActionDetection(bool active)
        {
            if (messageSubscriber != null)
            {
                return messageSubscriber.SubscribeActionDetection(active);
            }
            return false;
        }

        public bool SubscribeFitting(bool active)
        {
            if (messageSubscriber != null)
            {
                return messageSubscriber.SubscribeFitting(active);
            }

            return false;
        }
        
        /// <summary>
        /// 设置FPS
        /// </summary>
        /// <param name="fps"></param>
        /// <returns></returns>
        public bool SendFrameRateControl(int fps)
        {
            if (messageSubscriber != null)
            {
                return messageSubscriber.SendFrameRateControl(fps);
            }
            return false;
        }
        /// <summary>
        ///  震动
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="vibrationType"></param>
        /// <param name="strength"></param>
        /// <returns></returns>
        public bool SendVibrationControl(int deviceId, int vibrationType, int strength)
        {
            if (messageSubscriber != null)
            {
                return messageSubscriber.SendVibrationControl(deviceId, vibrationType, strength);
            }
            return false;
        }
        
        /// <summary>
        ///重置imu
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public bool SendImuResetControl(int deviceId)
        {
            if (messageSubscriber != null)
            {
                return messageSubscriber.SendImuResetControl(deviceId);
            }
            return false;
        }

        /// <summary>
        /// 心率计控制
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool SendHeartControl(int deviceId, int command)
        {
            if (messageSubscriber != null)
            {
                return messageSubscriber.SendHeartControl(deviceId, command);
            }
            return false;
        }
        //start
        private void InitConnect()
        {
            socket = new UnityWebSocket.WebSocket(url);
            socket.OnOpen += Socket_OnOpen;
            socket.OnMessage += Socket_OnMessage;
            socket.OnClose += Socket_OnClose;
            socket.OnError += Socket_OnError;
            socket.ConnectAsync();
        }

        private void InitMessageSubscriber(IWebSocket webSocket)
        {
            messageSubscriber = new MessageSender(webSocket);
        }
        
        private void Socket_OnOpen(object sender, OpenEventArgs e)
        {
            messageSubscriber.SendMessageRegister();
            
            OnConnect?.Invoke();
        }
        
        private void Socket_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.IsBinary)
            {
           //     Debug.LogError(string.Format("Receive Bytes ({1}): {0}", e.Data, e.RawData.Length));
            }
            else if (e.IsText)
            {
               // Debug.LogError(string.Format("Receive: {0}", e.Data));
                OnReceived?.Invoke(e.Data);
            }
        }
        
        private void Socket_OnClose(object sender, CloseEventArgs e)
        {
            Console.WriteLine("Closed: StatusCode: {0}, Reason: {1}", e.StatusCode, e.Reason);
        }

        private void Socket_OnError(object sender, UnityWebSocket.ErrorEventArgs e)
        {
            Console.WriteLine("Error: {0}", e.Message);
        }
    }
}
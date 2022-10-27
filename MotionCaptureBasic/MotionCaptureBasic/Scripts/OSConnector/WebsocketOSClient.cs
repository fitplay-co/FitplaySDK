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
        public event Action OnClosed;
        public event Action OnError;
        public event ReceiveAction OnReceived;
        public event Action<byte[]> OnReceivedBytes;


        private IWebSocket socket;
        private MessageSender messageSubscriber;
        private bool isUseJson = false;

        private WebsocketOSClient()
        {
        }

        private static WebsocketOSClient instance;

        private static readonly object _Synchronized = new object();

        public static WebsocketOSClient GetInstance()
        {
            if (instance == null)
            {
                lock (_Synchronized)
                {
                    if (instance == null)
                    {
                        instance = new WebsocketOSClient();
                    }
                }
            }

            return instance;
        }

        ~WebsocketOSClient()
        {
            ReleaseConnect();
            Console.WriteLine("WebSocket closed.");
        }

        public bool SendMessageRegister(bool useJson)
        {
            if (messageSubscriber != null)
            {
                return messageSubscriber.SendMessageRegister();
            }

            return false;
        }

        public void SetUseJson(bool useJson)
        {
            isUseJson = useJson;
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

        public bool SubscribeGeneral(bool active)
        {
            if (messageSubscriber != null)
            {
                return messageSubscriber.SubscribeGeneral(active);
            }

            return false;
        }

        public bool ResetGroundLocation()
        {
            if (messageSubscriber != null)
            {
                return messageSubscriber.ResetGroundLocation();
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
        /// 设置身高
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        public bool SendHeightSetting(int h)
        {
            if (messageSubscriber != null)
            {
                return messageSubscriber.SendHeightSetting(h);
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
        public void InitConnect(string webSocketUrl)
        {
            string url = $"ws://{webSocketUrl}:8181/";
            Debug.Log($"url:{url}");
            if (IsConnected)
            {
                ReleaseConnect();
            }
            socket = new UnityWebSocket.WebSocket(url);
            socket.OnOpen += Socket_OnOpen;
            socket.OnMessage += Socket_OnMessage;
            socket.OnClose += Socket_OnClose;
            socket.OnError += Socket_OnError;
            socket.ConnectAsync();

            InitMessageSubscriber(socket);
        }

        public void ReleaseConnect()
        {
            socket.CloseAsync();
            socket.OnOpen -= Socket_OnOpen;
            socket.OnMessage -= Socket_OnMessage;
            socket.OnClose -= Socket_OnClose;
            socket.OnError -= Socket_OnError;
            socket = null;
        }

        public bool IsConnected
        {
            get => (socket != null && socket.ReadyState != UnityWebSocket.WebSocketState.Connecting);
        }

        private void InitMessageSubscriber(IWebSocket webSocket)
        {
            messageSubscriber = new MessageSender(webSocket, isUseJson);
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
                //Debug.LogError(string.Format("Receive Bytes ({1}): {0}", e.Data, e.RawData.Length));
                OnReceivedBytes?.Invoke(e.RawData);
            }
            else if (e.IsText)
            {
                // Debug.LogError(string.Format("Receive: {0}", e.Data));
                OnReceived?.Invoke(e.Data);
            }
        }

        private void Socket_OnClose(object sender, CloseEventArgs e)
        {
            OnClosed?.Invoke();
            Console.WriteLine("Closed: StatusCode: {0}, Reason: {1}", e.StatusCode, e.Reason);
        }

        private void Socket_OnError(object sender, UnityWebSocket.ErrorEventArgs e)
        {
            OnError?.Invoke();
            Console.WriteLine("Error: {0}", e.Message);
        }

        public void SetDebug(bool isDebug)
        {
            messageSubscriber.SetDebug(isDebug);
        }
    }
}
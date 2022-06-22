using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityWebSocket;
using WebSocketState = System.Net.WebSockets.WebSocketState;

namespace IMU
{
    public class ImuManager : MonoBehaviour
    {
        public delegate void ReceiveAction(string message);

        public event ReceiveAction OnReceived;
        private ClientWebSocket webSocket = null;
        private bool _isConnected = false;
        [SerializeField] private bool autoConnect = true;

        [SerializeField] private string url = "ws://127.0.0.1:8181/"; //get packed message from OS server
        // private string url = "ws://127.0.0.1:9081/"; //get original message from IMU

        private static ImuManager _instance;

        public static ImuManager Instance
        {
            get { return _instance; }
        }

        public static ImuManager Create()
        {
            var proto = FindObjectOfType<ImuManager>();
            if (proto != null) return proto;
            var go = new GameObject("IMUManager");
            proto = go.AddComponent<ImuManager>();
            return proto;
        }

        public bool IsConnected
        {
            get => _isConnected;
        }

        public void Connect()
        {
            if (!IsConnected)
                ConnectToServer();
        }

        void Start()
        {
            _isConnected = false;
            _instance = this;
            if (autoConnect)
            {
                InitConnect();
            }
        }

        void OnDestroy()
        {
            if (webSocket != null)
                webSocket.Dispose();

            if (socket != null && socket.ReadyState != UnityWebSocket.WebSocketState.Closed)
            {
                socket.CloseAsync();
            }

            Debug.Log("WebSocket closed.");
        }


        public void ConnectToServer()
        {
            InitConnect();
        }

        //start
        private IWebSocket socket;

        private void InitConnect()
        {
            socket = new UnityWebSocket.WebSocket(url);
            socket.OnOpen += Socket_OnOpen; //todo
            socket.OnMessage += Socket_OnMessage;
            socket.OnClose += Socket_OnClose;
            socket.OnError += Socket_OnError;
            socket.ConnectAsync();
        }

        private void Socket_OnOpen(object sender, OpenEventArgs e)
        {
            var message = new AppClientMessage();
            var encoded = Encoding.UTF8.GetBytes(JsonUtility.ToJson(message));
            socket.SendAsync(encoded);
            var buffer = new ArraySegment<Byte>(encoded, 0, encoded.Length);
            _isConnected = true;
            // socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private void Socket_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.IsBinary)
            {
                //Debug.LogError(string.Format("Receive Bytes ({1}): {0}", e.Data, e.RawData.Length));
            }
            else if (e.IsText)
            {
                // Debug.LogError(string.Format("Receive: {0}", e.Data));
                OnReceived?.Invoke(e.Data);
            }
        }

        private void Socket_OnClose(object sender, CloseEventArgs e)
        {
            _isConnected = false;
            Debug.LogError(string.Format("Closed: StatusCode: {0}, Reason: {1}", e.StatusCode, e.Reason));
        }

        private void Socket_OnError(object sender, UnityWebSocket.ErrorEventArgs e)
        {
            Debug.LogError(string.Format("Error: {0}", e.Message));
        }
        //end

        public async Task Connect(string uri)
        {
            try
            {
                Debug.Log("WebSocket start connection.");
                webSocket = new ClientWebSocket();
                await webSocket.ConnectAsync(new Uri(uri), CancellationToken.None);
                Debug.Log("WebSocket connected.");

                Debug.Log(webSocket.State);
                var message = new AppClientMessage();
                await Send(JsonUtility.ToJson(message));
                while (webSocket.State == WebSocketState.Open)
                {
                    await Receive();
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }

        private class AppClientMessage
        {
            public string type = "application_client";
        }

        private async Task Send(string message)
        {
            var encoded = Encoding.UTF8.GetBytes(message);
            var buffer = new ArraySegment<Byte>(encoded, 0, encoded.Length);

            await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task Receive()
        {
            ArraySegment<Byte> buffer = new ArraySegment<byte>(new Byte[8192]);

            while (webSocket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = null;

                using (var ms = new MemoryStream())
                {
                    do
                    {
                        result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
                        ms.Write(buffer.Array, buffer.Offset, result.Count);
                    } while (!result.EndOfMessage);

                    ms.Seek(0, SeekOrigin.Begin);

                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        using (var reader = new StreamReader(ms, Encoding.UTF8))
                        {
                            string message = reader.ReadToEnd();
                            OnReceived?.Invoke(message);
                        }
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty,
                            CancellationToken.None);
                    }
                }
            }
        }
    }
}
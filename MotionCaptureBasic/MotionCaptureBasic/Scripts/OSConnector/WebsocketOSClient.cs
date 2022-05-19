using System;
using System.Net.WebSockets;
using System.Text;
using UnityEngine;
using UnityWebSocket;
using WebSocketState = System.Net.WebSockets.WebSocketState;

namespace MotionCaptureBasic.OSConnector
{
    public class WebsocketOSClient
    {
        public delegate void ReceiveAction(string message);
        public event ReceiveAction OnReceived;

        private ClientWebSocket webSocket = null;
        
        private string url = "ws://127.0.0.1:8181/";

        private WebsocketOSClient()
        {
            InitConnect();
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
            if (webSocket != null)
                webSocket.Dispose();

            if (socket != null && socket.ReadyState != UnityWebSocket.WebSocketState.Closed)
            {
                socket.CloseAsync();
            }
            Console.WriteLine("WebSocket closed.");
        }

        
        //start
        private IWebSocket socket;
        private void InitConnect()
        {
            socket = new UnityWebSocket.WebSocket(url);
            socket.OnOpen += Socket_OnOpen;
            socket.OnMessage += Socket_OnMessage;
            socket.OnClose += Socket_OnClose;
            socket.OnError += Socket_OnError;
            socket.ConnectAsync();
        }
        
        private void Socket_OnOpen(object sender, OpenEventArgs e)
        {
            var message = new AppClientMessage();
            var encoded = Encoding.UTF8.GetBytes(JsonUtility.ToJson(message));
            var buffer = new ArraySegment<Byte>(encoded, 0, encoded.Length);
            socket.SendAsync(encoded);
           // socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
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

        private class AppClientMessage
        {
            public string type = "application_client";
        }
    }
}
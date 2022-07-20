using System;
using System.Text;
using UnityEngine;
using UnityWebSocket;

namespace SEngineBasic
{
    public class WebsocketOSClient
    {
        public IWebSocket Socket { get; private set; }
        private string url;

        public IMessage IMessage { get; private set;}
        private Action<WebSocketState> state;
        public void ConnectAsync(Action<WebSocketState> state, string url = "ws://127.0.0.1:8181/")
        {
            if (Socket != null) return;
            this.state = state;
            this.url = url;
            Socket = new WebSocket(url);
            Socket.OnOpen += OnOpen;
            Socket.OnMessage += OnMessage;
            Socket.OnClose += OnClose;
            Socket.OnError += OnError;
            Socket.ConnectAsync();
        }

        public void TryReConnectAsync()
        {
            if(Socket == null) return;
            if(Socket.ReadyState != WebSocketState.Closed) return;
            Socket.ConnectAsync();
        }

        public void CloseAsync()
        {
            if (Socket == null) return;
            if(Socket.ReadyState == WebSocketState.Closed) return;
            Socket.CloseAsync();
        }

        private void OnOpen(object sender, OpenEventArgs e)
        {
            /*
            var message = new AppClientMessage();
            var encoded = Encoding.UTF8.GetBytes(JsonUtility.ToJson(message));
            Debug.LogError(JsonUtility.ToJson(message));
            Socket.SendAsync(encoded);
            */
            state?.Invoke(WebSocketState.Open);
        }
        private void OnMessage(object sender, MessageEventArgs e)
        {
            if (e.IsText)
            {
                /*
                if (SEngineBasicDemo.Instance.Time > 0)
                {
                    SEngineBasicDemo.Instance.IsStart = true;
                    Debug.LogError($"message: {e.Data}");
                }
                else
                {
                    SEngineBasicDemo.Instance.IsStart = false;
                }
                */
                IMessage = Protocol.UnMarshal(e.Data);
            }
            else if (e.IsBinary)
            {
                
            }
        }
        
        private void OnClose(object sender, CloseEventArgs e)
        {
            state?.Invoke(WebSocketState.Closed);
            Debug.LogError($"Closed: StatusCode: {e.StatusCode}, Reason: {e.Reason}");
        }

        private void OnError(object sender, UnityWebSocket.ErrorEventArgs e)
        {
            state?.Invoke(WebSocketState.Closed);
            Debug.LogError($"Error: {e.Message}");
        }
    }
}
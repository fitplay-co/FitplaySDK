using System.Threading;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace UnityWebSocket.Server
{
    public abstract class WebSocketServerBase : MonoBehaviour
    {
        [SerializeField] protected string path = "/";
        [SerializeField] protected int port = 8080;
        [SerializeField] protected bool isDebug;
        
        private WebSocketServer server;
        private SynchronizationContext context;

        private const string DEBUGLOG_PREFIX = "[<color=#FF9654>WSServer</color>]";

        protected void StartServer(string path = "/", int port = 8080)
        {
            this.path = path;
            this.port = port;
            
            context = SynchronizationContext.Current;
 
            server = new WebSocketServer(port);
            server.AddWebSocketService<WebSocketServerBehavior>(path, serverBehavior =>
            {
                serverBehavior.SetContext(context, OnOpen, OnReceived, OnReceivedBytes, OnClose);
            });
 
            server.Start();

            if (isDebug)
            {
                Debug.Log($"{DEBUGLOG_PREFIX} Start path={path}, port={port}");
            }
        }
        
        protected void StopServer()
        {
            server.Stop();
            server.RemoveWebSocketService(path);
            server = null;

            if (isDebug)
            {
                Debug.Log($"{DEBUGLOG_PREFIX} Stop path={path}, port={port}");
            }
        }

        protected virtual void OnOpen(string userIp)
        {
            if (isDebug)
            {
                Debug.Log($"{DEBUGLOG_PREFIX} OnOpen");
            }
        }
 
        protected virtual void OnReceived(string data)
        {
            if (isDebug)
            {
                Debug.Log($"{DEBUGLOG_PREFIX} Received message: {data}");
            }
        }

        protected virtual void OnReceivedBytes(byte[] data)
        {
            if (isDebug)
            {
                Debug.Log($"{DEBUGLOG_PREFIX} Received message: {data.Length}");
            }
        }

        protected virtual void OnClose()
        {
            if (isDebug)
            {
                Debug.Log($"{DEBUGLOG_PREFIX} OnClose");
            }
        }
    }
}
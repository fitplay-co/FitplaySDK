using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace EasySocketConnection
{

    public abstract class SocketServerBase : MonoBehaviour
    {
        private struct ReceiveState
        {
            public Socket clientSocket;
            public byte[] buffer;
        }

        [SerializeField] protected string path = "/";
        [SerializeField] protected bool isDebug;
        
        private const int MaxCnt = 5;
        [SerializeField]  protected  int Port = 10086;
        private Socket m_ServerSocket;
        private Dictionary<string, Socket> m_ClientDict = new Dictionary<string, Socket>();
        private Dictionary<Socket, byte[]> m_ClientBuffer = new Dictionary<Socket, byte[]>();
        private const string DEBUGLOG_PREFIX = "[<color=#FF9654>WSServer</color>]";

        protected void StartServer(string path = "/", int port = 8080)
        {
            this.path = path;
            this.Port = port;
            Open(Port);

        }
        
        public void Open(int port)
        {
            m_ServerSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, port);
            m_ServerSocket.Bind(ipEndPoint);
            m_ServerSocket.Listen(MaxCnt);
            m_ServerSocket.BeginAccept(OnAccept, m_ServerSocket);
            if (isDebug)
            {
                Debug.Log($"{DEBUGLOG_PREFIX} Start path={path}, port={port}");
            }
        }

        public void Close()
        {
            m_ServerSocket.Close();
        }

        private void OnAccept(IAsyncResult result)
        {
            Socket serverSocket = (Socket)result.AsyncState;
            Socket clientSocket = serverSocket.EndAccept(result);
            string clientIp = clientSocket.RemoteEndPoint.ToString();
            m_ClientDict.Add(clientIp, clientSocket);
            //Console.WriteLine("{clientIp}连接上服务器，当前连接数:{m_ClientDict.Count}");
            ReceiveData(clientSocket);
            if (isDebug)
            {
                Debug.Log($"{DEBUGLOG_PREFIX} OnOpen at IP{clientIp}");
            }
            // 继续等待其他客户端连接
            serverSocket.BeginAccept(OnAccept, serverSocket);
            
        }

        private void ReceiveData(Socket client)
        {
            if (!m_ClientBuffer.TryGetValue(client, out byte[] buffer))
            {
                buffer = new byte[1024];
                m_ClientBuffer.Add(client, buffer);
            }

            client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, EndReceive, new ReceiveState
            {
                clientSocket = client,
                buffer = buffer
            });
        }

        private void EndReceive(IAsyncResult result)
        {
            ReceiveState state = (ReceiveState)result.AsyncState;
            Socket client = state.clientSocket;
            byte[] buffer = state.buffer;
            try
            {
                int length = client.EndReceive(result);
                string msgString = Encoding.Default.GetString(buffer, 0, length);
                Console.WriteLine($"客户端{((IPEndPoint)client.RemoteEndPoint).Address.MapToIPv4()}发来信息 {msgString}");
                ReceiveData(client);

                Send("服务器回复的心跳包", client);
            }
            catch (SocketException e)
            {
                m_ClientBuffer.Remove(client);
                m_ClientDict.Remove(client.RemoteEndPoint.ToString());
                Console.WriteLine($"{client.RemoteEndPoint} 下线,剩余连接数{m_ClientDict.Count}");
            }
            catch (FormatException e)
            {
                //Console.WriteLine("{client.RemoteEndPoint} 发来的数据无法解析......");
            }
        }

        public void Send(string msg, Socket client)
        {
            byte[] msgBytes = Encoding.Default.GetBytes(msg);
            client.BeginSend(msgBytes, 0, msgBytes.Length, SocketFlags.None, OnEndSend, client);
        }

        private void OnEndSend(IAsyncResult result)
        {
            Socket client = (Socket)result.AsyncState;
            int cnt = client.EndSend(result);
        }
    }
}
//
// namespace UnityWebSocket.Server
// {
//     public abstract class SocketServerBase : MonoBehaviour
//     {
//         [SerializeField] protected string path = "/";
//         [SerializeField] protected int port = 8080;
//         [SerializeField] protected bool isDebug;
//
//         private Socket server;
//         private WebSocketServer
//         private IPEndPoint ipEnd;
//         private SynchronizationContext context;
//
//         private const string DEBUGLOG_PREFIX = "[<color=#FF9654>WSServer</color>]";
//
//         protected void StartServer(string path = "/", int port = 8080)
//         {
//             this.path = path;
//             this.port = port;
//             
//             context = SynchronizationContext.Current;
//
//             ipEnd = new IPEndPoint(IPAddress.Any, port);
//             server = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
//             server.
//             server.AddWebSocketService<WebSocketServerBehavior>(path, serverBehavior =>
//             {
//                 serverBehavior.SetContext(context, OnOpen, OnReceived, OnReceivedBytes, OnClose);
//             });
//  
//             server.Start();
//
//             if (isDebug)
//             {
//                 Debug.Log($"{DEBUGLOG_PREFIX} Start path={path}, port={port}");
//             }
//         }
//         
//         protected void StopServer()
//         {
//             server.Stop();
//             server.RemoveWebSocketService(path);
//             server = null;
//
//             if (isDebug)
//             {
//                 Debug.Log($"{DEBUGLOG_PREFIX} Stop path={path}, port={port}");
//             }
//         }
//
//         protected virtual void OnOpen()
//         {
//             if (isDebug)
//             {
//                 Debug.Log($"{DEBUGLOG_PREFIX} OnOpen");
//             }
//         }
//  
//         protected virtual void OnReceived(string data)
//         {
//             if (isDebug)
//             {
//                 Debug.Log($"{DEBUGLOG_PREFIX} Received message: {data}");
//             }
//         }
//
//         protected virtual void OnReceivedBytes(byte[] data)
//         {
//             if (isDebug)
//             {
//                 Debug.Log($"{DEBUGLOG_PREFIX} Received message: {data.Length}");
//             }
//         }
//
//         protected virtual void OnClose()
//         {
//             if (isDebug)
//             {
//                 Debug.Log($"{DEBUGLOG_PREFIX} OnClose");
//             }
//         }
//     }
// }
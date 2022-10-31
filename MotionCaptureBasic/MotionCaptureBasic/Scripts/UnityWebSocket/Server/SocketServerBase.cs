using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace UnityWebSocket.SocketServer
{
    public abstract class SocketServerBase : MonoBehaviour
    {
        [SerializeField]
        protected string Path = ROOT_PATH;

        [SerializeField]
        protected int ConnectionLength = 5;

        [SerializeField]
        protected int Port = DEFAULT_PORT;

        const string DEBUGLOG_PREFIX = "[<color=#FF9654>WebSocketServer</color>]";
        const string ROOT_PATH = "/";
        const int DATA_LENGTH = 1024;
        const int DEFAULT_PORT = 8080;

        Socket m_ServerSocket;
        byte[] m_Data = new byte[DATA_LENGTH];

        protected virtual void StartServer(string InPath = ROOT_PATH, int port = DEFAULT_PORT)
        {
            Path = InPath;
            Port = port;
            Open(Port);
        }

        protected virtual void OnAccept(Socket client, string ip) { }

        protected virtual void OnReceive(Socket client, byte[] data, int length)
        {
            client.BeginReceive(data, 0, data.Length, SocketFlags.None, asyncResult =>
            {
                int newLength = client.EndReceive(asyncResult);
                OnReceive(client, data, newLength);
            }, null);
        }

        protected virtual void OnSend(Socket client, int length) { }

        public void Open(int port)
        {
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, port);
            m_ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            m_ServerSocket.Bind(ipEndPoint);
            m_ServerSocket.Listen(ConnectionLength);
            Debug.Log($"{DEBUGLOG_PREFIX} Start path={Path}, port={port}");
            m_ServerSocket.BeginAccept(ar =>
            {
                Socket client = m_ServerSocket.EndAccept(ar);
                Debug.Log($"{DEBUGLOG_PREFIX} Client Request Connection {GetAddress(client)}");
                OnAccept(client, GetAddress(client));
                SendData(client, "Hello");
                client.BeginReceive(m_Data, 0, m_Data.Length, SocketFlags.None, (asyncResult) =>
                {
                    int length = client.EndReceive(asyncResult);
                    OnReceive(client, m_Data, length);
                }, null);
            }, null);
        }

        public void SendData(Socket client, string data)
        {
            if (client == null || string.IsNullOrEmpty(data))
            {
                return;
            }

            byte[] bytes = Encoding.UTF8.GetBytes(data);
            try
            {
                client.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, (ar) =>
                {
                    int sendLength = client.EndSend(ar);
                    OnSend(client, sendLength);
                    Debug.Log($"{DEBUGLOG_PREFIX} Client Send data length {sendLength.ToString()}");
                }, null);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void Close()
        {
            m_ServerSocket.Close();
        }

        static string GetAddress(Socket client)
        {
            return client == null ? string.Empty : ((IPEndPoint)client.RemoteEndPoint).Address.MapToIPv4().ToString();
        }
    }
}
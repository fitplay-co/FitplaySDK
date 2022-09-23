using System;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MotionCaptureBasic.OSConnector
{
    public class HttpProtocolHandler
    {
        private static HttpProtocolHandler instance;
        private static readonly object _Synchronized = new object();

        private bool isDebug;
        private float lastTime;
        private Action onConnect;
        private Action onClosed;
        private Action onError;
        private IKBodyUpdateMessage _bodyMessageBase;

        public IKBodyUpdateMessage BodyMessageBase => _bodyMessageBase;

        private HttpProtocolHandler()
        {
           
        }
        
        public enum ConnectStatus
        {
            OnConnected,
            OnError,
            OnDisConnect
            
        }

        ~HttpProtocolHandler()
        {
        }

        public void StartWebSocket(string url, bool useJson)
        {
            var app = WebsocketOSClient.GetInstance();
            app.SetUseJson(useJson);
            app.InitConnect(url);
            app.OnReceived += OnReceived;
            app.OnConnect += OnConnect;
            app.OnClosed += OnClosed;
            app.OnError += OnError;
        }

        public static HttpProtocolHandler GetInstance()
        {
            if (instance == null)
            {
                lock(_Synchronized)
                {
                    if(instance == null)
                    {
                        instance = new HttpProtocolHandler();
                    }
                }
            }

            return instance;
        }
        
        public void AddConnectEvent(Action onConnect, Action onClosed = null, Action onError = null)
        {
            this.onConnect += onConnect;
            this.onClosed += onClosed;
            this.onError += onError;
        }

        public void SetDebug(bool isDebug)
        {
            this.isDebug = isDebug;
        }

        private void OnReceived(string message)
        {
            var diff = Time.time - lastTime;
            lastTime = Time.time;
            message = RemoveBetween(message, "\"flatbuffersData\":{", "},");
            if (string.IsNullOrEmpty(message)) return;
            if (isDebug)
            {
                using (FileStream fileStream = new FileStream("Logs/OsMsgLog.log", FileMode.Append))
                {
                    using (StreamWriter streamWriter = new StreamWriter(fileStream))
                    {
                        streamWriter.WriteLine(message);
                    }
                }
            }
            //只解析数据类型
            //目前有三种类型数据，camera, imu, input
            MessageType dataType = Protocol.UnMarshalType(message);
            if (dataType == null)
            {
                Debug.LogError("Data Error, don't include sensor_type!");
                return;
            }
            //imu和input数据处理通道
            if (dataType.sensor_type != null)
            {
                string sType = dataType.sensor_type.ToLower();
                if (sType == SensorType.IMU || sType == SensorType.INPUT)
                {
                    BasicEventHandler.DispatchImuDataEvent(message);
                    return;
                }
            }
            
            //动捕数据处理通道
            _bodyMessageBase = Protocol.UnMarshal(message) as IKBodyUpdateMessage;
            var d  = _bodyMessageBase.timeProfiling.beforeSendTime - _bodyMessageBase.timeProfiling.startTime;
            var nowTime = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
           
            //     Console.WriteLine("本级时间戳-startTime:" + (nowTime -  _bodyMessageBase.timeProfiling.startTime) + "，" + nowTime + " ，" + _bodyMessageBase.timeProfiling.startTime);
            //     Console.WriteLine($"上一帧和当前帧相差时间：{diff * 1000} 毫秒,服务器处理的时间：{d } 毫秒");
        }
        
        private string RemoveBetween(string s, string begin, string end) 
        {
            Regex r = new Regex(begin);
            Match m = r.Match(s); 
            if (m.Success)
            {
                int start = m.Index;
                r = new Regex(end);
                m = r.Match(s, start + begin.Length);
                if (m.Success)
                {
                    int e = m.Index;
                    return s.Remove(start, e - start + end.Length);
                }
            }
            return s;
        }

        private void OnConnect()
        {
            onConnect?.Invoke();
        }
        
        private void OnClosed()
        {
            onClosed?.Invoke();
        }
        
        private void OnError()
        {
            onError?.Invoke();
        }
    }
}
using System;
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
        private IKBodyUpdateMessage _bodyMessageBase;

        public IKBodyUpdateMessage BodyMessageBase => _bodyMessageBase;

        private HttpProtocolHandler()
        {
            var app = WebsocketOSClient.GetInstance();
            app.OnReceived += OnReceived;
            app.OnConnect += OnConnect;
        }

        ~HttpProtocolHandler()
        {
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
        
        public void AddConnectEvent(Action onConnect)
        {
            this.onConnect += onConnect;
        }

        public void SetDebug(bool isDebug)
        {
            this.isDebug = isDebug;
        }

        private void OnReceived(string message)
        {
            var diff = Time.time - lastTime;
            lastTime = Time.time;
            
            var osData = Protocol.UnMarshal(message) as IKBodyUpdateMessage;

            if (osData == null)
            {
                return;
            }

            if (osData.sensor_type != null && (osData.sensor_type == "imu" || osData.sensor_type == "input" ))
            {
                //MotionDataModelHttp.GetInstance().OnImuReceived(osData);
                return;
            }

            _bodyMessageBase = osData;
            var d  = _bodyMessageBase.timeProfiling.beforeSendTime - _bodyMessageBase.timeProfiling.startTime;

            var nowTime = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
           
            //     Console.WriteLine("本级时间戳-startTime:" + (nowTime -  _bodyMessageBase.timeProfiling.startTime) + "，" + nowTime + " ，" + _bodyMessageBase.timeProfiling.startTime);
            //     Console.WriteLine($"上一帧和当前帧相差时间：{diff * 1000} 毫秒,服务器处理的时间：{d } 毫秒");

            if (isDebug)
            {
                Debug.Log(message);
            }
        }

        private void OnConnect()
        {
            onConnect?.Invoke();
        }
    }
}
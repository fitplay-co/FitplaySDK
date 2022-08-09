using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MotionCaptureBasic.OSConnector
{
    public class MobileOSHandler
    {
        private static MobileOSHandler instance;
        private static readonly object _Synchronized = new object();
        private GameObject osDataReceiverObj;

        private bool isDebug;
        private float lastTime;
        private IKBodyUpdateMessage _bodyMessageBase;

        public IKBodyUpdateMessage BodyMessageBase => _bodyMessageBase;

        private MobileOSHandler()
        {

            OsDataReceiver osDataReceiver = Object.FindObjectOfType<OsDataReceiver>();
            if (osDataReceiver == null)
            {
                osDataReceiverObj = new GameObject("osDataReceiverObj");
                osDataReceiver = osDataReceiverObj.AddComponent<OsDataReceiver>();
            }

            osDataReceiver.onReceiveNormalData += OnReceived;
        }

        ~MobileOSHandler()
        {
            if (osDataReceiverObj != null)
            {
                Object.Destroy(osDataReceiverObj);
                osDataReceiverObj = null;
            }
        }

        public static MobileOSHandler GetInstance()
        {
            if (instance == null)
            {
                lock(_Synchronized)
                {
                    if(instance == null)
                    {
                        instance = new MobileOSHandler();
                    }
                }
            }

            return instance;
        }

        public void SetDebug(bool isDebug)
        {
            this.isDebug = isDebug;
        }

        private void OnReceived(string message)
        {
            var diff = Time.time - lastTime;
            lastTime = Time.time;
            if (string.IsNullOrEmpty(message)) return;
            if (isDebug) Debug.Log(message);
            //只解析数据类型
            //目前有三种类型数据，camera, imu, input
            MessageType dataType = Protocol.UnMarshalType(message);
            if (dataType == null)
            {
                Debug.LogError("Data Error, don't include sensor_type!");
                return;
            }
            //imu和input数据处理通道
            string sType = dataType.sensor_type.ToLower();
            if (sType == SensorType.IMU || sType == SensorType.INPUT)
            {
                BasicEventHandler.DispatchImuDataEvent(message);
                return;
            }
            //动捕数据处理通道
            _bodyMessageBase = Protocol.UnMarshal(message) as IKBodyUpdateMessage;
        }
    }
}
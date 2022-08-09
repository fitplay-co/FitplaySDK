using System;
using UnityEngine;

namespace MotionCaptureBasic
{
    public class OsDataReceiver:MonoBehaviour
    {
        public Action<string> onReceiveNormalData;
        public Action<string> onReceiveActionDetectionData;
        public Action<string> onReceiveGroundLocationData;
        public Action<string> onReceiveFittingData;
        
        //目前有三种类型数据，camera, imu, input
        public void ReceivedOsNormalData(string msg)
        {
            Debug.Log($"收到了Normal数据：{msg}");
            onReceiveNormalData?.Invoke(msg);
        }

        //接收到的ActionDetection数据
        public void ReceivedActionDetectionData(string msg)
        {
            Debug.Log($"收到了ActionDetection数据：{msg}");
            onReceiveActionDetectionData?.Invoke(msg);
        }
        //接收到的GroundLocation数据
        public void ReceivedGroundLocationData(string msg)
        {
            Debug.Log($"收到了GroundLocation数据：{msg}");
            onReceiveGroundLocationData?.Invoke(msg);
        }

        //接收到的Fitting数据
        public void ReceivedFittingData(string msg)
        {
            Debug.Log($"收到了Fitting数据：{msg}");
            onReceiveFittingData?.Invoke(msg);
        }
    }
}
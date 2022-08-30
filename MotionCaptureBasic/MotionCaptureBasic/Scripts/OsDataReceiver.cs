using System;
using UnityEngine;

namespace MotionCaptureBasic
{
    public class OsDataReceiver:MonoBehaviour
    {
        public Action<string> onReceiveNormalData;

        public void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        //目前有三种类型数据，camera, imu, input
        public void ReceivedOsNormalData(string msg)
        {
            onReceiveNormalData?.Invoke(msg);
        }

        //接收到的ActionDetection数据
        public void ReceivedDebugLog(string msg)
        {
            Debug.Log(msg);
            Console.Write(msg);
        }
    }
}
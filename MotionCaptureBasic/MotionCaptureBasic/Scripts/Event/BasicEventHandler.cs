using MotionCaptureBasic.OSConnector;
using System;
using UnityEngine.Events;
namespace MotionCaptureBasic.OSConnector
{
    public static class BasicEventHandler
    {
        [Serializable] public class UEvent : UnityEvent { }
        /// <summary>
        ///订阅IMU数据
        /// </summary>
        public delegate void EventImuDataRecievied(string imuMessage);
        public static EventImuDataRecievied OnImuDataRecieved;

        public static void DispatchImuDataEvent(string imuMessage) => OnImuDataRecieved?.Invoke(imuMessage);
    }
}

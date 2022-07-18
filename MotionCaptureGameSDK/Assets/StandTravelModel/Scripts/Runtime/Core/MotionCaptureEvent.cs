using System;
using UnityEngine.Events;

namespace StandTravelModel.Scripts.Runtime.Core
{
    public static class MotionCaptureEvent
    {
        [Serializable]
        public class UEvent : UnityEvent
        {
        }

        /// <summary>
        ///IK数据设置后
        /// </summary>
        public delegate void EventAfterSetIK();

        public static EventAfterSetIK OnSetIK;


        /// <summary>
        /// IK数据更新后的事件
        /// </summary>
        public static void DispatchAfterSetIKEvent() => OnSetIK?.Invoke();
    }
}
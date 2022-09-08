﻿using System;
using System.Numerics;
using UnityEngine.Events;

namespace MotionLib.Scripts
{
    public class MotionLibEventHandler
    {
        [Serializable]
        public class UEvent : UnityEvent
        {
        }

        /// <summary>
        ///玩家被创建
        /// </summary>
        public static Action onLocalPlayerSpawn;

        public static void DispatchPlayerLocalSpawnEvent() => onLocalPlayerSpawn?.Invoke();

        /// <summary>
        /// 玩家被移除
        /// </summary>
        public static Action onLocalPlayerDeath;

        public static void DispatchPlayerLocalDeathEvent() => onLocalPlayerDeath?.Invoke();
        
        /// <summary>
        ///识别动作切换
        /// </summary>
        public static Action onMotionChanged;

        public static void DispatchMotionChangedEvent() => onMotionChanged?.Invoke();
        /// <summary>
        ///识别到切换StandToTravel
        /// </summary>
        public static Action onSwithStandToTravel;

        public static void DispatchSwitchMotionModeEvent() => onSwithStandToTravel?.Invoke();
        
        //public static Action<MotionLibController.MotionMode> onMotionDetected;

        //public static void DispatchMotionDetectionEvent() => onMotionDetected?.Invoke(MotionLibController.MotionMode.None);
        
        public delegate void EventMotionDetected(MotionLibController.MotionMode mode);
        public static EventMotionDetected OnMotionDetected;
        public static void DispatchMotionDetectionEvent(MotionLibController.MotionMode mode) => OnMotionDetected?.Invoke(mode);
    }
}

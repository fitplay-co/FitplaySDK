using System;
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
    }
}
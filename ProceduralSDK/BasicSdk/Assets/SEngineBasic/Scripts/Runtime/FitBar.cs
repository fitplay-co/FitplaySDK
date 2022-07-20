using System.Collections.Generic;
using UnityEngine;

namespace SEngineBasic
{
    public static class FitBar
    {
        private static Dictionary<EHandleType, InputMessage> inputMessages = new Dictionary<EHandleType, InputMessage>();
        private static Dictionary<EHandleType, ImuMessage> imuMessages = new Dictionary<EHandleType, ImuMessage>();

        internal static void SetMessages(IMessage message)
        {
            if (message is ImuMessage imuMessage)
            {
                var type = imuMessage.HandleType;
                if (inputMessages.TryGetValue(type, out var value))
                {
                    imuMessages[type] = imuMessage;
                }
                else
                {
                    imuMessages.Add(type, imuMessage);
                }
            }
            else if (message is InputMessage inputMessage)
            {
                var type = inputMessage.HandleType;
                if (inputMessages.TryGetValue(type, out var value))
                {
                    inputMessages[type] = inputMessage;
                }
                else
                {
                    inputMessages.Add(type, inputMessage);
                }
            }
        }

        public static void Clear()
        {
            inputMessages.Clear();
            imuMessages.Clear();
            lCache.Clear();
            rCache.Clear();
        }

        #region imu
        /// <summary>
        /// 返回Quaternion（比如rotation）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Quaternion GetImuQuaternion(EImuKey key, EHandleType type = EHandleType.LeftHandle)
        {
            return imuMessages.TryGetValue(type, out var value) ? ImuQuaternion(value, key) : Quaternion.identity;
        }

        private static Quaternion ImuQuaternion(ImuMessage imu, EImuKey key)
        {
            switch (key)
            {
                case EImuKey.Rotation:
                    return imu.imu.quaternions == null ? Quaternion.identity : new Quaternion(imu.imu.quaternions.x, imu.imu.quaternions.y, imu.imu.quaternions.z, imu.imu.quaternions.w);
            }
            return Quaternion.identity;
        }

        /// <summary>
        /// 返回Vector3 （比如加速度）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Vector3 GetImuVector3(EImuKey key, EHandleType type = EHandleType.LeftHandle)
        {
            return imuMessages.TryGetValue(type, out var value) ? ImuVector3(value, key) : Vector3.zero;
        }
        private static Vector3 ImuVector3(ImuMessage imu, EImuKey key)
        {
            switch (key)
            {
                case EImuKey.Accelerometer:
                    return imu.imu.accelerometer == null ? Vector3.zero : new Vector3(imu.imu.accelerometer.x, imu.imu.accelerometer.y, imu.imu.accelerometer.z);
                case EImuKey.Gyroscope:
                    return imu.imu.gyroscope == null ? Vector3.zero : new Vector3(imu.imu.gyroscope.x, imu.imu.gyroscope.y, imu.imu.gyroscope.z);
                case EImuKey.Magnetometer:
                    return imu.imu.magnetometer == null ? Vector3.zero : new Vector3(imu.imu.magnetometer.x, imu.imu.magnetometer.y, imu.imu.magnetometer.z);
            }
            return Vector3.zero;
        }
        #endregion
        
        #region input
        /// <summary>
        /// 返回vector2（如遥感）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Vector2 GetInputVector2(EInputKey key, EHandleType type = EHandleType.LeftHandle)
        {
            return inputMessages.TryGetValue(type, out var value) ? InputVector2(value, key) : Vector2.zero;
        }
        private static Vector2 InputVector2(InputMessage input, EInputKey key)
        {
            switch (key)
            {
                case EInputKey.Joystick:
                    //中间值32768
                    //32000 ～ 33536 为idle状态
                    if(input.input.joystick == null) return Vector2.zero;
                    var vx = input.input.joystick.x >= 32000 && input.input.joystick.x <= 33536 ? 0 : input.input.joystick.x / 32768.0f - 1;
                    var vy = input.input.joystick.y >= 32000 && input.input.joystick.y <= 33536 ? 0 : input.input.joystick.y / 32768.0f - 1;
                    return new Vector2(vx, vy);
            }
            return Vector2.zero;
        }
        /// <summary>
        /// input 相关值（心率，血氧等）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static float GetInputValue(EInputValue key, EHandleType type = EHandleType.LeftHandle)
        {
            return inputMessages.TryGetValue(type, out var value) ? InputValue(value, key) : 0;
        }

        private static float InputValue(InputMessage input, EInputValue key)
        {
            switch (key)
            {
                case EInputValue.HeartRate:
                    return input.input.heart_rate;
                case EInputValue.BloodOxygen:
                    return input.input.blood_oxygen;
            }
            return 0;
        }

        /// <summary>
        /// 返回按钮当前值
        /// </summary>
        /// <param name="key">按键或遥感</param>
        /// <param name="type">手柄类型</param>
        /// <returns></returns>
        public static float GetInputButtonValue(EInputKey key, EHandleType type = EHandleType.LeftHandle)
        {
            return inputMessages.TryGetValue(type, out var value) ? InputButtonValue(value, key) : 0;
        }
        private static float InputButtonValue(InputMessage input, EInputKey key)
        {
            switch (key)
            {
                case EInputKey.KeyA:
                    return input.input.keys.key_A;
                case EInputKey.KeyB:
                    return input.input.keys.key_B;
                case EInputKey.KeyMenu:
                    return input.input.keys.key_menu;
                case EInputKey.L1:
                    return input.input.linear_key.L1 / 65535.0f;//0~1
                case EInputKey.L2:
                    return input.input.linear_key.L2 / 65535.0f;//0~1
                case EInputKey.JoystickKey:
                    return input.input.joystick.key;
                
            }
            return 0;
        }
        
        /// <summary>
        /// 返回按钮当前状态 false：弹起，true：按下
        /// </summary>
        /// <param name="key">按键</param>
        /// <param name="type">手柄类型</param>
        /// <param name="continuous">持续的</param>
        /// <returns></returns>
        public static bool GetInputButtonState(EInputKey key, EHandleType type = EHandleType.LeftHandle, bool continuous = false)
        {
            return inputMessages.TryGetValue(type, out var value) && InputButton(value, key, type, continuous);
        }
        
        private static bool InputButton(InputMessage input, EInputKey key, EHandleType type, bool isContinuous)
        {
            switch (key)
            {
                case EInputKey.KeyA:
                    if (input.input.keys == null) return false;
                    if (isContinuous)
                    {
                        return input.input.keys.key_A == 0;
                    }
                    else
                    {
                        return InputButtonCheck(key, type, input.input.keys.key_A == 0);
                    }

                case EInputKey.KeyB:
                    if (input.input.keys == null) return false;
                    if (isContinuous)
                    {
                        return input.input.keys.key_B == 0;
                    }
                    else
                    {
                        return InputButtonCheck(key, type, input.input.keys.key_B == 0);
                    }
                case EInputKey.KeyMenu:
                    if (input.input.keys == null) return false;
                    if (isContinuous)
                    {
                        return input.input.keys.key_menu == 0;
                    }
                    else
                    {
                        return InputButtonCheck(key, type, input.input.keys.key_menu == 0);
                    }
                case EInputKey.L1:
                    if (input.input.linear_key == null) return false;
                    if (isContinuous)
                    {
                        return input.input.linear_key.L1 <= 65000; //65535
                    }
                    else
                    {
                        return InputButtonCheck(key, type, input.input.linear_key.L1 <= 65000);
                    }
                case EInputKey.L2:
                    if (input.input.linear_key == null) return false;
                    if (isContinuous)
                    {
                        return input.input.linear_key.L2 <= 65000; //65535
                    }
                    else
                    {
                        return InputButtonCheck(key, type, input.input.linear_key.L2 <= 65000);
                    }

                case EInputKey.JoystickKey:
                    if (input.input.joystick == null) return false;
                    if (isContinuous)
                    {
                        return input.input.joystick.key == 0;
                    }
                    else
                    {
                        return InputButtonCheck(key, type, input.input.joystick.key == 0);
                    }
                default:
                    return false;
            }
        }
        
        private static Dictionary<EInputKey, bool> lCache = new Dictionary<EInputKey, bool>();
        private static Dictionary<EInputKey, bool> rCache = new Dictionary<EInputKey, bool>();
        private static bool InputButtonCheck(EInputKey key, EHandleType type, bool value)
        {
            return type == EHandleType.LeftHandle ? CheckHandler(ref lCache, key, value) : CheckHandler(ref rCache, key, value);
        }

        private static bool CheckHandler(ref Dictionary<EInputKey, bool> cache, EInputKey key, bool value)
        {
            if (!cache.ContainsKey(key))
            {
                cache.Add(key, true);
            }

            var b = cache[key];
            if (value)
            {
                if (!b) return false;
                cache[key] = false;
                return true;
            }
            cache[key] = true;
            return false;
        }
        #endregion
    }
}
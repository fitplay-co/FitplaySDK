using System;
using System.Collections.Generic;
using UnityEngine;

namespace IMU
{
    /// <summary>
    /// BY Dong yihui - 2022-06-10
    /// 1. 继承自系统的Input
    /// 2. 对GetKeyDown和GetKeyUp等函数增加了Imu手柄按键的响应
    /// 3. 对GetKey函数增加了Imu手柄LineKey的响应支持
    /// 4. 对GetAxis函数增加Imu手柄摇杆的响应支持
    /// TODO:增加都心率和血氧的数据支持
    /// </summary>
    public class Input : UnityEngine.Input
    {
        /// <summary>
        /// 记录按键Down - 下一帧清除
        /// </summary>
        private static HashSet<string> _imuKeyUpCode;
        /// <summary>
        /// 记录按键UP - 下一帧清除
        /// </summary>
        private static HashSet<string> _imuKeyDownCode;
        /// <summary>
        /// 记录按键Down - UP时清除
        /// </summary>
        private static HashSet<string> _imuKeyCode;
        private static Quaternion _quaternion_L;
        private static Quaternion _quaternion_R;
        private static Vector3 _accelerometer_L = Vector3.zero;
        private static Vector3 _accelerometer_R = Vector3.zero;
        private static Gyroscope _gyroscope_L = new Gyroscope();
        private static Gyroscope _gyroscope_R = new Gyroscope();
        private static Magnetometer _magnetometer_L = new Magnetometer();
        private static Magnetometer _magnetometer_R = new Magnetometer();
        
        /// <summary>
        /// 使用float定义取值的手柄按键
        /// </summary>
        public static Dictionary<KeyCode, float> KeyValueMap = new Dictionary<KeyCode, float>()
        {
            {KeyCode.L_JoyStack_H, 0},
            {KeyCode.L_JoyStack_V, 0},
            {KeyCode.R_JoyStack_H, 0},
            {KeyCode.R_JoyStack_V, 0},
            {KeyCode.L_L1Key, 0},
            {KeyCode.L_L2Key, 0},
            {KeyCode.R_L1Key, 0},
            {KeyCode.R_L2Key, 0}
        };

        /// <summary>
        /// 使用按下和弹起状态的手柄按键
        /// </summary>
        public static Dictionary<KeyCode, KeyStatusData> KeyStatusMap = new Dictionary<KeyCode, KeyStatusData>()
        {
            {KeyCode.L_Key_A, new KeyStatusData() {Current = KeyStatus.Up, Record = KeyStatus.Up}},
            {KeyCode.L_Key_B, new KeyStatusData() {Current = KeyStatus.Up, Record = KeyStatus.Up}},
            {KeyCode.L_Key_Menu, new KeyStatusData() {Current = KeyStatus.Up, Record = KeyStatus.Up}},
            {KeyCode.L_JoyStick, new KeyStatusData() {Current = KeyStatus.Up, Record = KeyStatus.Up}},
            {KeyCode.R_Key_A, new KeyStatusData() {Current = KeyStatus.Up, Record = KeyStatus.Up}},
            {KeyCode.R_Key_B, new KeyStatusData() {Current = KeyStatus.Up, Record = KeyStatus.Up}},
            {KeyCode.R_Key_Menu, new KeyStatusData() {Current = KeyStatus.Up, Record = KeyStatus.Up}},
            {KeyCode.R_JoyStick, new KeyStatusData() {Current = KeyStatus.Up, Record = KeyStatus.Up}}
        };

        /// <summary>
        /// 手柄状态数据
        /// </summary>
        public class KeyStatusData
        {
            public KeyStatus Current;
            public KeyStatus Record;
        }
        
        /// <summary>
        /// KeyCode名转KeyCode
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static KeyCode ToEnum(string str)
        {
            try
            {
                return (KeyCode) Enum.Parse(typeof(KeyCode), str);
            }
            catch (Exception e)
            {
                return KeyCode.None;
            }
        }
        
        /// <summary>
        /// 是否有键按下
        /// </summary>
        public static bool anyKeyDown
        {
            get
            {
                if (UnityEngine.Input.anyKeyDown)
                    return true;
                return _imuKeyDownCode?.Count > 0;
            }
        }

        /// <summary>
        /// 是否有key松开
        /// </summary>
        public static bool imuKeyUp
        {
            get => (_imuKeyUpCode?.Count > 0);
        }

        /// <summary>
        /// 是否有key按下
        /// </summary>
        public static bool imuKeyDown
        {
            get => (_imuKeyDownCode?.Count > 0);
        }

        /// <summary>
        /// 获取四元数
        /// --左手柄
        /// </summary>
        public static Quaternion ImuQuaternion_L
        {
            get => _quaternion_L;
            set => _quaternion_L = value;
        }

        /// <summary>
        /// 获取四元数
        /// --右手柄
        /// </summary>
        public static Quaternion ImuQuaternion_R
        {
            get => _quaternion_R;
            set => _quaternion_R = value;
        }

        /// <summary>
        /// 获取加速度
        /// --左手柄
        /// </summary>
        public static Vector3 ImuAccelerometer_L
        {
            get => _accelerometer_L;
            set => _accelerometer_L = value;
        }

        /// <summary>
        /// 获取加速度
        /// --右手柄
        /// </summary>
        public static Vector3 ImuAccelerometer_R
        {
            get => _accelerometer_R;
            set => _accelerometer_R = value;
        }

        public static Gyroscope ImuGyroscope_L
        {
            get => _gyroscope_L;
            set => _gyroscope_L = value;
        }

        public static Gyroscope ImuGyroscope_R
        {
            get => _gyroscope_R;
            set => _gyroscope_R = value;
        }

        public static Magnetometer ImuMagnetometer_L
        {
            get => _magnetometer_L;
            set => _magnetometer_L = value;
        }

        public static Magnetometer ImuMagnetometer_R
        {
            get => _magnetometer_R;
            set => _magnetometer_R = value;
        }

        /// <summary>
        /// 添加一个key，当这个key按下时
        /// </summary>
        /// <param name="isKeyDown"></param>
        /// <param name="keyCode"></param>
        public static void AddKey(string keyCode, bool isKeyDown)
        {
            if (isKeyDown)
            {
                _imuKeyDownCode ??= new HashSet<string>();
                _imuKeyDownCode.Add(keyCode);
                //按下时添加
                _imuKeyCode ??= new HashSet<string>();
                _imuKeyCode.Add(keyCode);
            }
            else
            {
                _imuKeyUpCode ??= new HashSet<string>();
                _imuKeyUpCode.Add(keyCode);
                //弹起时清理
                if(_imuKeyCode != null)
                    _imuKeyCode.Remove(keyCode);
            }
        }

        /// <summary>
        /// 删除一个key，当这个key释放后
        /// </summary>
        /// <param name="isKeyDown"></param>
        /// <param name="keyCode"></param>
        public static void RemoveKey(string keyCode, bool isKeyDown)
        {
            if (isKeyDown)
            {
                if (_imuKeyDownCode == null) return;
                _imuKeyDownCode.Remove(keyCode);
            }
            else
            {
                if (_imuKeyUpCode == null) return;
                _imuKeyUpCode.Remove(keyCode);
            }
        }

        /// <summary>
        ///按键的Up检测
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool GetKeyUp(string name)
        {
            if (KeyStatusMap.ContainsKey(ToEnum(name)))
            {
                if (_imuKeyUpCode != null && imuKeyUp)
                {
                    return _imuKeyUpCode.Contains(name);
                }

                return false;
            }

            return UnityEngine.Input.GetKeyUp(name);
        }

        /// <summary>
        /// 按键的Up检测
        /// </summary>
        /// <param name="keyCode"></param>
        /// <returns></returns>
        public static bool GetKeyUp(KeyCode keyCode)
        {
            if (KeyStatusMap.ContainsKey(keyCode))
            {
                if (_imuKeyUpCode != null && imuKeyUp)
                {
                    return _imuKeyUpCode.Contains(keyCode.ToString());
                }

                return false;
            }

            return UnityEngine.Input.GetKeyUp((UnityEngine.KeyCode) keyCode);
        }

        /// <summary>
        /// 按键的Down检测
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public new static bool GetKeyDown(string name)
        {
            if (KeyStatusMap.ContainsKey(ToEnum(name)))
            {
                if (_imuKeyDownCode != null && imuKeyDown)
                {
                    return _imuKeyDownCode.Contains(name);
                }

                return false;
            }

            return UnityEngine.Input.GetKeyDown(name);
        }

        /// <summary>
        /// 按键的Down检测
        /// </summary>
        /// <param name="keyCode"></param>
        /// <returns></returns>
        public static bool GetKeyDown(KeyCode keyCode)
        {
            if (KeyStatusMap.ContainsKey(keyCode))
            {
                if (_imuKeyDownCode != null && imuKeyDown)
                {
                    return _imuKeyDownCode.Contains(keyCode.ToString());
                }

                return false;
            }

            return UnityEngine.Input.GetKeyDown((UnityEngine.KeyCode) keyCode);
        }

        /// <summary>
        /// 获取按键是否按下 - 可能一直未松开
        /// </summary>
        /// <param name="keyCode"></param>
        /// <returns></returns>
        public static bool GetKey(KeyCode keyCode)
        {
            if (KeyStatusMap.ContainsKey(keyCode))
            {
                if (_imuKeyCode != null)
                {
                    return _imuKeyCode.Contains(keyCode.ToString());
                }

                return false;
            }

            return UnityEngine.Input.GetKey((UnityEngine.KeyCode) keyCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public new static float GetAxis(string name)
        {
            var code = ToEnum(name);
            if (KeyValueMap.ContainsKey(code))
            {
                return KeyValueMap[code];
            }

            return UnityEngine.Input.GetAxis(name);
        }
    }
}
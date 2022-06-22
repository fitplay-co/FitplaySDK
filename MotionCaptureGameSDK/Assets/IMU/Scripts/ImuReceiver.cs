using UnityEngine;
using System;
using System.Collections.Generic;
using MotionCaptureBasic.OSConnector;
using UniRx;

namespace IMU
{
    public class ImuReceiver : MonoBehaviour
    {
        private string version;
        private uint device_id;
        private uint timestamp;
        private int _heartRate;
        private int _bloodOxygen;
        private Dictionary<string, IDisposable> _disposables = new Dictionary<string, IDisposable>();
        void Start()
        {
            //var app = ImuManager.Create();
            //app.OnReceived += this.OnReceived;
            BasicEventHandler.OnImuDataRecieved += this.OnReceived;
            HttpProtocolHandler.GetInstance().SetDebug(true);
        }

        void Update()
        {
            /*
            if (Input.imuKeyUp)
            {
                if (Input.GetKeyUp(KeyCode.L_Key_A.ToString()))
                {
                    Debug.Log("=================KEY UP A =============");
                }

                if (Input.GetKeyUp(KeyCode.L_Key_B.ToString()))
                {
                    Debug.Log("=================KEY UP B =============");
                }

                if (Input.GetKeyUp(KeyCode.L_Key_Menu.ToString()))
                {
                    Debug.Log("=================KEY UP Key_menu =============");
                }

                if (Input.GetKeyUp(KeyCode.L_JoyStick.ToString()))
                {
                    Debug.Log("=================KEY UP JoyStick =============");
                }
            }

            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(KeyCode.L_Key_A.ToString()))
                {
                    Debug.Log("=================KEY DOWN A =============");
                }

                if (Input.GetKeyDown(KeyCode.L_Key_B.ToString()))
                {
                    Debug.Log("=================KEY DOWN B =============");
                }

                if (Input.GetKeyDown(KeyCode.L_Key_Menu.ToString()))
                {
                    Debug.Log("=================KEY DOWN Key_menu =============");
                }

                if (Input.GetKeyDown(KeyCode.L_JoyStick.ToString()))
                {
                    Debug.Log("=================KEY DOWN JoyStick =============");
                }
            }
            //if(Input.ImuLinearKey != null)
            //    Debug.Log($"ImuLinearKey:{Input.ImuLinearKey.L1}, {Input.ImuLinearKey.L2}");
            //if(Input.ImuQuaternion != null)
            //    Debug.Log($"ImuQuaternion:{Input.ImuQuaternion.x}, {Input.ImuQuaternion.y}, {Input.ImuQuaternion.z}, {Input.ImuQuaternion.w}");
          
            if(Input.ImuAccelerometer_L != null)
                Debug.Log($"ImuAccelerometer_L:{Input.ImuAccelerometer_L.x}, {Input.ImuAccelerometer_L.y}, {Input.ImuAccelerometer_L.z}");
            if(Input.ImuAccelerometer_R != null)
                Debug.Log($"ImuAccelerometer_R:{Input.ImuAccelerometer_R.x}, {Input.ImuAccelerometer_R.y}, {Input.ImuAccelerometer_R.z}");
            if(Input.ImuQuaternion_L != null)
                Debug.Log($"ImuQuaternion_L:{Input.ImuQuaternion_L.x}, {Input.ImuQuaternion_L.y}, {Input.ImuQuaternion_L.z}, {Input.ImuQuaternion_L.w}");
            if(Input.ImuQuaternion_R != null)
                Debug.Log($"ImuQuaternion_R:{Input.ImuQuaternion_R.x}, {Input.ImuQuaternion_R.y}, {Input.ImuQuaternion_R.z}, {Input.ImuQuaternion_R.w}");
              */
        }

       /// <summary>
       /// 检测所有的按键信息
       /// </summary>
        private void CheckAllKeyDown()
        {
            var keyStatusMap = Input.KeyStatusMap;
            if (keyStatusMap != null && keyStatusMap.Count > 0)
            {
                foreach(KeyValuePair<KeyCode, Input.KeyStatusData> valuePair in keyStatusMap)
                {
                    Input.KeyStatusData keyData = valuePair.Value;
                    if (keyData.Record != KeyStatus.Down)
                    {
                        if (keyData.Current == KeyStatus.Down)
                        {
                            string keyCode = valuePair.Key.ToString();
                            keyData.Record = KeyStatus.Down;
                            Input.AddKey(keyCode, true);
                            var ob = Observable.IntervalFrame(0, FrameCountType.EndOfFrame).Subscribe(_ =>
                            {
                                Input.RemoveKey(keyCode, true);
                                _disposables[keyCode + "_Down"]?.Dispose();
                                _disposables.Remove(keyCode + "_Down");
                            });
                            _disposables.Add(keyCode + "_Down", ob);
                        }
                    }
                    else if (keyData.Record != KeyStatus.Up)
                    {
                        if (keyData.Current  == KeyStatus.Up)
                        {
                            string keyCode = valuePair.Key.ToString();
                            keyData.Record = KeyStatus.Up;
                            Input.AddKey(keyCode, false);
                            var ob = Observable.IntervalFrame(0, FrameCountType.EndOfFrame).Subscribe(_ =>
                            {
                                Input.RemoveKey(keyCode, false);
                                _disposables[keyCode + "_Up"]?.Dispose();
                                _disposables.Remove(keyCode + "_Up");
                            });
                            _disposables.Add(keyCode + "_Up", ob);
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// 接收到的数据
        /// </summary>
        /// <param name="message"></param>
        private void OnReceived(string message)
        {
            var handleMessage = (HandleUpdateMessage) ImuProtocol.UnMarshal(message);
            if (handleMessage != null)
            {
                bool isLeftImu = (handleMessage.device_id == (int) EHandleType.LeftHandle);
                //version = _LHandleMessage.version;
                //device_id = _LHandleMessage.device_id;
                //timestamp = _LHandleMessage.timestamp;
                switch (handleMessage.sensor_type)
                {
                    case "input":
                        var keys = handleMessage.input.keys;
                        var joystick = handleMessage.input.joystick;
                        var linearKey = handleMessage.input.linear_key;
                        _heartRate = handleMessage.input.heart_rate;
                        _bloodOxygen = handleMessage.input.blood_oxygen;
                        if (isLeftImu)
                        {
                            Input.KeyStatusMap[KeyCode.L_Key_A].Current = keys.Key_A;
                            Input.KeyStatusMap[KeyCode.L_Key_B].Current = keys.Key_B;
                            Input.KeyStatusMap[KeyCode.L_Key_Menu].Current = keys.Key_menu;
                            Input.KeyStatusMap[KeyCode.L_JoyStick].Current = joystick.Key;
                            
                            Input.KeyValueMap[KeyCode.L_L1Key] = linearKey.L1;
                            Input.KeyValueMap[KeyCode.L_L2Key] = linearKey.L2;
                            Input.KeyValueMap[KeyCode.L_JoyStack_H] = joystick.x;
                            Input.KeyValueMap[KeyCode.L_JoyStack_V] = joystick.y;
                        }
                        else
                        {
                            Input.KeyStatusMap[KeyCode.R_Key_A].Current = keys.Key_A;
                            Input.KeyStatusMap[KeyCode.R_Key_B].Current = keys.Key_B;
                            Input.KeyStatusMap[KeyCode.R_Key_Menu].Current = keys.Key_menu;
                            Input.KeyStatusMap[KeyCode.R_JoyStick].Current = joystick.Key;
                            
                            Input.KeyValueMap[KeyCode.R_L1Key] = linearKey.L1;
                            Input.KeyValueMap[KeyCode.R_L2Key] = linearKey.L2;
                            Input.KeyValueMap[KeyCode.R_JoyStack_H] = joystick.x;
                            Input.KeyValueMap[KeyCode.R_JoyStack_V] = joystick.y;
                        }
                        CheckAllKeyDown();
                        break;
                    case "imu":
                        Quaternions quat = handleMessage.imu.quaternions;
                        if (quat?.x == null) return;
                        var quaternion = new Quaternion(-Convert.ToSingle(quat.x), -Convert.ToSingle(quat.z), -Convert.ToSingle(quat.y), Convert.ToSingle(quat.w)) * Quaternion.Euler(0f,180f,0f);
                        if (isLeftImu)
                        {
                            Input.ImuQuaternion_L = quaternion;
                            Input.ImuAccelerometer_L = handleMessage.imu.accelerometer;
                            Input.ImuGyroscope_L = handleMessage.imu.gyroscope;
                            Input.ImuMagnetometer_L = handleMessage.imu.magnetometer;
                        }
                        else
                        {
                            Input.ImuQuaternion_R = quaternion;
                            Input.ImuAccelerometer_R = handleMessage.imu.accelerometer;;
                            Input.ImuGyroscope_R = handleMessage.imu.gyroscope;
                            Input.ImuMagnetometer_R = handleMessage.imu.magnetometer;
                        }
                        break;
                }
            }
        }

        // void OnGUI()
        // {
        //     GUIStyle style = new GUIStyle();
        //     style.fontSize = Mathf.RoundToInt(Screen.height * 0.02f);
        //     GUI.Label(new Rect(10, 0, 0, 0),
        //         $"device_id: {device_id}", style);
        // }
    }
}
using System;
using UnityEngine;

namespace IMU
{
    public enum EHandleType
    {
        /// <summary>
        /// 左手柄 0
        /// </summary>
        LeftHandle,

        /// <summary>
        /// 右手柄 1
        /// </summary>
        RightHandle,
    }

    public class Protocol_handle
    {
        public static HandleMessageBase UnMarshal(string message)
        {
            try
            {
                //Debug.LogError("message:"+message);
                return UpdateMessageHandler(message);
            }
            catch (Exception e)
            {
                Debug.LogError("e:" + e);
                return null;
            }
        }

        private static HandleUpdateMessage UpdateMessageHandler(string message)
        {
            var body = JsonUtility.FromJson<HandleUpdateMessage>(message);
            return body;
        }
    }

    public interface HandleMessageBase
    {
    }
    /*
     let imuObj={
         "type":"imu",
         "version":"0.0.0", // string
         "device_id":0,//0=left, 1=right
         "timestamp":2814744,
         "seq":371349,
         "accelerometer":{"x":"0.1328125","y":"0.9379883","z":"0.3442383"}, //string !!
         "gyroscope":{"x":"15.0756836","y":"-5.2490234","z":"3.8452148"},
         "magnetometer":{"x":598400,"y":575824,"z":387632},
         "quaternions":{"x":"0.3001404","y":"0.3063965","z":"0.5061035","w":"0.7482300"}
         }
     
     let inputObj={
          "type": "input",
          "version": "0.0.0",
          "device_id": 255,
          "timestamp": 0,
          "keys": {
              "key_A": 0,
              "key_B": 0,
              "key_menu": 0
          },
          "linear_key": {
              "L1": 0,
              "L2": 0
          },
          "joystick": {
              "x": 0,
              "y": 0,
              "key": 0
          },
          "heart_rate": 0,
          "blood_oxygen": 0
        };

     let deviceObj={
          "type": "device_info",
          "version": "0.0.0",
          "device_id": 0,
          "key_size": 0,
          "imu_type": 0,
          "linear_key_size": 0,
          "joystick": 0,
          "vibration": 0,
          "vibration_strength": 0,
          "bat_percent": 0
        };
    */

    [Serializable]
    public class HandleUpdateMessage : HandleMessageBase
    {
        public string type;
        public string version;
        public uint device_id;
        public uint timestamp;
        public uint seq;
        public Accelerometer accelerometer;
        public Gyroscope gyroscope;
        public Magnetometer magnetometer;
        public Quaternions quaternions;
    }

    [Serializable]
    public class Accelerometer
    {
        public string x;
        public string y;
        public string z;
    }

    [Serializable]
    public class Gyroscope
    {
        public string x;
        public string y;
        public string z;
    }

    [Serializable]
    public class Magnetometer
    {
        //string 和 int格式都是可以读到数据的
        public int x;
        public int y;
        public int z;
    }

    [Serializable]
    public class Quaternions
    {
        public string x;
        public string y;
        public string z;
        public string w;
    }
}
using System;
using Newtonsoft.Json;
using UnityEngine;

namespace IMU
{
    public class ImuProtocol
    {
        public static HandleMessageBase UnMarshal(string message)
        {
            try
            {
                return UpdateMessageHandler(message);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError("e:" + e);
                return null;
            }
        }

        private static HandleUpdateMessage UpdateMessageHandler(string message)
        {
            HandleUpdateMessage body = JsonConvert.DeserializeObject<HandleUpdateMessage>(message);
            if (body.sensor_type == EImuDataType.input.ToString())
            {
                if(body.input.joystick != null) ConvertToSteer(ref body.input.joystick);
                if(body.input.linear_key != null)ConvertToPercent(ref body.input.linear_key);    
            }
            return body;
        }
        private static void ConvertToSteer(ref Joystick joystick)
        {
            joystick.x = (joystick.x - 32768) / 32768f;
            joystick.y = (joystick.y - 32768) / 32768f;
        }
        
        private static void ConvertToPercent(ref LinearKey linearKey)
        {
            linearKey.L1 = (65535f - linearKey.L1) / 65535f;
            linearKey.L2 = (65535f - linearKey.L2) / 65535f;
        }

    }

   
    public interface HandleMessageBase
    {
    }
    
    [Serializable]
    public class InputData
    {
        public Keys keys;
        public LinearKey linear_key;
        public Joystick joystick;
        public int heart_rate;
        public int blood_oxygen;
    }
    
    [Serializable]
    public class ImuData
    {
        public Vector3 accelerometer;
        public Gyroscope gyroscope;
        public Magnetometer magnetometer;
        public Quaternions quaternions;
    }
    
    [Serializable]
    public class HandleUpdateMessage : HandleMessageBase
    {
        public string type;
        public string sensor_type;
        public string version;
        public uint device_id;
        public uint timestamp;
        public uint seq;

        public InputData input;
        //public Keys keys;
        //public LinearKey linear_key;
        //public Joystick joystick;
        //public int heart_rate;
        //public int blood_oxygen;

        public ImuData imu;
        //public Vector3 accelerometer;
        //public Gyroscope gyroscope;
        //public Magnetometer magnetometer;
        //public Quaternions quaternions;
    }
    
    public enum EHandleType
    {
        //左手柄 0
        LeftHandle = 0,
        //右手柄 1
        RightHandle = 1,
    }
    
    public enum EImuDataType
    {
        input = 0,
        imu = 1,
    }


    public enum KeyStatus
    {
        Down = 0,
        Up = 1,
        Hold = 2
    }
    
    //[Serializable]
    //public class Accelerometer
    //{
    //    public float x;
    //    public float y;
    //    public float z;
    //}

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
        public float x;
        public float y;
        public float z;
    }

    [Serializable]
    public class Quaternions
    {
        public float x;
        public float y;
        public float z;
        public float w;
    }

    [Serializable]
    public class Keys: HandleMessageBase
    {
        public KeyStatus Key_A;
        public KeyStatus Key_B;
        public KeyStatus Key_menu;
    }

    [Serializable]
    public class LinearKey: HandleMessageBase
    {
        public float L1;
        public float L2;
    }

    [Serializable]
    public class Joystick: HandleMessageBase
    {
        public float x;
        public float y;
        public KeyStatus Key;
    }
}
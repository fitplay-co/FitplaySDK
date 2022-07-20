using System;
using System.Collections.Generic;
using UnityEngine;

namespace SEngineBasic
{
    internal static class Protocol
    {
        public static IMessage UnMarshal(string message)
        {
            try
            {
                return UpdateMessageHandler(message);
            }
            catch (Exception e)
            {
                Debug.LogError("e:" + e);
                return null;
            }
        }
        private static IMessage UpdateMessageHandler(string message)
        {
            var messageBase = JsonUtility.FromJson<MessageBase>(message);
            switch (messageBase.sensor_type)
            {
                case "imu":
                    var imuValue = JsonUtility.FromJson<ImuMessage>(message);
                    FitBar.SetMessages(imuValue);
                    //Debug.LogError("imuValue:" + message);
                    //Debug.LogError($"imuValue: {JsonUtility.ToJson(imuValue)}");
                    return imuValue;
                case "input":
                    var inputValue = JsonUtility.FromJson<InputMessage>(message);
                    //Debug.LogError("inputValue:" + message);
                    //Debug.LogError($"inputValue: {JsonUtility.ToJson(inputValue)}");
                    FitBar.SetMessages(inputValue);
                    return inputValue;
                default:
                    var body = JsonUtility.FromJson<IKBodyMessage>(message);
                    //Debug.LogError("body:" + message);
                    //Debug.LogError($"body: {JsonUtility.ToJson(body)}");
                    return body;
            }
        }
    }

    public interface IMessage { }

    #region 体感数据
    [Serializable]
    public class IKBodyMessage : IMessage
    {
        public string type;
        public Fitting fitting;
        public PoseLandmarkItem pose_landmark;
        public TimeProfiling timeProfiling;
        public GroundLocationItem ground_location;
        public GazeTracking gaze_tracking;
        public ActionDetectionItem action_detection;
        public MonitorItem monitor;
    }

    [Serializable]
    public struct Fitting
    {
        public List<FittingRotationItem> mirrorRotation;
        public List<FittingRotationItem> rotation;
        
        public List<FittingRotationItem> localRotation;
        public List<FittingRotationItem> mirrorLocalRotation;
    }

    [Serializable]
    public struct FittingRotationItem
    {
        public string name;
        public float x;
        public float y;
        public float z;
        public float w;
        
        public Quaternion Rotation()
        {
            return new Quaternion(x, y, z, w);
        }
    }
    
    [Serializable]
    public struct PoseLandmarkItem
    {
        public List<KeyPointItem> keypoints;
        public List<KeyPointItem> keypoints3D;
        public int timestamp;
        public string version;
    }
    
    [Serializable]
    public struct KeyPointItem
    {
        public float x;
        public float y;
        public float z;
        public float score;
        public string name;
    }
    
    [Serializable]
    public struct TimeProfiling
    {
        public long startTime;
        public long afterPoseDetection;
        public long afterDrawingWeb;
        public long beforeSend;
        public long serverReceive;
        public long processingTime;
        public long beforeSendTime;
    }
    
    [Serializable]
    public struct GroundLocationItem
    {
        public float x;
        public float y;
        public float z;
        public float legLength;
        public bool tracing;
    }

    
    [Serializable]
    public struct GazeTracking
    {
        public float x;
        public float y;
        public float z;
        public bool tracing;
    }

    
    [Serializable]
    public struct WalkActionItem
    {
        public int legUp;
        public int rightLeg;
        public float leftFrequency;
        public float rightFrequency;
        public int leftHipAng;
        public int rightHipAng;
        public float leftStepLength;
        public float rightStepLength;
    }
    
    [Serializable]
    public struct JumpActionItem
    {
        public int up;
        public float strength;
    }
    
    [Serializable]
    public struct ActionDetectionItem
    {
        public string version;
        public WalkActionItem walk;
        public JumpActionItem jump;
    }
    
    [Serializable]
    public struct MonitorItem
    {
        public float rawData_z;
        public float watchData_z;
        public float rawData_x;
        public float watchData_x;
        public float rawData_y;
        public float watchData_y;
    }
    #endregion

    #region imu数据
    [Serializable]
    public class MessageBase : IMessage
    {
        public string type;
        public string sensor_type;
        public string version;
        public uint device_id;
        public uint timestamp;

        public EHandleType HandleType => device_id == 0 ? EHandleType.LeftHandle : EHandleType.RightHandle;
    }
    
    [Serializable]
    public class ImuMessage : MessageBase
    {
        public Imu imu;
    }
    
    [Serializable]
    public class Imu
    {
        public uint seq;
        /// <summary>
        /// 加速计X,Y,Z轴数据
        /// </summary>
        public Accelerometer accelerometer;

        /// <summary>
        /// 陀螺仪X,Y,Z轴数据
        /// </summary>
        public Gyroscope gyroscope;

        /// <summary>
        /// 磁力计X,Y,Z轴数据
        /// </summary>
        public Magnetometer magnetometer;

        /// <summary>
        /// 四元数X,Y,Z,W
        /// </summary>
        public Quaternions quaternions;
    }
    
    [Serializable]
    public class Accelerometer
    {
        public float x;
        public float y;
        public float z;
    }

    [Serializable]
    public class Gyroscope
    {
        public float x;
        public float y;
        public float z;
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
    #endregion

    #region input
    public class InputMessage : MessageBase
    {
        public Input input;
    }

    [Serializable]
    public class Input
    {
        public Keys keys;
        public LinearKey linear_key;
        public Joystick joystick;

        public uint heart_rate;
        public uint blood_oxygen;
    }

    [Serializable]
    public class Keys
    {
        public uint key_A;
        public uint key_B;
        public uint key_menu;
    }

    [Serializable]
    public class LinearKey
    {
        public uint L1;
        public uint L2;
    }

    [Serializable]
    public class Joystick
    {
        public uint x;
        public uint y;
        public uint key;
    }
    #endregion
}
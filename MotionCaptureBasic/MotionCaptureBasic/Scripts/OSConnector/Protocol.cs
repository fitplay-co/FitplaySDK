using System;
using System.Collections.Generic;
using UnityEngine;

namespace MotionCaptureBasic.OSConnector
{
    public enum EJointType
    {
        /// <summary>
        /// 鼻子 0
        /// </summary>
        Nose = 0,
        /// <summary>
        /// 右眼内侧 1
        /// </summary>
        RightEyeInner,
        /// <summary>
        /// 右眼 2
        /// </summary>
        RightEye,
        /// <summary>
        /// 右眼外侧 3
        /// </summary>
        RightEyeOuter,
        /// <summary>
        /// 左眼内侧 4
        /// </summary>
        LeftEyeInner,
        /// <summary>
        /// 左眼 5
        /// </summary>
        LeftEye,
        /// <summary>
        /// 左眼外侧 6
        /// </summary>
        LeftEyeOuter,
        /// <summary>
        /// 右耳 7
        /// </summary>
        RightEar,
        /// <summary>
        /// 左耳 8
        /// </summary>
        LeftEar,
        /// <summary>
        /// 右嘴角 9
        /// </summary>
        RightMouth,
        /// <summary>
        /// 左嘴角 10
        /// </summary>
        LeftMouth,
        /// <summary>
        /// 右肩 11
        /// </summary>
        RightShoulder,
        /// <summary>
        /// 左肩 12
        /// </summary>
        LeftShoulder,
        /// <summary>
        /// 右肘 13
        /// </summary>
        RightElbow,
        /// <summary>
        /// 左肘 14
        /// </summary>
        LeftElbow,
        /// <summary>
        /// 右手腕 15
        /// </summary>
        RightWrist,
        /// <summary>
        /// 左手腕 16
        /// </summary>
        LeftWrist,
        /// <summary>
        /// 右小指关节 17
        /// </summary>
        RightPinkyKnuckle,
        /// <summary>
        /// 左小指关节 18
        /// </summary>
        LeftPinkyKnuckle,
        /// <summary>
        /// 右指关节 19
        /// </summary>
        RightIndexKnuckle,
        /// <summary>
        /// 左指关节 20
        /// </summary>
        LeftIndexKnuckle,
        /// <summary>
        /// 右拇指指关节 21
        /// </summary>
        RightThumbKnuckle,
        /// <summary>
        /// 左拇指指关节 22
        /// </summary>
        LeftThumbKnuckle,
        /// <summary>
        /// 右髋 23
        /// </summary>
        RightHip,
        /// <summary>
        /// 左髋 24
        /// </summary>
        LeftHip,
        /// <summary>
        /// 右膝 25
        /// </summary>
        RightKnee,
        /// <summary>
        /// 左膝 26
        /// </summary>
        LeftKnee,
        /// <summary>
        /// 右脚踝 27
        /// </summary>
        RightAnkle,
        /// <summary>
        /// 左脚踝 28
        /// </summary>
        LeftAnkle,
        /// <summary>
        /// 右脚根 29
        /// </summary>
        RightHeel,
        /// <summary>
        /// 左脚根 30
        /// </summary>
        LeftHeel,
        /// <summary>
        /// 右脚指 31
        /// </summary>
        RightFootIndex,
        /// <summary>
        /// 左脚指 32
        /// </summary>
        LeftFootIndex,
        
        /// <summary>
        /// 骨骼点总数
        /// </summary>
        JointCount = LeftFootIndex + 1,
    }

    public enum EFKType
    {
        Neck = 0,
        Head,
        LShoulder,
        RShoulder,
        LArm,
        RArm,
        LWrist,
        RWrist,
        LHand,
        RHand,
        LHip,
        RHip,
        LKnee,
        RKnee,
        LAnkle,
        RAnkle,
        LFoot,
        RFoot,
        CenterHip,
        
        Count = RFoot + 1
    }

    public class SensorType
    {
        public static string CAMERA = "camera";
        public static string IMU = "imu";
        public static string INPUT = "input";
    }
    public static partial class EnumExtend
    {
        public static int Int(this EFKType i)
        {
            return (int)i;
        }
    }
    public class Protocol
    {
        private static IKBodyUpdateMessage _bodyMsgCache = new IKBodyUpdateMessage();
        public static IKBodyMessageBase UnMarshal(string message)
        {
            try
            {
                //Debug.Log("message:"+message);
                return UpdateMessageHandler(message);
            }
            catch (Exception e)
            {
                Console.WriteLine("e:" + e);
                return null;
            }
        }
        
        public static MessageType UnMarshalType(string message)
        {
            try
            {
                //Debug.Log("message:"+message);
                return UpdateMessageTypeHandler(message);
            }
            catch (Exception e)
            {
                Console.WriteLine("e:" + e);
                return null;
            }
        }

        private static void CheckBodyMsg(IKBodyUpdateMessage bodyMsg)
        {
            var poseLandmarkItem = bodyMsg.pose_landmark;
            if (poseLandmarkItem != null)
            {
                if (poseLandmarkItem.keypoints != null && poseLandmarkItem.keypoints3D != null)
                {
                    _bodyMsgCache.pose_landmark = poseLandmarkItem;
                }
                else
                {
                    _bodyMsgCache.pose_landmark = null;
                }
            }

            var fittingItem = bodyMsg.fitting;
            if (fittingItem != null)
            {
                if (fittingItem.rotation != null || fittingItem.localRotation != null)
                {
                    _bodyMsgCache.fitting = fittingItem;
                }
                else
                {
                    _bodyMsgCache.fitting = null;
                }
            }

            var actionDetectionItem = bodyMsg.action_detection;
            if (actionDetectionItem != null)
            {
                if (actionDetectionItem.walk != null)
                {
                    _bodyMsgCache.action_detection = actionDetectionItem;
                }
                else
                {
                    _bodyMsgCache.action_detection = null;
                }
            }

            //TODO: currently no need to check
            _bodyMsgCache.timeProfiling = bodyMsg.timeProfiling;
            _bodyMsgCache.type = bodyMsg.type;
            _bodyMsgCache.ground_location = bodyMsg.ground_location;
            _bodyMsgCache.gaze_tracking = bodyMsg.gaze_tracking;
            _bodyMsgCache.monitor = bodyMsg.monitor;
        }

        
        private static IKBodyUpdateMessage UpdateMessageHandler(string message)
        {
            message = message.Replace("μs", "");
            var body = Newtonsoft.Json.JsonConvert.DeserializeObject<IKBodyUpdateMessage>(message);
            //var body = JsonUtility.FromJson<IKBodyUpdateMessage>(message);
            return body;
        }
        
        private static MessageType UpdateMessageTypeHandler(string message)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<MessageType>(message);
            return JsonUtility.FromJson<MessageType>(message);
        }
    }

    
    [Serializable]
    public class MessageType : HandleMessageBase
    {
        public string sensor_type;
    }

    public interface IKBodyMessageBase { }

    public interface HandleMessageBase { }
    [Serializable]
    public class IKBodyUpdateMessage : IKBodyMessageBase
    {
        public Fitting fitting;
        public PoseLandmarkItem pose_landmark;
        public TimeProfiling timeProfiling;
        public string type;
        public GroundLocationItem ground_location;
        public GazeTracking gaze_tracking;
        public ActionDetectionItem action_detection;
        public MonitorItem monitor;
        public StandDetection stand_detection;
        public GeneralDetectionItem general_detection;

        public KeyPointItem GetJointMessage3D(EJointType jointType)
        {
            var index = (int) jointType;
            return GetJointMessage3D(index);
        }

        public KeyPointItem GetJointMessage3D(int index)
        {
            if(index >= GetKeyPointsCount3D()) return new KeyPointItem();
            var item = pose_landmark.keypoints3D[index];
            return item;
        }

        public int GetKeyPointsCount3D()
        {
            return pose_landmark.keypoints3D?.Count ?? 0;
        }
        
        public KeyPointItem GetJointMessage(EJointType jointType)
        {
            var index = (int) jointType;
            return GetJointMessage(index);
        }

        public KeyPointItem GetJointMessage(int index)
        {
            if(index >= GetKeyPointsCount()) return new KeyPointItem();
            var item = pose_landmark.keypoints[index];
            return item;
        }

        public int GetKeyPointsCount()
        {
            return pose_landmark.keypoints?.Count ?? 0;
        }
    }

    [Serializable]
    public class HandleUpdateMessage : HandleMessageBase
    {
        public string type;
        public string sensor_type;
        public string version;
        public uint device_id;
        public long timestamp;
        public uint seq;

        public Keys keys;
        public LinearKey linear_key;
        public Joystick joystick;
        public int heart_rate;
        public int blood_oxygen;

        public Vector3 accelerometer;
        public Gyroscope gyroscope;
        public Magnetometer magnetometer;
        public Quaternions quaternions;

    }

    # region Motion Capture Data
    [Serializable]
    public class Fitting
    {
        public List<FittingPositionItem> keypoints3D;
        public List<FittingRotationItem> rotation;
        public List<FittingRotationItem> mirrorRotation;
        public List<FittingRotationItem> localRotation;
        public List<FittingRotationItem> mirrorLocalRotation;
    }
    [Serializable]
    public class FittingPositionItem
    {
        public string name;
        public float x;
        public float y;
        public float z;

        public Vector3 Position()
        {
            return new Vector3(x, y, z);
        }
    }
    [Serializable]
    public class FittingRotationItem
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
    public class MonitorItem
    {
        public float rawData_z;
        public float watchData_z;
        public float rawData_x;
        public float watchData_x;
        public float rawData_y;
        public float watchData_y;
    }

    [Serializable]  
    public class PoseLandmarkItem
    {
        public List<KeyPointItem> keypoints;
        public List<KeyPointItem> keypoints3D;
        public long timestamp;
        public string version;
    }

    [Serializable]
    public class KeyPointItem
    {
        public float x;
        public float y;
        public float z;
        public float score;
        public string name;
        
        public Vector3 Position
        {
            get
            {
                var position = new Vector3(-x, -y, -z);
                return position;
            }
        }
        
        public Quaternion Rotation
        {
            get
            {
                return Quaternion.identity;
            }
        }
    }

    [Serializable]
    public class TimeProfiling
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
    public class GroundLocationItem
    {
        public float x;
        public float y;
        public float z;
        public float legLength;
        public bool tracing;
    }
    
    [Serializable]
    public class GazeTracking
    {
        public float x;
        public float y;
        public float z;
    }

    [Serializable]
    public class ActionDetectionItem
    {
        public string version;
        public WalkActionItem walk;
        public JumpActionItem jump;
    }

    [Serializable]
    public class GeneralDetectionItem
    {
        public string version;
        public int confidence;
    }

    [Serializable]
    public class WalkActionItem
    {
        public static bool useRealtimeData;
        public int legUp;
        public int frequency;
        public float strength;
        public int leftLeg;
        public int rightLeg;
        public float leftStrength;
        public float rightStrength;
        public float leftStepLength;
        public float rightStepLength;
        public float leftHipAng;
        public float rightHipAng;
        public float leftFrequency;
        public float rightFrequency;
        public float velocity;
        public float velocityThreshold;
        public float stepRate;
        public float stepLen;
        public int realtimeLeftLeg;
        public int realtimeRightLeg;

        public int GetLeftLeg()
        {
            return useRealtimeData ? realtimeLeftLeg : leftLeg;
        }

        public int GetRightLeg()
        {
            return useRealtimeData ? realtimeRightLeg : rightLeg;
        }

        public void SetLeftLeg(int value)
        {
            leftLeg = value;
            realtimeLeftLeg = value;
        }

        public void SetRightLeg(int value)
        {
            rightLeg = value;
            realtimeRightLeg = value;
        }
    }
    
    [Serializable]
    public class JumpActionItem
    {
        public int up;
        public float strength;
    }

    [Serializable]
    public class StandDetection
    {
        public int mode;
    }
    #endregion

    /// <summary>
    /// IMU 数据块
    /// </summary>
    #region IMU Data
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

    public enum KeyStatus
    {
        Down = 0,
        Up = 1,
        Hold = 2
    }
    #endregion    
}

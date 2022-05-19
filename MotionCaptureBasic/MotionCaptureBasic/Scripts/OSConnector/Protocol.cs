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

    public class Protocol
    {
        public static IKBodyMessageBase UnMarshal(string message)
        {
            try
            {
                //Console.WriteLine("message:"+message);
                return UpdateMessageHandler(message);
            }
            catch (Exception e)
            {
                Console.WriteLine("e:" + e);
                return null;
            }
        }
        private static IKBodyUpdateMessage UpdateMessageHandler(string message)
        {
            var body = JsonUtility.FromJson<IKBodyUpdateMessage>(message);
            return body;
        }
    }

    public interface IKBodyMessageBase { }

    [Serializable]
    public class IKBodyUpdateMessage : IKBodyMessageBase
    {
        public PoseLandmarkItem pose_landmark;
        public TimeProfiling timeProfiling;
        public string type;
        public GroundLocationItem ground_location;
        public GazeTracking gaze_tracking;
        public ActionDetectionItem action_detection;
        public MonitorItem monitor;

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
        public int timestamp;
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
    public class WalkActionItem
    {
        public int legUp;
        public int frequency;
        public float strength;
    }
    
    [Serializable]
    public class JumpActionItem
    {
        public int up;
        public float strength;
    }
}

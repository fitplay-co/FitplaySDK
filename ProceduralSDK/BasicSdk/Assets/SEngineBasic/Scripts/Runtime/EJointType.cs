namespace SEngineBasic
{
    /// <summary>
    /// IK骨骼点
    /// </summary>
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
    
    /// <summary>
    /// FK骨骼点
    /// </summary>
    public enum EFKType
    {
        Spine = 0,
        Neck,
        LShoulder,
        RShoulder,
        LUpArm,
        RUpArm,
        LLowArm,
        RLowArm,
        LHand,
        RHand,
        LHip,
        RHip,
        LUpLeg,
        RUpLeg,
        LLowLeg,
        RLowLeg,
        LFoot,
        RFoot,
        Root,
        
        Count = Root + 1
    }
    /// <summary>
    /// 左右手柄
    /// </summary>
    public enum EHandleType
    {
        /// <summary>
        /// 左手柄 0
        /// </summary>
        LeftHandle = 0,
        
        /// <summary>
        /// 右手柄 1
        /// </summary>
        RightHandle,
    }
    
    /// <summary>
    /// 手柄按键与遥感
    /// </summary>
    public enum EInputKey
    {
        KeyA = 0,
        KeyB,
        KeyMenu,
        L1,
        L2,
        Joystick,
        JoystickKey,
    }
    /// <summary>
    /// 心率，血氧等
    /// </summary>
    public enum EInputValue
    {
        /// <summary>
        /// 心率
        /// </summary>
        HeartRate,
        /// <summary>
        /// 血氧
        /// </summary>
        BloodOxygen,
    }
    
    public enum EImuKey
    {
        /// <summary>
        /// 加速度
        /// </summary>
        Accelerometer = 0,
        /// <summary>
        /// 陀螺仪
        /// </summary>
        Gyroscope,
        /// <summary>
        /// 磁力计
        /// </summary>
        Magnetometer,
        /// <summary>
        /// rotation
        /// </summary>
        Rotation
    }
    
    public static partial class EnumExtend
    {
        public static int Int(this EJointType i)
        {
            return (int)i;
        }
        
        public static int Int(this EFKType i)
        {
            return (int)i;
        }
        
        public static int Int(this EHandleType i)
        {
            return (int)i;
        }
        
        public static int Int(this EInputKey i)
        {
            return (int)i;
        }
    }
}
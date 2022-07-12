namespace SEngineBasic
{
    public enum EHeartCommandType
    {
        /// <summary>
        /// 关闭心率计
        /// </summary>
        CloseHeartRate = 0,
        /// <summary>
        /// 打开心率计
        /// </summary>
        OpenHeartRate,
        /// <summary>
        /// 关闭血氧计
        /// </summary>
        CloseBloodOxygen,
        /// <summary>
        /// 打开血氧计
        /// </summary>
        OpenBloodOxygen,
    }

    public static partial class EnumExtend
    {
        public static int Int(this EHeartCommandType i)
        {
            return (int)i;
        }
    }

    public class InputHeart : OSApplicationBase
    {
        public string type;
        public string sensor_type;
        public int device_id;
        public int command;
        public InputHeart(EHandleType handleType, EHeartCommandType controlType)
        {
            type = "sensor_control";
            sensor_type = "heart_control";
            device_id = handleType.Int();
            command = controlType.Int();
        }
    }
}
namespace SEngineBasic
{
    public struct ImuZero : OSApplicationBase
    {
        public string type;
        public string sensor_type;
        public int device_id;
        public ImuZero(EHandleType handleType)
        {
            type = "sensor_control";
            sensor_type = "imu_zero";
            device_id = handleType.Int();
        }
    }
}
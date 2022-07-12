namespace SEngineBasic
{
    public struct ImuVibration : OSApplicationBase
    {
        public string type;
        public string sensor_type;
        public int device_id;
        public int vibration_type;
        public int strength;
        public ImuVibration(EHandleType handleType, int vibration_type, int strength)
        {
            type = "sensor_control";
            sensor_type = "vibration";
            device_id = handleType.Int();
            this.vibration_type = vibration_type;
            this.strength = strength;
        }
    }
}
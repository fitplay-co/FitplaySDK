namespace MotionCaptureBasic.MessageSend
{
    public enum MessageType
    {
        none,
        application_client,                 //注册帧
        application_control,                //控制帧
        sensor_control,                     //传感器控制
    }

    public enum SensorType
    {
        none,
        vibration,                          //震动
        imu_zero,                           //IMU置零
        heart_control                       //心率计控制命令
    }
}
namespace MotionCaptureBasic.MessageSend
{
    public static class MessageFactory
    {
        public static object CreateMessageRegister()
        {
            return new MessageBody()
            {
                type = MessageType.application_client.ToString()
            };
        }

        public static object CreateMessageControl(MessageControlFeatureId controlFeatureIdId, MessageControlAction controlAction)
        {
            return new MessageBody()
            {
                type = MessageType.application_control.ToString(),
                action = controlAction.ToString(),
                feature_id = controlFeatureIdId.ToString(),
            };
        }

        public static object CreateMessageControl(MessageControlFeatureId controlFeatureId, bool active)
        {
            var action = active ? MessageControlAction.subscribe : MessageControlAction.release;
            return CreateMessageControl(controlFeatureId, action);
        }
        
        /// <summary>
        /// 控制帧
        /// </summary>
        /// <param name="fps"></param>
        /// <returns></returns>
        public static object CreateConfigMessage(int fps)
        {
            return new MessageConfig()
            {
                type = MessageType.application_control.ToString(),
                feature_id = MessageControlFeatureId.imu.ToString(),
                action = MessageControlAction.config.ToString(),
                data = new Config(){fps = fps}
            };
        }

        /// <summary>
        /// 配置身高的控制帧
        /// </summary>
        /// <param name="h">身高，单位cm</param>
        /// <returns></returns>
        public static object CreateHeightSetMessage(int h)
        {
            return new MessageConfig()
            {
                type = MessageType.application_control.ToString(),
                feature_id = MessageControlFeatureId.action_detection.ToString(),
                action = MessageControlAction.set_player.ToString(),
                data = new Config(){height = h}
            };
        }

        /// <summary>
        /// 震动
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="vibrationType"></param>
        /// <param name="strength"></param>
        /// <returns></returns>
        public static object CreateVibrationMessage(int deviceId, int vibrationType, int strength)
        {
            return new VibrationMessage()
            {
                type = MessageType.sensor_control.ToString(),
                sensor_type = SensorType.vibration.ToString(),
                device_id = deviceId,
                vibration_type = vibrationType,
                strength = strength
            };
        }
        
        /// <summary>
        /// imu重置
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public static object CreateImuResetMessage(int deviceId)
        {
            return new ImuResetMessage()
            {
                type = MessageType.sensor_control.ToString(),
                sensor_type = SensorType.imu_zero.ToString(),
                device_id = deviceId,
            };
        }
        
        /// <summary>
        /// 心率计控制命令
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public static object CreateHeartMessage(int deviceId, int command)
        {
            return new HeartControlMessage()
            {
                type = MessageType.sensor_control.ToString(),
                sensor_type = SensorType.heart_control.ToString(),
                device_id = deviceId,
                command = command
            };
        }
        

        public static object CreateMessageFitting(bool active)
        {
            var action = active ? MessageControlAction.subscribe : MessageControlAction.release;
            return new MessageFitting()
            {
                type = MessageType.application_control.ToString(),
                action = action.ToString(),
                feature_id = MessageControlFeatureId.fitting.ToString(),
                data = MessageFittingType.camera.ToString(),
            };
        }
    }
}
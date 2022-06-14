namespace MotionCaptureBasic.MessageSubscribe
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
            var action = active ? MessageControlAction.subsribe : MessageControlAction.release;
            return CreateMessageControl(controlFeatureId, action);
        }

        public static object CreateMessageFitting(bool active)
        {
            var action = active ? MessageControlAction.subsribe : MessageControlAction.release;
            return new MessageFitting()
            {
                type = MessageType.application_control.ToString(),
                action = action.ToString(),
                feature_id = MessageControlFeatureId.fitting.ToString(),
            };
        }
    }
}
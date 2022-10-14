using FlatBuffers;

namespace MotionCaptureBasic.MessageSend
{
    public static class MessageFlatbufferFactory
    {
        public const string Version = "0.1.0";
        /// <summary>
        /// 生成注册帧消息的flatbuffer字节流
        /// </summary>
        /// <param name="useJson">是否使用json</param>
        /// <returns></returns>
        public static byte[] CreateFlatbufferRegister(bool useJson)
        {
            FlatBufferBuilder builder = new FlatBufferBuilder(1);
            
            //生成flatbuffer用的application_id string offset
            string id = MessageApplicationId.game_app.ToString();
            StringOffset idOffset = builder.CreateString(id);
            
            //生成注册帧client字段的offset
            var clientOffset = ApplicationClient.Client.CreateClient(builder, idOffset, useJson);

            StringOffset versionOffset = builder.CreateString(Version);
            
            //生成注册帧input message的offset
            OsInput.InputMessage.StartInputMessage(builder);
            OsInput.InputMessage.AddVersion(builder, versionOffset);
            OsInput.InputMessage.AddType(builder, OsInput.MessageType.ApplicationClient);
            OsInput.InputMessage.AddClient(builder, clientOffset);
            var result = OsInput.InputMessage.EndInputMessage(builder);
            
            OsInput.InputMessage.FinishInputMessageBuffer(builder, result);

            return builder.SizedByteArray();
        }

        /// <summary>
        /// 生成订阅帧的flatbuffer字节流
        /// </summary>
        /// <param name="controlFeatureId"></param>
        /// <param name="controlAction"></param>
        /// <returns></returns>
        public static byte[] CreateFlatbufferControl(MessageControlFeatureId controlFeatureId, MessageControlAction controlAction)
        {
            FlatBufferBuilder builder = new FlatBufferBuilder(1);

            string action = controlAction.ToString();
            StringOffset actionOffset = builder.CreateString(action);

            string featureId = controlFeatureId.ToString();
            StringOffset featureIdOffset = builder.CreateString(featureId);

            var controlOffset = ApplicationControl.Control.CreateControl(builder, featureIdOffset, actionOffset);
            
            StringOffset versionOffset = builder.CreateString(Version);
            
            //生成订阅帧input message的offset
            OsInput.InputMessage.StartInputMessage(builder);
            OsInput.InputMessage.AddVersion(builder, versionOffset);
            OsInput.InputMessage.AddType(builder, OsInput.MessageType.ApplicationControl);
            OsInput.InputMessage.AddControl(builder, controlOffset);
            var result = OsInput.InputMessage.EndInputMessage(builder);
            
            OsInput.InputMessage.FinishInputMessageBuffer(builder, result);
            
            return builder.SizedByteArray();
        }
        
        public static byte[] CreateFlatbufferControl(MessageControlFeatureId controlFeatureId, bool active)
        {
            var action = active ? MessageControlAction.subscribe : MessageControlAction.release;
            return CreateFlatbufferControl(controlFeatureId, action);
        }

        /// <summary>
        /// 配置身高的控制帧
        /// </summary>
        /// <param name="h">身高，单位cm</param>
        /// <returns></returns>
        public static byte[] CreateHeightSetFlatbuffer(int h)
        {
            FlatBufferBuilder builder = new FlatBufferBuilder(1);

            string action = MessageControlAction.set_player.ToString();
            StringOffset actionOffset = builder.CreateString(action);

            string featureId = MessageControlFeatureId.action_detection.ToString();
            StringOffset featureIdOffset = builder.CreateString(featureId);

            var controlData = ApplicationControl.ControlData.CreateControlData(builder, 0, h);

            var controlOffset = ApplicationControl.Control.CreateControl(builder, featureIdOffset, actionOffset, controlData);
            
            StringOffset versionOffset = builder.CreateString(Version);
            
            //生成配置身高的控制帧input message的offset
            OsInput.InputMessage.StartInputMessage(builder);
            OsInput.InputMessage.AddVersion(builder, versionOffset);
            OsInput.InputMessage.AddType(builder, OsInput.MessageType.ApplicationControl);
            OsInput.InputMessage.AddControl(builder, controlOffset);
            var result = OsInput.InputMessage.EndInputMessage(builder);
            
            OsInput.InputMessage.FinishInputMessageBuffer(builder, result);
            
            return builder.SizedByteArray();
        }
    }
}
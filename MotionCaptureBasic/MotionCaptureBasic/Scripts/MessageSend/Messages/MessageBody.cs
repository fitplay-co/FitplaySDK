using System;
using UnityEngine;

namespace MotionCaptureBasic.MessageSend
{
    public class MessageBody
    {
        public int id;
        public string type;
        public string action;
        public string feature_id;
    }

    public class MessageRegister
    {
        public string type;
        public string id;
        public bool useJson;
    }

    public class Config
    {
        public int fps;
        public int height;
    }

    public class MessageFitting : MessageBody
    {
        public string data;
    }
    
    public class MessageConfig : MessageBody
    {
        public Config data;
    }
    
    public class ImuResetMessage
    {
        public string type;
        public string sensor_type;
        public int device_id;
        public string timestamp;
    }
    
    public class VibrationMessage:ImuResetMessage
    {
        public int vibration_type;
        public int strength;
    }
    
    public class HeartControlMessage:ImuResetMessage
    {
        public int command;
    }

}
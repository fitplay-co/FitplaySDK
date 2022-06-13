using System;
using UnityEngine;

namespace MotionCaptureBasic.MessageSubscribe
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
    }
}
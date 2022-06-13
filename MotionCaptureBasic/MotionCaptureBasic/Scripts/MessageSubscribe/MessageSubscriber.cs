using System.Text;
using UnityEngine;
using UnityWebSocket;

namespace MotionCaptureBasic.MessageSubscribe
{
    public class MessageSubscriber
    {
        private IWebSocket socket;

        public MessageSubscriber(IWebSocket socket)
        {
            this.socket = socket;
        }

        public bool SendMessageRegister()
        {
            return SendAsync(MessageFactory.CreateMessageRegister());
        }

        public bool SubscribeGazeTracking(bool active)
        {
            return SendAsync(MessageFactory.CreateMessageControl(MessageControlFeatureId.gaze_tracking, active));
        }

        public bool SubscribeGroundLocation(bool active)
        {
            return SendAsync(MessageFactory.CreateMessageControl(MessageControlFeatureId.ground_loccation, active));
        }

        public bool SubscribeActionDetection(bool active)
        {
            return SendAsync(MessageFactory.CreateMessageControl(MessageControlFeatureId.action_detection, active));
        }

        private bool SendAsync(object message)
        {
            if (socket != null)
            {
                Debug.Log(JsonUtility.ToJson(message));
                socket.SendAsync(Encoding.UTF8.GetBytes(JsonUtility.ToJson(message)));
                return true;
            }

            return false;
        }
    }
}
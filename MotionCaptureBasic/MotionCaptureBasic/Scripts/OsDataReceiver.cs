using UnityEngine;

namespace MotionCaptureBasic
{
    public class OsDataReceiver:MonoBehaviour
    {
        //目前有三种类型数据，camera, imu, input
        public void ReceivedOsNormalData(string msg)
        {
                //TODO:收到基本数据的处理
                Debug.Log($"收到了Normal数据：{msg}");
        }

        //接收到的ActionDetection数据
        public void ReceivedActionDetectionData(string msg)
        {
                //TODO:收到数据的处理
                Debug.Log($"收到了ActionDetection数据：{msg}");
        }
        //接收到的GroundLocation数据
        public void ReceivedGroundLocationData(string msg)
        {
                //TODO:收到数据的处理
                Debug.Log($"收到了GroundLocation数据：{msg}");
        }

        //接收到的Fitting数据
        public void ReceivedFittingData(string msg)
        {
            //TODO:收到数据的处理
            Debug.Log($"收到了Fitting数据：{msg}");
        }
    }
}
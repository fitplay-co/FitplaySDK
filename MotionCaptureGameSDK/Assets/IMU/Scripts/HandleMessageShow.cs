using UnityEngine;
using System;
namespace IMU
{
    public class HandleMessageShow : MonoBehaviour
    {
        private HandleUpdateMessage _handleUpdateMessage;
        private string type;
        private string version;
        private uint device_id;
        private uint timestamp;
        private uint seq;
        private Accelerometer acc;
        private Gyroscope gyro;
        private Magnetometer mag;
        private Quaternions quat;

        private Quaternion siys;
        // private float timeee;

        void Start()
        {
            var app = ImuClient.Create();
            app.OnReceived += this.OnReceived;
        }

        void Update()
        {
            // timeee += Time.deltaTime;
            // if (timeee>=0.05f) //os发的数据太快，加个固定时间间隔取值
            // {
            transform.localRotation = siys;
            // Debug.Log($"物体姿态是: {transform.localRotation}");
            //     timeee = 0;
            // }       
        }

        private void OnReceived(string message)
        {
            _handleUpdateMessage = (HandleUpdateMessage) Protocol_handle.UnMarshal(message);
            if (_handleUpdateMessage != null)
            {
                type = _handleUpdateMessage.type;
                version = _handleUpdateMessage.version;
                device_id = _handleUpdateMessage.device_id;
                timestamp = _handleUpdateMessage.timestamp;
                seq = _handleUpdateMessage.seq;
                gyro = _handleUpdateMessage.gyroscope;
                mag = _handleUpdateMessage.magnetometer;
                acc = _handleUpdateMessage.accelerometer;
                quat = _handleUpdateMessage.quaternions;

                if (quat.x == null) return;

                Debug.Log($"type: {type} " +
                          // $"device_id: {device_id}  " +
                          // $"version: {version} " +
                          // $"timestamp: {timestamp} " +
                          "acc: " + acc.x + "; " + acc.y + "; " + acc.z + " " +
                          "gyro: " + gyro.x + "; " + gyro.y + "; " + gyro.z + " " +
                          "quat(x,y,z,w):   " + quat.x + "; " + quat.y + "; " + quat.z + "; " + quat.w + " " +
                          "mag: " + mag.x + "; " + mag.y + "; " + mag.z + " ");

                //构建四元数
                // siys = new Quaternion(Convert.ToSingle(quat.x),
                // Convert.ToSingle(quat.y),  Convert.ToSingle(quat.z), Convert.ToSingle(quat.w));
                siys = new Quaternion(-Convert.ToSingle(quat.x),
                    -Convert.ToSingle(quat.z), -Convert.ToSingle(quat.y), Convert.ToSingle(quat.w));

                // Debug.Log($"四元数是: {siys}");
            }
            else
            {
                // Debug.Log("IK message is not received ");
            }
        }

        // void OnGUI()
        // {
        //     GUIStyle style = new GUIStyle();
        //     style.fontSize = Mathf.RoundToInt(Screen.height * 0.02f);
        //     GUI.Label(new Rect(10, 0, 0, 0),
        //         $"device_id: {device_id}", style);
        // }
    }
}
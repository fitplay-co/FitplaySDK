using System.Collections;
using System.Collections.Generic;
using MotionCaptureBasic;
using UnityEngine;

namespace AndroidOs
{
    public class AndroidOsInit : MonoBehaviour
    {
        // Start is called before the first frame update
        private int startY = 450;
        private string OsDataReceiver = "osDataReceiverObj";

        void Start()
        {
            ExtensionHelper.Initialize();
            ExtensionHelper.InitOsDataHandler(OsDataReceiver);
            Invoke(nameof(SubscribeMotions), 1.0f);
        }

        private void SubscribeMotions()
        {
            ExtensionHelper.SubscribeActionDetection();
            ExtensionHelper.SubscribeGroundLocation();
            ExtensionHelper.SubscribeFitting();
        }

        // Update is called once per frame
        void OnGUI()
        {
            if (GUI.Button(new Rect(50, startY, 200, 50), "订阅AC数据"))
            {
                ExtensionHelper.SubscribeActionDetection();
                Debug.Log("发送订阅数据:{SubscribeActionDetection}");
            }

            if (GUI.Button(new Rect(50, startY + 60, 200, 50), "订阅Ground数据"))
            {
                ExtensionHelper.SubscribeGroundLocation();
                Debug.Log("发送订阅数据:{SubscribeGroundLocation}");
            }

            if (GUI.Button(new Rect(50, startY + 120, 200, 50), "订阅Fitting数据"))
            {
                ExtensionHelper.SubscribeFitting();
                Debug.Log("发送订阅数据:{SubscribeFitting}");
            }
        }
    }
}
using System;
using UnityEngine;
using MotionCaptureBasic;
using MotionCaptureBasic.OSConnector;
using Object = UnityEngine.Object;

namespace StandTravelModel.Scripts.Runtime.TestDemo
{
    public class FakeMobileOSTest : MonoBehaviour
    {
        private TextAsset testData;
        private OsDataReceiver osDataReceiver;
        private StandTravelModelManager standTravelModelManager;

        private bool _isOsConnected = false;

        private bool isOsConnected
        {
            set
            {
                if (_isOsConnected != value)
                {
                    _isOsConnected = value;
                    if (_isOsConnected)
                    {
                        var obj = Object.FindObjectOfType<OsDataReceiver>();
                        SetOsDataReceiver(obj);
                    }
                }
            }
        }

        public void Start()
        {
            testData = Resources.Load<TextAsset>("TestDataWrong");
            standTravelModelManager = FindObjectOfType<StandTravelModelManager>();
        }

        public void FixedUpdate()
        {
            isOsConnected = standTravelModelManager.osConnected;
        }

        public void SetOsDataReceiver(OsDataReceiver comp)
        {
            osDataReceiver = comp;
            //osDataReceiver.onReceiveNormalData += OnReceiveNormalDataTest;
        }

        private void OnReceiveNormalDataTest(string msg)
        {
            if (msg.Contains("action_detection"))
            {
                var index = msg.IndexOf("action_detection", StringComparison.Ordinal);
                var actionDetectionStr = msg.Substring(index);
                //Debug.Log(actionDetectionStr);
            }
            else
            {
                //Debug.LogError(msg);
                IKBodyUpdateMessage iMsg = Protocol.UnMarshal(msg) as IKBodyUpdateMessage;
            }
        }

        public void OnTestBtn()
        {
            string testMsg = "";
            if (testData != null)
            {
                testMsg = testData.text;
            }

            if (osDataReceiver != null)
            {
                osDataReceiver.ReceivedOsNormalData(testMsg);
            }
        }
    }
}
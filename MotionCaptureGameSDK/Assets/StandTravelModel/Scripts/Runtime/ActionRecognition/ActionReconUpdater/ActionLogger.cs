using System.Collections.Generic;
using UnityEngine;
using MotionCaptureBasic;
using Newtonsoft.Json;
using System.IO;
using System.Text;

public class ActionLogger : MonoBehaviour
{
    [SerializeField] private bool isLogging;
    [SerializeField] private float timeOffset;
    [SerializeField] private string logName;

    private struct ActionLoggerData
    {
        public float time;
        public float leftFrequency;
        public float rightFrequency;
        public float leftStepLength;
        public float rightStepLength;
    }

    private List<ActionLoggerData> loggerDatas = new List<ActionLoggerData>();

    private void Update()
    {
        if(Input.GetKeyDown("joystick button 2"))
        {
            Debug.Log("!!!");
            OutputDatas();
        }

        var keyDown = Input.GetKeyDown("joystick button 0");
        if(keyDown)
        {
            isLogging = !isLogging;
        }

        if(!isLogging)
        {
            return;
        }

        var actionDetectionItem = MotionDataModelHttp.GetInstance().GetActionDetectionData();
        if(actionDetectionItem != null)
        {
            var loggerData = new ActionLoggerData()
            {
                time = Time.time - timeOffset,
                leftFrequency = actionDetectionItem.walk.leftFrequency,
                rightFrequency = actionDetectionItem.walk.rightFrequency,
                leftStepLength = actionDetectionItem.walk.leftStepLength,
                rightStepLength = actionDetectionItem.walk.rightStepLength
            };
            loggerDatas.Add(loggerData);
        }
    }

    public void OutputDatas()
    {
        if(loggerDatas.Count == 0)
        {
            for(int i = 0; i < 10; i++)
            {
                var loggerData = new ActionLoggerData()
                {
                    time = Time.time,
                };
                loggerDatas.Add(loggerData);
            }
        }

        var name = string.IsNullOrEmpty(logName) ? "motionLogs" : logName;
        var path = string.Format("C:\\Logs\\{0}.json", name);
        using(var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
        {
            var msg = JsonConvert.SerializeObject(loggerDatas);
            byte[] bytes = Encoding.UTF8.GetBytes(msg);
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
            stream.Close();
        }
    }
}
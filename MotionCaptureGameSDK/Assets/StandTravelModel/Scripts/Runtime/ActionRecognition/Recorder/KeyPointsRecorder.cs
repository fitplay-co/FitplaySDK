using System.Collections.Generic;
using System.IO;
using MotionCaptureBasic;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.ActionRecognition.Recorder
{
    public class KeyPointsRecorder : MonoBehaviour
    {
        [SerializeField] private int gettingIndex;
        [SerializeField] private bool enableRecord;
        [SerializeField] private float recordedAcc;
        [SerializeField] private float recordDelay;
        [SerializeField] private float recordLength;

        private List<Points> deserializeds;
        private List<Points> keyPointsList = new List<Points>();

        private void Update() {
            if(enableRecord)
            {
                recordedAcc += Time.deltaTime;
                if(recordDelay > 0)
                {
                    if(recordedAcc < recordDelay)
                    {
                        return;
                    }
                }

                if(recordLength > 0)
                {
                    if(recordedAcc >= recordLength + recordDelay)
                    {
                        return;
                    }
                }

                var pointsData = MotionDataModelHttp.GetInstance().GetIKPointsData(true, true);
                if(pointsData != null)
                {
                    var points = new Points();
                    points.pointList.AddRange(pointsData);

                    keyPointsList.Add(points);
                }
            }
        }

        public void EnableRecording()
        {
            enableRecord = true;
        }

        public void DisableRecording()
        {
            enableRecord = false;
        }

        public void ClearKeyPointsList()
        {
            keyPointsList.Clear();
        }

        public void OutputKeyPointsList()
        {
            DOOutputKeyPointsList(keyPointsList);
        }

        public List<Vector3> GetRecordKeyPoints()
        {
            return GetRecordKeyPoints(gettingIndex++);
        }

        public List<Vector3> GetRecordKeyPoints(int index)
        {
            TryLoadPoints();

            if(deserializeds != null && deserializeds.Count > 0)
            {
                return deserializeds[index % deserializeds.Count].pointList;
            }

            return null;
        }

        public int GetFrameCount()
        {
            TryLoadPoints();
            return deserializeds != null ? deserializeds.Count : 0;
        }

        public void PrintFilePath()
        {
            Debug.Log(GetFilePath());
        }

        private void TryLoadPoints()
        {
            if(deserializeds == null || deserializeds.Count == 0)
            {
                LoadKeyPointsList();
            }
        }

        private string GetFilePath()
        {
            return Application.persistentDataPath + "/keyPointsList.json";
        }

        private void LoadKeyPointsList()
        {
            using(FileStream fileStream = new FileStream(GetFilePath(), FileMode.OpenOrCreate))
            {
                using(StreamReader streamReader = new StreamReader(fileStream))
                {
                    var json = streamReader.ReadToEnd();
                    var container = JsonUtility.FromJson<PointsContainer>(json);
                    if(container != null)
                    {
                        deserializeds = container.points;
                    }
                }
            }
        }

        private void DOOutputKeyPointsList(List<Points> pointsList)
        {
            using(FileStream fileStream = new FileStream(GetFilePath(), FileMode.Create))
            {
                using(StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.Write("{\"points\":[");

                    var listIdx = 0;
                    foreach(var points in pointsList)
                    {
                        streamWriter.Write("{\"pointList\":[");
                        var pointIdx = 0;
                        foreach(var point in points.pointList)
                        {
                            streamWriter.Write(ToJson(point));
                            if(pointIdx++ < points.pointList.Count - 1)
                            {
                                streamWriter.Write(",");
                            }
                        }
                        streamWriter.Write("]}");

                        if(listIdx++ < pointsList.Count - 1)
                        {
                            streamWriter.Write(",");
                        }
                    }

                    streamWriter.Write("]}");
                }
            }
        }

        private string ToJson(Vector3 point)
        {
            return string.Format(
                "{0}\"x\":{1:0.00000},\"y\":{2:0.00000},\"z\":{3:0.00000}{4}", '{', point.x, point.y, point.z, '}'
            );
        }
    }
}
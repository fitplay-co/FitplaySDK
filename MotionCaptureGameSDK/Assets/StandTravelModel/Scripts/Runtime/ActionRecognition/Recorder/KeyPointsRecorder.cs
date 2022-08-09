using System.Collections.Generic;
using System.IO;
using MotionCaptureBasic;
using UnityEngine;
using Newtonsoft.Json;
using MotionCaptureBasic.OSConnector;

namespace StandTravelModel.Scripts.Runtime.ActionRecognition.Recorder
{
    public class KeyPointsRecorder : MonoBehaviour
    {
        [SerializeField] private int gettingIndex;
        [SerializeField] private bool enableRecord;
        [SerializeField] private float recordedAcc;
        [SerializeField] private float recordDelay;
        [SerializeField] private float recordLength;

        private PointsContainer seContainer;
        private PointsContainer deContainer;

        private void Update() {
            if(enableRecord)
            {
                if(seContainer == null)
                {
                    seContainer = new PointsContainer();
                }

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

                    seContainer.points.Add(points);
                }

                var actionItem = MotionDataModelHttp.GetInstance().GetActionDetectionData();
                if(actionItem != null && actionItem.walk != null)
                {
                    var walk = new Walk()
                    {
                        leftLeg = actionItem.walk.leftLeg,
                        rightLeg = actionItem.walk.rightLeg,
                        leftHip = actionItem.walk.leftHipAng,
                        rightHip = actionItem.walk.rightHipAng
                    };
                    seContainer.walks.Add(walk);
                }
                else
                {
                    seContainer.walks.Add(null);
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
            seContainer.points.Clear();
        }

        public void OutputDatas()
        {
            DOOutputKeyPointsList(seContainer);
        }

        public void GetRecordDatas(out List<Vector3> keyPoints, out ActionDetectionItem actionDetectionItem)
        {
            keyPoints = GetRecordKeyPoints(gettingIndex);
            actionDetectionItem = GetActionDetectionItem(gettingIndex);
            gettingIndex++;
        }

        public List<Vector3> GetRecordKeyPoints(int index)
        {
            TryLoadPoints();

            if(deContainer != null && deContainer.points != null && deContainer.points.Count > 0)
            {
                return deContainer.points[index % deContainer.points.Count].pointList;
            }

            return null;
        }

        private ActionDetectionItem GetActionDetectionItem(int index)
        {
            if(deContainer != null && deContainer.walks != null && deContainer.walks.Count > 0)
            {
                var walk = deContainer.walks[index % deContainer.points.Count];
                if(walk != null)
                {
                    var item = new ActionDetectionItem();
                    item.walk = new WalkActionItem()
                    {
                        leftLeg = walk.leftLeg,
                        rightLeg = walk.rightLeg,
                        leftHipAng = walk.leftHip,
                        rightHipAng = walk.rightHip,
                        leftFrequency = walk.leftFrequency,
                        rightFrequency = walk.rightFrequency,
                        leftStepLength = walk.leftSteplength,
                        rightStepLength = walk.rightStepLength,
                    };
                    return item;
                }
            }
            return null;
        }

        public int GetFrameCount()
        {
            TryLoadPoints();
            return deContainer.points != null ? deContainer.points.Count : 0;
        }

        public void PrintFilePath()
        {
            Debug.Log(GetFilePath());
        }

        private void TryLoadPoints()
        {
            if(deContainer == null || deContainer.points == null || deContainer.points.Count == 0)
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
                    deContainer = JsonConvert.DeserializeObject<PointsContainer>(json);
                }
            }
        }

        private void DOOutputKeyPointsList(PointsContainer container)
        {
            using(FileStream fileStream = new FileStream(GetFilePath(), FileMode.Create))
            {
                using(StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.Write(
                        JsonConvert.SerializeObject(
                            container,
                            new JsonSerializerSettings()
                            { 
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            }
                        )
                    );
                }
            }
        }
    }
}
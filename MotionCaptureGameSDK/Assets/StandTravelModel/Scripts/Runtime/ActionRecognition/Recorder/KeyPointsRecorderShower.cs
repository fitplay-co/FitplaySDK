using System;
using System.Collections.Generic;
using MotionCaptureBasic.Interface;
using StandTravelModel.Scripts.Runtime.ActionRecognition.ActionReconComponents;
using StandTravelModel.Scripts.Runtime.ActionRecognition.HumanRecon;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.ActionRecognition.Recorder
{
    public class KeyPointsRecorderShower : MonoBehaviour
    {
        [SerializeField] private int index;
        [SerializeField] private KeyPointsRecorder keyPointsRecorder;
        [SerializeField] private KeyPointsRecorderShowerElement[] showerElements;
        [SerializeField] private KeyPointsRecorderShowerElementWithDirect[] showerElementWithDirects;
        
        private void OnValidate() {
            var keyPoints = keyPointsRecorder.GetRecordKeyPoints(index);
            ProcessPoints(keyPoints, null, null);
        }

        public void Simulate()
        {
            var recon = new ActionReconInstanceHuman(
                actionId => {
                    Debug.Log("recon -> " + actionId);
                },
                true
            );

            FilterRecoder(
                keyPoints => {
                    recon.OnUpdate(keyPoints);
                }
            );
        }

        public void WalkThroughRecorder()
        {
            FilterRecoder(
                keyPoints => {
                    ProcessPoints(
                        keyPoints,
                        angle => {
                        },
                        angle => {
                            if(angle < 145)
                            {
                                Debug.Log("!!! " + angle);
                            }
                        }
                    );
                }
            );
        }

        private void FilterRecoder(Action<List<Vector3>> handle)
        {
            var frameCount = keyPointsRecorder.GetFrameCount();
            for(int i = 0; i < frameCount; i++)
            {
                handle(keyPointsRecorder.GetRecordKeyPoints(i));
            }
        }

        private void ProcessPoints(List<Vector3> keyPoints, Action<float> processAngle, Action<float> processAngleWithDirect)
        {
            if(keyPoints != null)
            {
                foreach(var element in showerElements)
                {
                    var angleGetter = new ReconCompAngleGetter(element.keyPointsTypeFor, element.keyPointsTypeMid, element.keyPointsTypeBak);
                    element.angle = angleGetter.GetAngle(keyPoints);
                    
                    if(processAngle != null)
                    {
                        processAngle(element.angle);
                    }
                }

                foreach(var element in showerElementWithDirects)
                {
                    var angleGetter = new ReconCompAngleGetterWithDirect(element.keyPointsTypeFor, element.keyPointsTypeMid, GameKeyPointsType.Nose, element.direct);
                    element.angle = angleGetter.GetAngle(keyPoints);

                    if(processAngleWithDirect != null)
                    {
                        processAngleWithDirect(element.angle);
                    }
                }
            }
        }
    }
}
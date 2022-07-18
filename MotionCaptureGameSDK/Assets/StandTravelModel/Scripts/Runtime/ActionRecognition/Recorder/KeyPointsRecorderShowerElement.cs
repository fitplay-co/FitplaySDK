using System;
using MotionCaptureBasic.Interface;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.ActionRecognition.Recorder
{
    [Serializable]
    public class KeyPointsRecorderShowerElement
    {
        public float angle;
        public GameKeyPointsType keyPointsTypeFor;
        public GameKeyPointsType keyPointsTypeMid;
        public GameKeyPointsType keyPointsTypeBak;
    }

    [Serializable]
    public class KeyPointsRecorderShowerElementWithDirect
    {
        public float angle;
        public GameKeyPointsType keyPointsTypeFor;
        public GameKeyPointsType keyPointsTypeMid;
        public Vector3 direct;
    }
}
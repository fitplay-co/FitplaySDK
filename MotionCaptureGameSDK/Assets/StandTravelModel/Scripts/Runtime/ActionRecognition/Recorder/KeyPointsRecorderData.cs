using System;
using System.Collections.Generic;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.ActionRecognition.Recorder
{
    [Serializable]
    public class PointsContainer
    {
        public List<Points> points = new List<Points>();
    }

    [Serializable]
    public class Points
    {
        public List<Vector3> pointList = new List<Vector3>();
    }

    [Serializable]
    public class Walk
    {

    }
}
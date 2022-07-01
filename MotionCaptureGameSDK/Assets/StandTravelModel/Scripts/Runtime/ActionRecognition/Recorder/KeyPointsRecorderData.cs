using System;
using System.Collections.Generic;
using UnityEngine;

namespace Recorder
{
    [Serializable]
    public class PointsContainer
    {
        public List<Points> points;
    }

    [Serializable]
    public class Points
    {
        public List<Vector3> pointList = new List<Vector3>();
    }
}
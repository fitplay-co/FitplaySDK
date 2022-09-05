using System.Collections.Generic;
using MotionCaptureBasic.OSConnector;
using UnityEngine;

namespace MotionLib.Scripts
{
    public abstract class MotionLibBase: MonoBehaviour
    {
        [HideInInspector]
        public bool isMotioned = false;
        [HideInInspector]
        public bool isRunning = false;
        [HideInInspector]
        public bool isDebug;
        [HideInInspector]
        public MotionLibController.MotionMode motionMode =  MotionLibController.MotionMode.None;
        public abstract void CheckMotion(List<Vector3> keyPointList);
        public abstract void Enabled(bool isEnabled);
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.ActionRecognition.ActionReconComponents
{
    public class ActionReconCompAngle : IActionReconComp
    {
        private bool isDebug;
        private float angleMin;
        private float angleMax;
        private float curAngle;
        private float lastAngle;
        private Action<bool> onAction;
        private ReconCompAngleGetter angleGetter;

        public ActionReconCompAngle(float angleMin, float angleMax, ReconCompAngleGetter angleGetter)
        {
            this.angleMin = angleMin;
            this.angleMax = angleMax;
            this.angleGetter = angleGetter;
        }

        public void OnUpdate(List<Vector3> keyPoints)
        {
            this.lastAngle = this.curAngle;

            this.curAngle = angleGetter.GetAngle(keyPoints);
            var isInAngle = WeatherInAngle(curAngle);

            if(isInAngle != WasInAgnle())
            {
                OnStateFlip(isInAngle);
            }
        }

        public void SetDebug(bool isDebug)
        {
            this.isDebug = isDebug;
        }

        public void SetAction(Action<bool> onAction)
        {
            this.onAction = onAction;
        }

        public float GetCurAngle()
        {
            return curAngle;
        }

        protected virtual void OnStateFlip(bool isInAngle)
        {
            if(onAction != null)
            {
                onAction(isInAngle);
            }
        }

        protected bool IsExpanding()
        {
            return curAngle > lastAngle;
        }

        private bool WasInAgnle()
        {
            return WeatherInAngle(lastAngle);
        }

        private bool WeatherInAngle(float angle)
        {
            return angle <= angleMax && angle >= angleMin;
        }
    }
}
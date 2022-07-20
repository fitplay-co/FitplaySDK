using MotionCaptureBasic.OSConnector;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.Core.AnimationStates.Components
{
    public class RunConditioner
    {
        private float lastRun;

        public bool IsEnterRunReady(WalkActionItem walkData)
        {
            //Debug.Log(walkData.leftFrequency + "|" + walkData.rightFrequency);
            var isRun = (walkData.leftFrequency * walkData.leftStepLength > 5) || (walkData.rightFrequency * walkData.rightStepLength > 5);
            /* if(!isRun)
            {
                if(Time.time < lastRun + 1)
                {
                    return true;
                }
                return false;
            }
            else
            {
                lastRun = Time.time;
                return true;
            } */
            return isRun;

            //return walkData.leftFrequency > 2 || walkData.rightFrequency > 2;
            /* if(Input.GetKey("joystick button 1"))
            {
                return true;
            }
            return false; */
        }
    }
}
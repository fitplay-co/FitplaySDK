using MotionCaptureBasic.OSConnector;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.Core.AnimationStates.Components
{
    public class RunConditioner
    {
        private float runThrehold;

        public RunConditioner(float runThrehold)
        {
            this.runThrehold = runThrehold;
        }

        public bool IsEnterRunReady(WalkActionItem walkData)
        {
            //Debug.Log(walkData.leftFrequency + "|" + walkData.rightFrequency);
            var isRun = (walkData.leftFrequency * walkData.leftStepLength > runThrehold) || (walkData.rightFrequency * walkData.rightStepLength > runThrehold);
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
using MotionCaptureBasic.OSConnector;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.Core.AnimationStates.Components
{
    public class RunConditioner
    {
        public bool IsEnterRunReady(WalkActionItem walkData)
        {
            //Debug.Log(walkData.leftFrequency + "|" + walkData.rightFrequency);
            return (walkData.leftFrequency * walkData.leftStepLength > 5) || (walkData.rightFrequency * walkData.rightStepLength > 5);
            //return walkData.leftFrequency > 2 || walkData.rightFrequency > 2;
            /* if(Input.GetKey("joystick button 1"))
            {
                return true;
            }
            return false; */
        }
    }
}
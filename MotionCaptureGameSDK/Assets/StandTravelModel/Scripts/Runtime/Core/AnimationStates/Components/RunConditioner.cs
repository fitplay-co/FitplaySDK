using System;
using MotionCaptureBasic.OSConnector;

namespace StandTravelModel.Scripts.Runtime.Core.AnimationStates.Components
{
    public class RunConditioner
    {
        private Func<float> getRunThrehold;
        private StepStrideCacher strideCacher;

        public RunConditioner(Func<float> getRunThrehold, StepStrideCacher strideCacher)
        {
            this.strideCacher = strideCacher;
            this.getRunThrehold = getRunThrehold;
        }

        public bool IsEnterRunReady(WalkActionItem walkData, bool debug)
        {
            strideCacher.OnUpdate(walkData.leftLeg, walkData.leftStepLength);

            //Debug.Log(walkData.leftFrequency + "|" + walkData.rightFrequency);
            //UnityEngine.Debug.Log("velocity -> " + walkData.velocity);
            return walkData.velocity > getRunThrehold();
          
            //return walkData.leftFrequency * strideCacher.GetStrideSmooth() > getRunThrehold();

            /* if(debug)
            {
                if(!isRun)
                {
                    Debug.Log(walkData.leftFrequency + "|" + walkData.rightFrequency);
                }
            } */

            
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

            //return walkData.leftFrequency > 2 || walkData.rightFrequency > 2;
            /* if(Input.GetKey("joystick button 1"))
            {
                return true;
            }
            return false; */
        }
    }
}
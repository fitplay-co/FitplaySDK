using MotionCaptureBasic.OSConnector;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.Core.AnimationStates.Components
{
    public class RunConditioner
    {
        private float runThrehold;
        private StepStrideCacher strideCacher;

        public RunConditioner(float runThrehold)
        {
            this.runThrehold = runThrehold;
            this.strideCacher = new StepStrideCacher();
        }

        public bool IsEnterRunReady(WalkActionItem walkData, bool debug)
        {
            strideCacher.OnUpdate(walkData.leftLeg, walkData.leftStepLength);

            //Debug.Log(walkData.leftFrequency + "|" + walkData.rightFrequency);
          
            var isRun = walkData.leftFrequency > 2f && walkData.leftFrequency * strideCacher.GetStride() > runThrehold;

            /* if(debug)
            {
                if(!isRun)
                {
                    Debug.Log(walkData.leftFrequency + "|" + walkData.rightFrequency);
                }
            } */

            return isRun;

            
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
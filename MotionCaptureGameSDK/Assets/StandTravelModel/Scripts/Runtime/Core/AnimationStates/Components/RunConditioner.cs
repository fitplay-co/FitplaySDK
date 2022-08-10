using System;
using MotionCaptureBasic.OSConnector;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.Core.AnimationStates.Components
{
    public class RunConditioner
    {
        private class RunCacher
        {
            private bool isLeft;
            private bool isRunning;
            private int lastLeg;
            private float lastLegChange;
            private Func<float> getRunThrehold;

            public RunCacher(bool isLeft, Func<float> getRunThrehold)
            {
                this.isLeft = isLeft;
                this.getRunThrehold = getRunThrehold;
            }

            public bool IsEnterRunReady(WalkActionItem walkData, bool debug)
            {
                /* if(walkData.leftFrequency > getRunThrehold() || walkData.rightFrequency > getRunThrehold())
                {
                    Debug.Log(walkData.leftFrequency + "|" + walkData.rightFrequency + "|||");
                    Debug.Break();
                } */
                return walkData.leftFrequency > getRunThrehold() || walkData.rightFrequency > getRunThrehold();
                /* var curLeft = isLeft ? walkData.realtimeLeftLeg : walkData.realtimeRightLeg;

                if(lastLeg != 0 && lastLeg != curLeft)
                {
                    if(lastLegChange != 0)
                    {
                        if(isRunning)
                        {
                            isRunning = Time.time < lastLegChange + getRunThrehold() * 1.1f;
                        }
                        else
                        {
                            isRunning = Time.time < lastLegChange + getRunThrehold();
                        }
                    }

                    lastLegChange = Time.time;
                }
                lastLeg = curLeft;
                return isRunning; */
            }
        }

        private bool isRunning;
        private RunCacher cacherLeft;
        private RunCacher cacherRight;
        private StepStrideCacher strideCacher;

        public RunConditioner(Func<float> getRunThrehold, StepStrideCacher strideCacher)
        {
            this.cacherLeft = new RunCacher(true, getRunThrehold);
            this.cacherRight = new RunCacher(false, getRunThrehold);
            this.strideCacher = strideCacher;
        }

        public bool IsEnterRunReady(WalkActionItem walkData, bool debug)
        {
            strideCacher.OnUpdate(walkData.realtimeLeftLeg, walkData.leftStepLength);

            var isRunningLeft = cacherLeft.IsEnterRunReady(walkData, debug);
            var isRunningRight = cacherRight.IsEnterRunReady(walkData, debug);

            /* if(!isRunning)
            {
                isRunning = isRunningLeft || isRunningRight;
            }
            else
            {
                isRunning = isRunningLeft && isRunningRight;
            } */
            isRunning = isRunningLeft || isRunningRight;

            return isRunning;

            //Debug.Log(walkData.leftFrequency + "|" + walkData.rightFrequency);
            //Debug.Log("velocity -> " + walkData.velocity);
            //Debug.Log($"stepRate -> {walkData.stepRate}");
            //Debug.Log($"stepLen -> {walkData.stepLen}");

            /* var speed = walkData.stepRate * walkData.stepLen;
            velocity = Mathf.Lerp(velocity, speed, Time.deltaTime * 5);
            return velocity > getRunThrehold(); */
            
            //return walkData.leftFrequency > getRunThrehold();
          
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
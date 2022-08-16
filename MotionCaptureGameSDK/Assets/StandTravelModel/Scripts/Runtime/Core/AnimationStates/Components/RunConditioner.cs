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
            private float actingTime;
            private float lastLegChange;
            private Func<bool> useFrequencey;
            private Func<float> getRunThrehold;

            public RunCacher(bool isLeft, Func<float> getRunThrehold, Func<bool> useFrequencey)
            {
                this.isLeft = isLeft;
                this.useFrequencey = useFrequencey;
                this.getRunThrehold = getRunThrehold;
            }

            public bool IsEnterRunReady(WalkActionItem walkData, bool debug)
            {
                if(useFrequencey())
                {
                    return IsEnterRunReadyByFrequency(walkData, debug);
                }
                else
                {
                    return IsEnterRunReadyBySpeed(walkData, debug);
                }
            }

            public float GetActTime()
            {
                return actingTime;
            }

            private bool IsEnterRunReadyBySpeed(WalkActionItem walkData, bool debug)
            {
                var frequency = isLeft ? walkData.leftFrequency : walkData.rightFrequency;
                var stepLength = isLeft ? walkData.leftStepLength : walkData.rightStepLength;
                return frequency * stepLength > getRunThrehold();
            }

            private bool IsEnterRunReadyByFrequency(WalkActionItem walkData, bool debug)
            {
                var curLeft = isLeft ? walkData.GetLeftLeg() : walkData.GetRightLeg();
                var isActing = lastLeg != 0 && lastLeg != curLeft;
                if(isActing)
                {
                    if(lastLegChange != 0)
                    {
                        actingTime = Time.time - lastLegChange;
                        if(isRunning)
                        {
                            isRunning = actingTime < getRunThrehold() * 1.1f;
                        }
                        else
                        {
                            isRunning = actingTime < getRunThrehold();
                        }
                    }

                    lastLegChange = Time.time;
                }
                lastLeg = curLeft;
                return isRunning;
            }
        }

        private bool isRunning;
        private RunCacher cacherLeft;
        private RunCacher cacherRight;
        private StepStrideCacher strideCacher;

        public RunConditioner(Func<float> getRunThrehold, Func<bool> useFrequencey, StepStrideCacher strideCacher)
        {
            this.cacherLeft = new RunCacher(true, getRunThrehold, useFrequencey);
            this.cacherRight = new RunCacher(false, getRunThrehold, useFrequencey);
            this.strideCacher = strideCacher;
        }

        public float GetActTimeLeft()
        {
            return cacherLeft.GetActTime();
        }

        public float GetActTimeRight()
        {
            return cacherRight.GetActTime();
        }

        public bool IsEnterRunReady(WalkActionItem walkData, bool debug)
        {
            strideCacher.OnUpdate(walkData.GetLeftLeg(), walkData.leftStepLength);

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
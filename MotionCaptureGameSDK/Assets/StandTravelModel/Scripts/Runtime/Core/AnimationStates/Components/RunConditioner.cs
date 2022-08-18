using System;
using MotionCaptureBasic.OSConnector;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.Core.AnimationStates.Components
{
    public class RunConditioner
    {
        private class RunCacher
        {
            private int lastLeg;
            private bool isLeft;
            private bool isRunning;
            private float actingTime;
            private float lastLegChange;
            private Func<bool> useFrequencey;
            private Func<float> getThreholdRun;
            private Func<float> getThreholdSprint;

            public RunCacher(bool isLeft, Func<float> getThreholdRun, Func<bool> useFrequencey, Func<float> getThreholdSprint)
            {
                this.isLeft = isLeft;
                this.useFrequencey = useFrequencey;
                this.getThreholdRun = getThreholdRun;
                this.getThreholdSprint = getThreholdSprint;
            }

            public bool IsEnterRunReady(WalkActionItem walkData, bool debug)
            {
                if(useFrequencey())
                {
                    return IsExceededThresholdFrequency(walkData, getThreholdRun());
                }
                else
                {
                    return IsExceededThresholdSpeed(walkData, walkData.velocityThreshold);
                }
            }

            public bool IsEnterSprintReady(WalkActionItem walkData)
            {
                if(useFrequencey())
                {
                    return IsExceededThresholdFrequency(walkData, getThreholdSprint());
                }
                else
                {
                    return IsExceededThresholdSpeed(walkData, getThreholdSprint());
                }
            }

            public float GetActTime()
            {
                return actingTime;
            }

            private bool IsExceededThresholdSpeed(WalkActionItem walkData, float threhold)
            {
                //var frequency = isLeft ? walkData.leftFrequency : walkData.rightFrequency;
                //var stepLength = isLeft ? walkData.leftStepLength : walkData.rightStepLength;
                return walkData.velocity >= threhold;
            }

            private bool IsExceededThresholdFrequency(WalkActionItem walkData, float threhold)
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
                            isRunning = actingTime < threhold * 1.1f;
                        }
                        else
                        {
                            isRunning = actingTime < threhold;
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

        public RunConditioner(Func<float> getThreholdRun, Func<float> getThreholdSprint, Func<bool> useFrequencey, StepStrideCacher strideCacher)
        {
            this.cacherLeft = new RunCacher(true, getThreholdRun, useFrequencey, getThreholdSprint);
            this.cacherRight = new RunCacher(false, getThreholdRun, useFrequencey, getThreholdSprint);
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

        public bool IsEnterSprintReady(WalkActionItem walkData)
        {
            return cacherLeft.IsEnterSprintReady(walkData) || cacherRight.IsEnterSprintReady(walkData);
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
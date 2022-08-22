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
            private bool isSprinting;
            private float actingTime;
            private float lastLegChange;
            private Func<bool> useFrequencey;
            private Func<float> getThreholdRun;
            private Func<float> getThreholdRunLow;
            private Func<float> getThreholdSprint;
            private Func<float> getRunThresholdScale;
            private Func<float> getRunThresholdScaleLow;

            public RunCacher(bool isLeft, Func<float> getThreholdRun, Func<float> getThreholdRunLow, Func<bool> useFrequencey, Func<float> getThreholdSprint, Func<float> getRunThresholdScale, Func<float> getRunThresholdScaleLow)
            {
                this.isLeft = isLeft;
                this.useFrequencey = useFrequencey;
                this.getThreholdRun = getThreholdRun;
                this.getThreholdRunLow = getThreholdRunLow;
                this.getThreholdSprint = getThreholdSprint;
                this.getRunThresholdScale = getRunThresholdScale;
                this.getRunThresholdScaleLow = getRunThresholdScaleLow;
            }

            public bool IsEnterRunReady(WalkActionItem walkData, bool debug)
            {
                if(useFrequencey())
                {
                    return IsExceededThresholdFrequency(ref isRunning, getThreholdRun(), getThreholdRunLow());
                }
                else
                {
                    return IsExceededThresholdSpeed(ref isRunning, walkData, walkData.velocityThreshold * getRunThresholdScale(), walkData.velocityThreshold * getRunThresholdScaleLow());
                }
            }

            public bool IsEnterSprintReady(WalkActionItem walkData)
            {
                if(useFrequencey())
                {
                    return IsExceededThresholdFrequency(ref isSprinting, getThreholdSprint(), getThreholdSprint());
                }
                else
                {
                    return IsExceededThresholdSpeed(ref isRunning, walkData, getThreholdSprint(), walkData.velocityThreshold * getRunThresholdScaleLow());
                }
            }

            public float GetActTime()
            {
                return actingTime;
            }

            public void UpdateFrequency(WalkActionItem walkData)
            {
                var curLeft = isLeft ? walkData.GetLeftLeg() : walkData.GetRightLeg();
                var isActing = lastLeg != 0 && lastLeg != curLeft;
                if(isActing)
                {
                    if(lastLegChange != 0)
                    {
                        actingTime = Time.time - lastLegChange;
                    }

                    /* if(curState)
                    {
                        Debug.Log(lastLeg + "|" + curLeft + "|" + actingTime + "|" + threhold);
                    } */

                    lastLegChange = Time.time;
                }
                lastLeg = curLeft;
            }

            private bool IsExceededThresholdSpeed(ref bool curState, WalkActionItem walkData, float threhold, float threholdLow)
            {
                //var frequency = isLeft ? walkData.leftFrequency : walkData.rightFrequency;
                //var stepLength = isLeft ? walkData.leftStepLength : walkData.rightStepLength;
                if(!curState)
                {
                    curState = walkData.velocity >= threhold;
                }
                else
                {
                    curState = walkData.velocity >= threholdLow;
                }
                return curState;
            }

            private bool IsExceededThresholdFrequency(ref bool curState, float threhold, float threholdLow)
            {
                if(lastLegChange != 0)
                {
                    if(curState)
                    {
                        curState = actingTime < threhold;
                    }
                    else
                    {
                        curState = actingTime < threholdLow;
                    }
                }

                return curState;
            }
        }

        private bool isRunning;
        private RunCacher cacherLeft;
        private RunCacher cacherRight;
        private StepStrideCacher strideCacher;

        public RunConditioner(Func<float> getThreholdRun, Func<float> getThreholdRunLow, Func<float> getThreholdSprint, Func<bool> useFrequencey, Func<float> getRunThresholdScale, Func<float> getRunThresholdScaleLow, StepStrideCacher strideCacher)
        {
            this.cacherLeft = new RunCacher(true, getThreholdRun, getThreholdRunLow, useFrequencey, getThreholdSprint, getRunThresholdScale, getRunThresholdScaleLow);
            this.cacherRight = new RunCacher(false, getThreholdRun, getThreholdRunLow, useFrequencey, getThreholdSprint, getRunThresholdScale, getRunThresholdScaleLow);
            this.strideCacher = strideCacher;
        }

        public void OnUpdate(WalkActionItem walkData)
        {
            cacherLeft.UpdateFrequency(walkData);
            cacherRight.UpdateFrequency(walkData);
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
            OnUpdate(walkData);
            return cacherLeft.IsEnterSprintReady(walkData) || cacherRight.IsEnterSprintReady(walkData);
        }

        public bool IsEnterRunReady(WalkActionItem walkData, bool debug)
        {
            OnUpdate(walkData);
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
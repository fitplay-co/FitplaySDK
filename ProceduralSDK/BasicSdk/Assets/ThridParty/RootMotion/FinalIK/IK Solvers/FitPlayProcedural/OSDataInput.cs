using System;
using System.Collections.Generic;
using SEngineBasic;
using UnityWebSocket;

namespace RootMotion.FinalIK.FitPlayProcedural
{
    public enum CurrentFoot
    {
        left ,
        right ,
        same
    }
    public class OSDataInput
    {
        private KeyPointItem leftfootPoint, rightfootPoint, leftKneePoint, rightKneePoint;
        private bool isStanding;
        public float currentDeltaHeight = 0f;
        
        public CurrentFoot currentFoot = CurrentFoot.same;
        private SEngineBasic.WebsocketOSClient os;
        
        private int standStillTimeCounter = 0;

        public OSDataInput()
        {                
            os = new WebsocketOSClient();
            os.ConnectAsync(state =>
            {
                if (state == WebSocketState.Open)
                {
                    os
                        .SubscribeApplicationClient()
                        .SubscribeGroundLocation(EOSActionType.Subscribe)
                        .SubscribeFitting(EOSActionType.Subscribe, EFittingType.Dual)
                        .SubscribeActionDetection(EOSActionType.Subscribe)
                        .SubscribeGazeTracking(EOSActionType.Subscribe)
                        .SetImuFPS();
                       // .HeartCommand(EHandleType.LeftHandle,EHeartCommandType.OpenHeartRate)
                       // .HeartCommand(EHandleType.RightHandle, EHeartCommandType.OpenBloodOxygen);
                }
            });
            
        }
        private void GetCurrentMovingFoot(KeyPointItem rightOsTrigger, KeyPointItem leftOsTrigger)
        {
            if (isStanding)
            {
                currentFoot = CurrentFoot.same;
                return;
            } 
            currentFoot = rightOsTrigger.y > leftOsTrigger.y ? CurrentFoot.right : CurrentFoot.left;

            /*//切换脚的时候把上只脚的数据记录。
            if (calcfoot != CurrentMovingFoot)
            {
                LastStepToOffset = CurrentPridictedStep;
                LerpProgress = 0f;
                LastFootPredictedHeight = MaxOSHeight;
                MaxOSHeight = 0;
            }*/

        }

        private float GetFrameHeight(KeyPointItem rightOsTrigger, KeyPointItem leftOsTrigger)
        {
            if (currentFoot == CurrentFoot.same)
                return 0f;
            return Math.Abs(rightOsTrigger.y - leftOsTrigger.y);
        }

        private bool UnderDistanceThreshold(KeyPointItem rightOsTrigger, KeyPointItem leftOsTrigger,float threshold=0.1f) //OS触发
        {

            return Math.Abs(rightOsTrigger.y - leftOsTrigger.y) < threshold;
        }
        public void OSUpdate()
        {

            if (!(os?.IMessage is IKBodyMessage message))
            {
                return;
            }

            List<KeyPointItem> landMarkKeyPointList = message.pose_landmark.keypoints3D;
            leftfootPoint = landMarkKeyPointList[28];
            rightfootPoint = landMarkKeyPointList[27];
            leftKneePoint = landMarkKeyPointList[26];
            rightKneePoint = landMarkKeyPointList[25];
            isStanding = UnderDistanceThreshold(rightKneePoint,leftKneePoint);
            GetCurrentMovingFoot(rightKneePoint,leftKneePoint);
            currentDeltaHeight = GetFrameHeight(rightKneePoint, leftKneePoint);
            UnityEngine.Debug.Log("standing------------------"  + isStanding);

            

        }


    }
}
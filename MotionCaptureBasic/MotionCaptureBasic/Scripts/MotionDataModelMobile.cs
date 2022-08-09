using System;
using System.Collections.Generic;
using MotionCaptureBasic.Interface;
using MotionCaptureBasic.OSConnector;
using UnityEngine;

namespace MotionCaptureBasic
{
    public class MotionDataModelMobile : IMotionDataModel
    {
        private static MotionDataModelMobile instance;

        private static readonly object _Synchronized = new object();

        private MobileOSHandler mobileOSHandler;
        private MotionDataPreprocessor motionDataPreprocessor;

        private MotionDataModelMobile()
        {
            mobileOSHandler = MobileOSHandler.GetInstance();
            motionDataPreprocessor = new MotionDataPreprocessor();
        }
        
        public static MotionDataModelMobile GetInstance()
        {
            if (instance == null)
            {
                lock(_Synchronized)
                {
                    if(instance == null)
                    {
                        instance = new MotionDataModelMobile();
                    }
                }
            }

            return instance;
        }

        public EventImuHandler OnImuReceived { get; }
        
        public List<Vector3> GetIKPointsData(bool isLocalCoordinates, bool isPreprocessed)
        {
            var bodymessage = mobileOSHandler.BodyMessageBase;

            if (!(bodymessage is IKBodyUpdateMessage ikBodyUpdateMessage))
            {
                return null;
            }

            if (ikBodyUpdateMessage.pose_landmark == null)
            {
                return null;
            }

            List<KeyPointItem> filteredKeyPoints = null;

            if (isLocalCoordinates)
            {
                if (ikBodyUpdateMessage.pose_landmark.keypoints3D != null)
                {
                    filteredKeyPoints = motionDataPreprocessor.FilteringKeyPoints(ikBodyUpdateMessage.pose_landmark.keypoints3D);
                }
            }
            else
            {
                if (ikBodyUpdateMessage.pose_landmark.keypoints != null)
                {
                    filteredKeyPoints = motionDataPreprocessor.FilteringKeyPoints(ikBodyUpdateMessage.pose_landmark.keypoints);
                }
            }

            if (filteredKeyPoints == null)
            {
                return null;
            }
            
            List<Vector3> ikPointsDataList = new List<Vector3>((int)GameKeyPointsType.Count);
            for (int i = 0; i < (int) GameKeyPointsType.Count; i++)
            {
                ikPointsDataList.Add(Vector3.zero);
            }

            int length = (int) GameKeyPointsType.Count;

            for (int i = 0; i < length; i++)
            {
                var ikPoint = ikPointsDataList[i];
                ikPoint.x = -filteredKeyPoints[i].x;
                ikPoint.y = -filteredKeyPoints[i].y;
                ikPoint.z = -filteredKeyPoints[i].z;
                ikPointsDataList[i] = ikPoint;
            }

            if (isPreprocessed)
            {
                ikPointsDataList = motionDataPreprocessor.GetKeyPointsCorrection(ikPointsDataList);
            }

            return ikPointsDataList;
        }

        public GroundLocationItem GetGroundLocationData()
        {
            var bodymessage = mobileOSHandler.BodyMessageBase;

            if (!(bodymessage is IKBodyUpdateMessage ikBodyUpdateMessage))
            {
                return null;
            }

            return ikBodyUpdateMessage.ground_location;
        }

        public ActionDetectionItem GetActionDetectionData()
        {
            var bodymessage = mobileOSHandler.BodyMessageBase;

            if (!(bodymessage is IKBodyUpdateMessage ikBodyUpdateMessage))
            {
                return null;
            }

            return ikBodyUpdateMessage.action_detection;
        }

        public MonitorItem GetMonitorData()
        {
            var bodymessage = mobileOSHandler.BodyMessageBase;

            if (!(bodymessage is IKBodyUpdateMessage ikBodyUpdateMessage))
            {
                return null;
            }

            return ikBodyUpdateMessage.monitor;
        }

        public TimeProfiling GetTimeProfilingData()
        {
            var bodymessage = mobileOSHandler.BodyMessageBase;

            if (!(bodymessage is IKBodyUpdateMessage ikBodyUpdateMessage))
            {
                return null;
            }

            return ikBodyUpdateMessage.timeProfiling;
        }

        public GazeTracking GetGazeTrackingData()
        {
            var bodymessage = mobileOSHandler.BodyMessageBase;

            if (!(bodymessage is IKBodyUpdateMessage ikBodyUpdateMessage))
            {
                return null;
            }

            return ikBodyUpdateMessage.gaze_tracking;
        }

        public bool SubscribeGazeTracking()
        {
            throw new NotImplementedException();
        }

        public bool SubscribeActionDetection()
        {
            throw new NotImplementedException();
        }

        public bool SubscribeGroundLocation()
        {
            throw new NotImplementedException();
        }

        public bool SubscribeHandPoseture()
        {
            throw new NotImplementedException();
        }

        public bool SubscribeFitting()
        {
            throw new NotImplementedException();
        }

        public void SetPreprocessorParameters(Vector3 motionScaling)
        {
            motionDataPreprocessor.ChangeScaleFactor(motionScaling);
        }

        public Fitting GetFitting()
        {
            return mobileOSHandler.BodyMessageBase?.fitting;
        }

        public void AddConnectEvent(Action onConnect)
        {
            throw new NotImplementedException();
        }

        public bool ReleaseGazeTracking()
        {
            throw new NotImplementedException();
        }

        public bool ReleaseActionDetection()
        {
            throw new NotImplementedException();
        }

        public bool ReleaseGroundLocation()
        {
            throw new NotImplementedException();
        }

        public bool ReleaseFitting()
        {
            throw new NotImplementedException();
        }

        public void SetDebug(bool isDebug)
        {
            mobileOSHandler.SetDebug(isDebug);
        }

        public bool ResetGroundLocation()
        {
            throw new NotImplementedException();
        }

        public void SetIKDataListSimulat(List<Vector3> ikPointsDataListSimulat)
        {
            throw new NotImplementedException();
        }

        public void ClearIKDataListSimulat()
        {
            throw new NotImplementedException();
        }
    }
}
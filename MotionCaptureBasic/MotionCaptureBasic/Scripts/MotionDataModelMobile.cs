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
        
        private List<Vector3> ikPointsDataListSimulat;

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
            if (ikPointsDataListSimulat != null)
            {
                return ikPointsDataListSimulat;
            }
            
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
            Debug.LogError(" is not implemented for mobile instance");
            return false;
        }

        public bool SubscribeActionDetection()
        {
            Debug.LogError(" is not implemented for mobile instance");
            return false;
        }

        public bool SubscribeGroundLocation()
        {
            Debug.LogError(" is not implemented for mobile instance");
            return false;
        }

        public bool SubscribeHandPoseture()
        {
            Debug.LogError(" is not implemented for mobile instance");
            return false;
        }

        public bool SubscribeFitting()
        {
            Debug.LogError(" is not implemented for mobile instance");
            return false;
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
            Debug.LogError(" is not implemented for mobile instance");
        }

        public bool ReleaseGazeTracking()
        {
            Debug.LogError(" is not implemented for mobile instance");
            return false;
        }

        public bool ReleaseActionDetection()
        {
            Debug.LogError(" is not implemented for mobile instance");
            return false;
        }

        public bool ReleaseGroundLocation()
        {
            Debug.LogError(" is not implemented for mobile instance");
            return false;
        }

        public bool ReleaseFitting()
        {
            Debug.LogError(" is not implemented for mobile instance");
            return false;
        }

        public void SetDebug(bool isDebug)
        {
            mobileOSHandler.SetDebug(isDebug);
        }

        public bool ResetGroundLocation()
        {
            Debug.LogError(" is not implemented for mobile instance");
            return false;
        }

        public void SetIKDataListSimulat(List<Vector3> ikPointsDataListSimulat)
        {
            this.ikPointsDataListSimulat = ikPointsDataListSimulat;
        }

        public void ClearIKDataListSimulat()
        {
            this.ikPointsDataListSimulat = null;
        }
    }
}
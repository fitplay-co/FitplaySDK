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
        
        public HttpProtocolHandler GetHttpProtocol()
        {
            return null;
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
        
        public GeneralDetectionItem GetGeneralDetectionData()
        {
            var bodymessage = mobileOSHandler.BodyMessageBase;

            if (!(bodymessage is IKBodyUpdateMessage ikBodyUpdateMessage))
            {
                return null;
            }

            return ikBodyUpdateMessage.general_detection;
        }
        
        public StandDetection GetStandDetectionData()
        {
            var bodymessage = mobileOSHandler.BodyMessageBase;

            if (!(bodymessage is IKBodyUpdateMessage ikBodyUpdateMessage))
            {
                return null;
            }

            return ikBodyUpdateMessage.stand_detection;
        }

        public bool SubscribeGazeTracking()
        {
            Debug.LogError("SubscribeGazeTracking is not implemented for mobile instance");
            return false;
        }

        public bool SubscribeActionDetection()
        {
            Debug.LogError("SubscribeActionDetection is not implemented for mobile instance");
            return false;
        }

        public bool SubscribeGroundLocation()
        {
            Debug.LogError("SubscribeGroundLocation is not implemented for mobile instance");
            return false;
        }

        public bool SubscribeHandPoseture()
        {
            Debug.LogError("SubscribeHandPoseture is not implemented for mobile instance");
            return false;
        }

        public bool SubscribeFitting()
        {
            Debug.LogError("SubscribeFitting is not implemented for mobile instance");
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

        public void AddConnectEvent(Action onConnect, Action onClosed = null, Action onError = null)
        {
            Debug.LogError("AddConnectEvent is not implemented for mobile instance");
        }

        public void ReleaseConnectEvent()
        {
            Debug.LogError("ReleaseConnectEvent is not implemented for mobile instance");
        }

        public bool ReleaseGazeTracking()
        {
            Debug.LogError("ReleaseGazeTracking is not implemented for mobile instance");
            return false;
        }

        public bool ReleaseActionDetection()
        {
            Debug.LogError("ReleaseActionDetection is not implemented for mobile instance");
            return false;
        }

        public bool ReleaseGroundLocation()
        {
            Debug.LogError("ReleaseGroundLocation is not implemented for mobile instance");
            return false;
        }

        public bool ReleaseFitting()
        {
            Debug.LogError("ReleaseFitting is not implemented for mobile instance");
            return false;
        }

        public void SetDebug(bool isDebug)
        {
            mobileOSHandler.SetDebug(isDebug);
        }

        public bool ResetGroundLocation()
        {
            Debug.LogError("ResetGroundLocation is not implemented for mobile instance");
            return false;
        }

        public bool SetPlayerHeight(int h)
        {
            Debug.LogError("SetPlayerHeight is not implemented for mobile instance");
            return false;
        }

        public bool SubscribeGeneral()
        {
            Debug.LogError("SubscribeGeneral is not implemented for mobile instance");
            return false;
        }

        public bool ReleaseGeneral()
        {
            Debug.LogError("ReleaseGeneral is not implemented for mobile instance");
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

        public void SetSimulatActionDetectionData(ActionDetectionItem simulatActionDetectionItem)
        {
            Debug.LogError("SetSimulatActionDetectionData is not implemented for mobile instance");
        }

        public void ClearSimulatActionDetectionData()
        {
            Debug.LogError("ClearSimulatActionDetectionData is not implemented for mobile instance");
        }

        public void SetSimulateFittingData(Fitting simulateFittingData)
        {
            Debug.LogError("SetSimulateFittingData is not implemented for mobile instance");
        }

        public void ClearSimulateFittingData()
        {
            Debug.LogError("ClearSimulateFittingData is not implemented for mobile instance");
        }
        
        public MotionDataModelType GetMotionDataModelType()
        {
            return MotionDataModelType.Mobile;
        }
    }
}
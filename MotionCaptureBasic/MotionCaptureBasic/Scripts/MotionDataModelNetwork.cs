using System;
using System.Collections.Generic;
using MotionCaptureBasic.Interface;
using MotionCaptureBasic.OSConnector;
using UnityEngine;

namespace MotionCaptureBasic
{
    public class MotionDataModelNetwork : IMotionDataModel

    {
        private ActionDetectionItem simulatActionDetectionItem;
        private Fitting simulateFittingData;
        public EventImuHandler OnImuReceived { get; }

        public List<Vector3> GetIKPointsData(bool isLocalCoordinates, bool isPreprocessed)
        {
            return null;
        }

        public GroundLocationItem GetGroundLocationData()
        {
            return null;
        }

        public ActionDetectionItem GetActionDetectionData()
        {
            if (simulatActionDetectionItem != null)
            {
                return simulatActionDetectionItem;
            }

            return null;
        }

        public MonitorItem GetMonitorData()
        {
            return null;
        }

        public TimeProfiling GetTimeProfilingData()
        {
            return null;
        }

        public GazeTracking GetGazeTrackingData()
        {
            return null;
        }

        public GeneralDetectionItem GetGeneralDetectionData()
        {
            return null;
        }

        public StandDetection GetStandDetectionData()
        {
            return null;
        }

        public bool SubscribeGazeTracking()
        {
            return false;
        }

        public bool SubscribeActionDetection()
        {
            return false;
        }

        public bool SubscribeGroundLocation()
        {
            return false;
        }

        public bool SubscribeHandPoseture()
        {
            return false;
        }

        public bool SubscribeFitting()
        {
            return false;
        }

        public void SetPreprocessorParameters(Vector3 motionScaling)
        {
           return;
        }

        public Fitting GetFitting()
        {
            if (simulateFittingData != null)
            {
                return simulateFittingData;
            }

            return null;
        }

        public void AddConnectEvent(Action onConnect)
        {
            return;
        }

        public bool ReleaseGazeTracking()
        {
            return false;
        }

        public bool ReleaseActionDetection()
        {
            return false;
        }

        public bool ReleaseGroundLocation()
        {
            return false;
        }

        public bool ReleaseFitting()
        {
            return false;
        }

        public void SetDebug(bool isDebug)
        {
            return;
        }

        public bool ResetGroundLocation()
        {
            return false;
        }

        public bool SubscribeGeneral()
        {
            return false;
        }

        public bool ReleaseGeneral()
        {
            return false;
        }

        public bool SetPlayerHeight(int h)
        {
            return false;
        }

        public void SetIKDataListSimulat(List<Vector3> ikPointsDataListSimulat)
        {
            return;
        }

        public void ClearIKDataListSimulat()
        {
            return;
        }

        public void SetSimulatActionDetectionData(ActionDetectionItem simulatActionDetectionItem)
        { 
            this.simulatActionDetectionItem = simulatActionDetectionItem;
        }

        public void ClearSimulatActionDetectionData()
        {
            simulatActionDetectionItem = null;
        }

        public void SetSimulateFittingData(Fitting simulateFittingData)
        {
            this.simulateFittingData = simulateFittingData;
        }

        public void ClearSimulateFittingData()
        {
            simulateFittingData = null;
        }
    }
}
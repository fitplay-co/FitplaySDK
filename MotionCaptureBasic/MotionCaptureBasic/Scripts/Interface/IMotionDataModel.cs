using System;
using System.Collections.Generic;
using MotionCaptureBasic.OSConnector;
using UnityEngine;

namespace MotionCaptureBasic.Interface
{
    public enum GameKeyPointsType
    {
        Nose = 0,
        LeftShoulder,
        RightShoulder,
        LeftElbow,
        RightElbow,
        LeftHand,
        RightHand,
        LeftIndex,
        RightIndex,
        LeftHip,
        RightHip,
        LeftKnee,
        RightKnee,
        LeftFoot,
        RightFoot,
        LeftFootIndex,
        RightFootIndex,
        
        Count = RightFootIndex + 1
    }

    public delegate void EventImuHandler(IKBodyUpdateMessage imuData);

    public interface IMotionDataModel
    {
        EventImuHandler OnImuReceived { get; }
        List<Vector3> GetIKPointsData(bool isLocalCoordinates, bool isPreprocessed);
        GroundLocationItem GetGroundLocationData();
        ActionDetectionItem GetActionDetectionData();
        MonitorItem GetMonitorData();
        TimeProfiling GetTimeProfilingData();
        GazeTracking GetGazeTrackingData();
        GeneralDetectionItem GetGeneralDetectionData();
        StandDetection GetStandDetectionData();
        bool SubscribeGazeTracking();
        bool SubscribeActionDetection();
        bool SubscribeGroundLocation();
        bool SubscribeHandPoseture();
        bool SubscribeFitting();
        void SetPreprocessorParameters(Vector3 motionScaling);
        Fitting GetFitting();
        void AddConnectEvent(Action onConnect);
        bool ReleaseGazeTracking();
        bool ReleaseActionDetection();
        bool ReleaseGroundLocation();
        bool ReleaseFitting();
        void SetDebug(bool isDebug);
        bool ResetGroundLocation();
        bool SubscribeGeneral();
        bool ReleaseGeneral();
        bool SetPlayerHeight(int h);
        void SetIKDataListSimulat(List<Vector3> ikPointsDataListSimulat);
        void ClearIKDataListSimulat();
    }
}
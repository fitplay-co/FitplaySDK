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

    public interface IMotionDataModel
    {
        List<Vector3> GetIKPointsData(bool isLocalCoordinates, bool isPreprocessed);
        GroundLocationItem GetGroundLocationData();
        ActionDetectionItem GetActionDetectionData();
        MonitorItem GetMonitorData();
        TimeProfiling GetTimeProfilingData();
        GazeTracking GetGazeTrackingData();
        bool SubscribeGazeTracking();
        bool SubscribeActionDetection();
        bool SubscribeGroundLocation();
        bool SubscribeHandPoseture();
        void SetPreprocessorParameters(Vector3 motionScaling);
        Fitting GetFitting();
        void AddConnectEvent(Action onConnect);
        bool ReleaseGazeTracking();
        bool ReleaseActionDetection();
        bool ReleaseGroundLocation();
        void SetDebug(bool isDebug);
    }
}
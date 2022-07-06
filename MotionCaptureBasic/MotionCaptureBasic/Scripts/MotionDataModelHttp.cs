using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MotionCaptureBasic.Interface;
using MotionCaptureBasic.OSConnector;
using UnityEngine;

namespace MotionCaptureBasic
{
    public class MotionDataModelHttp : IMotionDataModel
    {
        private static MotionDataModelHttp instance;
        
        private static readonly object _Synchronized = new object();

        private ActionDetectionItem simulatActionDetectionItem;
        private HttpProtocolHandler httpProtocolHandler;
        private MotionDataPreprocessor montionDataPreprocessor;
        private List<Vector3> ikPointsDataList;
        private List<Vector3> ikPointsDataListSimulat;

        private MotionDataModelHttp()
        {
            httpProtocolHandler = HttpProtocolHandler.GetInstance();
            montionDataPreprocessor = new MotionDataPreprocessor();
            ikPointsDataList = new List<Vector3>((int)GameKeyPointsType.Count);
            for (int i = 0; i < (int) GameKeyPointsType.Count; i++)
            {
                ikPointsDataList.Add(Vector3.zero);
            }
        }
        
        public static MotionDataModelHttp GetInstance()
        {
            if (instance == null)
            {
                lock(_Synchronized)
                {
                    if(instance == null)
                    {
                        instance = new MotionDataModelHttp();
                    }
                }
            }

            return instance;
        }

        public void SetPreprocessorParameters(Vector3 motionScaling)
        {
            montionDataPreprocessor.ChangeScaleFactor(motionScaling);
        }

        public EventImuHandler OnImuReceived { get; }

        /// <summary>
        /// 获取动捕数据17个关键结点
        /// </summary>
        /// <param name="isLocalCoordinates">是否取人体中心为原点的数据
        /// true: 返回以人体臀部中心点为原点，坐标范围归一化到-1~1的结点数据
        /// false: 人体结点在摄像头拍摄到画面上的坐标，x、y为-1~1的正交空间，z轴方向为对应关键点和摄像头的连线</param>
        /// <param name="isPreprocessed">是否进行前处理</param>
        /// <returns></returns>
        public List<Vector3> GetIKPointsData(bool isLocalCoordinates, bool isPreprocessed)
        {
            if (ikPointsDataListSimulat != null)
            {
                return ikPointsDataListSimulat;
            }
            
            var bodymessage = httpProtocolHandler.BodyMessageBase;

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
                    filteredKeyPoints = montionDataPreprocessor.FilteringKeyPoints(ikBodyUpdateMessage.pose_landmark.keypoints3D);
                }
            }
            else
            {
                if (ikBodyUpdateMessage.pose_landmark.keypoints != null)
                {
                    filteredKeyPoints = montionDataPreprocessor.FilteringKeyPoints(ikBodyUpdateMessage.pose_landmark.keypoints);
                }
            }

            if (filteredKeyPoints == null)
            {
                return null;
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
                ikPointsDataList = montionDataPreprocessor.GetKeyPointsCorrection(ikPointsDataList);
            }

            return ikPointsDataList;
        }

        /// <summary>
        /// 获取人体在摄像机拍摄范围内建立的局部坐标系下的位置信息
        /// </summary>
        /// <returns></returns>
        public GroundLocationItem GetGroundLocationData()
        {
            var bodymessage = httpProtocolHandler.BodyMessageBase;

            if (!(bodymessage is IKBodyUpdateMessage ikBodyUpdateMessage))
            {
                return null;
            }

            return ikBodyUpdateMessage.ground_location;
        }

        public void SetSimulatActionDetectionData(ActionDetectionItem simulatActionDetectionItem)
        {
            this.simulatActionDetectionItem = simulatActionDetectionItem;
        }

        public ActionDetectionItem GetActionDetectionData()
        {
            if (simulatActionDetectionItem != null)
            {
                return simulatActionDetectionItem;
            }
            
            var bodymessage = httpProtocolHandler.BodyMessageBase;

            if (!(bodymessage is IKBodyUpdateMessage ikBodyUpdateMessage))
            {
                return null;
            }

            return ikBodyUpdateMessage.action_detection;
        }

        public MonitorItem GetMonitorData()
        {
            var bodymessage = httpProtocolHandler.BodyMessageBase;

            if (!(bodymessage is IKBodyUpdateMessage ikBodyUpdateMessage))
            {
                return null;
            }

            return ikBodyUpdateMessage.monitor;
        }

        public TimeProfiling GetTimeProfilingData()
        {
            var bodymessage = httpProtocolHandler.BodyMessageBase;

            if (!(bodymessage is IKBodyUpdateMessage ikBodyUpdateMessage))
            {
                return null;
            }

            return ikBodyUpdateMessage.timeProfiling;
        }

        public GazeTracking GetGazeTrackingData()
        {
            var bodymessage = httpProtocolHandler.BodyMessageBase;

            if (!(bodymessage is IKBodyUpdateMessage ikBodyUpdateMessage))
            {
                return null;
            }

            return ikBodyUpdateMessage.gaze_tracking;
        }

        public bool SubscribeGazeTracking()
        {
            return WebsocketOSClient.GetInstance().SubscribeGazeTracking(true);
        }

        public bool SubscribeActionDetection()
        {
            return WebsocketOSClient.GetInstance().SubscribeActionDetection(true);
        }

        public bool SubscribeGroundLocation()
        {
            return WebsocketOSClient.GetInstance().SubscribeGroundLocation(true);
        }

        public bool SubscribeFitting()
        {
            return WebsocketOSClient.GetInstance().SubscribeFitting(true);
        }

        public bool ReleaseGazeTracking()
        {
            return WebsocketOSClient.GetInstance().SubscribeGazeTracking(false);
        }

        public bool ReleaseActionDetection()
        {
            return WebsocketOSClient.GetInstance().SubscribeActionDetection(false);
        }

        public bool ReleaseGroundLocation()
        {
            return WebsocketOSClient.GetInstance().SubscribeGroundLocation(false);
        }

        public bool ReleaseFitting()
        {
            return WebsocketOSClient.GetInstance().SubscribeFitting(false);
        }

        public bool ResetGroundLocation()
        {
            return WebsocketOSClient.GetInstance().ResetGroundLocation();
        }

        /// <summary>
        /// 设置FPS
        /// </summary>
        /// <param name="fps"></param>
        /// <returns></returns>
        public bool SendFrameRateControl(int fps)
        {
            return WebsocketOSClient.GetInstance().SendFrameRateControl(fps);
        }
        /// <summary>
        ///  震动
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="vibrationType"></param>
        /// <param name="strength"></param>
        /// <returns></returns>
        public bool SendVibrationControl(int deviceId, int vibrationType, int strength)
        {
            return WebsocketOSClient.GetInstance().SendVibrationControl(deviceId, vibrationType, strength);
        }
        
        /// <summary>
        ///重置imu
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public bool SendImuResetControl(int deviceId)
        {
            return WebsocketOSClient.GetInstance().SendImuResetControl(deviceId);
        }

        /// <summary>
        /// 心率计控制
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool SendHeartControl(int deviceId, int command)
        {
            return WebsocketOSClient.GetInstance().SendHeartControl(deviceId, command);
        }
        

        public bool SubscribeHandPoseture()
        {
            //TODO: OS层API实现注册后SDK再补上
            return false;
        }

        public Fitting GetFitting()
        {
            return httpProtocolHandler.BodyMessageBase?.fitting;
        }

        public void AddConnectEvent(Action onConnect)
        {
            httpProtocolHandler.AddConnectEvent(onConnect);
        }

        public void SetDebug(bool isDebug)
        {
            httpProtocolHandler.SetDebug(isDebug);
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
using System;
using System.Collections.Generic;
using MotionCaptureBasic.Interface;
using MotionCaptureBasic.OSConnector;
using UnityEngine;

namespace MotionCaptureBasic
{
    public class MotionDataModelHttp : IMotionDataModel
    {
        //private static MotionDataModelHttp instance;
        
        //private static readonly object _Synchronized = new object();
        private ActionDetectionItem simulatActionDetectionItem;
        private HttpProtocolHandler httpProtocol;
        private MotionDataPreprocessor montionDataPreprocessor;
        
        private List<Vector3> ikPointsDataListSimulat;
    
        private Fitting simulateFittingData;

        //private static Dictionary<string, MotionDataModelHttp> motionDataDic = new Dictionary<string, MotionDataModelHttp>();
        //private MotionDataModelHttp(string socketName)
        //{
        //    httpProtocolHandler = new HttpProtocolHandler(socketName);//HttpProtocolHandler.GetInstance();
        //    montionDataPreprocessor = new MotionDataPreprocessor();
        //}
        
        public MotionDataModelHttp(string socketName)
        {
            httpProtocol = new HttpProtocolHandler(socketName);//HttpProtocolHandler.GetInstance();
            montionDataPreprocessor = new MotionDataPreprocessor();
        }
        
        ~MotionDataModelHttp()
        {
            simulatActionDetectionItem = null;
            montionDataPreprocessor = null;
            ikPointsDataListSimulat.Clear();
            ikPointsDataListSimulat = null;
            simulateFittingData = null;
            httpProtocol = null;
            //motionDataDic.Clear();
            //motionDataDic = null;
        }

        public HttpProtocolHandler GetHttpProtocol()
        {
            return httpProtocol;
        }

        //public static MotionDataModelHttp GetInstance(string socketName)
        //{
        //    if (motionDataDic.ContainsKey(socketName))
        //        return motionDataDic[socketName];
        //    var instance = new MotionDataModelHttp(socketName);
        //    motionDataDic.Add(socketName, instance);
        //    //    lock(_Synchronized)
        //    //    {
        //    //        if(instance == null)
        //    //        {
        //    //            instance = new MotionDataModelHttp();
        //    //        }
        //    //    }
        //    //}
        //    return instance;
        //}

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
            
            var bodymessage = httpProtocol.BodyMessageBase;

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
            var bodymessage = httpProtocol.BodyMessageBase;

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

        public void ClearSimulatActionDetectionData()
        {
            simulatActionDetectionItem = null;
        }

        public ActionDetectionItem GetActionDetectionData()
        {
            if (simulatActionDetectionItem != null)
            {
                return simulatActionDetectionItem;
            }
            
            var bodymessage = httpProtocol.BodyMessageBase;

            if (!(bodymessage is IKBodyUpdateMessage ikBodyUpdateMessage))
            {
                return null;
            }

            return ikBodyUpdateMessage.action_detection;
        }

        public MonitorItem GetMonitorData()
        {
            var bodymessage = httpProtocol.BodyMessageBase;

            if (!(bodymessage is IKBodyUpdateMessage ikBodyUpdateMessage))
            {
                return null;
            }

            return ikBodyUpdateMessage.monitor;
        }

        public TimeProfiling GetTimeProfilingData()
        {
            var bodymessage = httpProtocol.BodyMessageBase;

            if (!(bodymessage is IKBodyUpdateMessage ikBodyUpdateMessage))
            {
                return null;
            }

            return ikBodyUpdateMessage.timeProfiling;
        }

        public GazeTracking GetGazeTrackingData()
        {
            var bodymessage = httpProtocol.BodyMessageBase;

            if (!(bodymessage is IKBodyUpdateMessage ikBodyUpdateMessage))
            {
                return null;
            }

            return ikBodyUpdateMessage.gaze_tracking;
        }
        
        public GeneralDetectionItem GetGeneralDetectionData()
        {
            var bodymessage = httpProtocol.BodyMessageBase;

            if (!(bodymessage is IKBodyUpdateMessage ikBodyUpdateMessage))
            {
                return null;
            }

            return ikBodyUpdateMessage.general_detection;
        }

        public StandDetection GetStandDetectionData()
        {
            var bodymessage = httpProtocol.BodyMessageBase;

            if (!(bodymessage is IKBodyUpdateMessage ikBodyUpdateMessage))
            {
                return null;
            }

            return ikBodyUpdateMessage.stand_detection;
        }

        public bool SubscribeGazeTracking()
        {
            return httpProtocol.Websocket.SubscribeGazeTracking(true);
        }

        public bool SubscribeActionDetection()
        {
            return httpProtocol.Websocket.SubscribeActionDetection(true);
        }

        public bool SubscribeGroundLocation()
        {
            return httpProtocol.Websocket.SubscribeGroundLocation(true);
        }

        public bool SubscribeFitting()
        {
            return httpProtocol.Websocket.SubscribeFitting(true);
        }

        public void ReleaseConnectEvent()
        {
            httpProtocolHandler.ReleaseConnectEvent();
        }

        public bool ReleaseGazeTracking()
        {
            return httpProtocol.Websocket.SubscribeGazeTracking(false);
        }

        public bool ReleaseActionDetection()
        {
            return httpProtocol.Websocket.SubscribeActionDetection(false);
        }

        public bool ReleaseGroundLocation()
        {
            return httpProtocol.Websocket.SubscribeGroundLocation(false);
        }

        public bool ReleaseFitting()
        {
            return httpProtocol.Websocket.SubscribeFitting(false);
        }

        public bool ResetGroundLocation()
        {
            return httpProtocol.Websocket.ResetGroundLocation();
        }
        
        public bool SubscribeGeneral()
        {
            return httpProtocol.Websocket.SubscribeGeneral(true);
        }

        public bool ReleaseGeneral()
        {
            return httpProtocol.Websocket.SubscribeGeneral(false);
        }

        /// <summary>
        /// 设置FPS
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fps"></param>
        /// <returns></returns>
        public bool SendFrameRateControl(int fps)
        {
            return httpProtocol.Websocket.SendFrameRateControl(fps);
        }

        public bool SetPlayerHeight(int h)
        {
            return httpProtocol.Websocket.SendHeightSetting(h);
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
            return httpProtocol.Websocket.SendVibrationControl(deviceId, vibrationType, strength);
        }
        
        /// <summary>
        ///重置imu
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public bool SendImuResetControl(int deviceId)
        {
            return httpProtocol.Websocket.SendImuResetControl(deviceId);
        }

        /// <summary>
        /// 心率计控制
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool SendHeartControl(int deviceId, int command)
        {
            return httpProtocol.Websocket.SendHeartControl(deviceId, command);
        }
        

        public bool SubscribeHandPoseture()
        {
            //TODO: OS层API实现注册后SDK再补上
            return false;
        }

        public Fitting GetFitting()
        {
            if (simulateFittingData != null)
            {
                return simulateFittingData;
            }

            return httpProtocol.BodyMessageBase?.fitting;
        }

        public void AddConnectEvent(Action onConnect, Action onClosed = null, Action onError = null)
        {
            httpProtocol.AddConnectEvent(onConnect, onClosed, onError);
        }

        public void SetDebug(bool isDebug)
        {
            httpProtocol.SetDebug(isDebug);
            httpProtocol.Websocket.SetDebug(isDebug);
        }

        public void SetIKDataListSimulat(List<Vector3> ikPointsDataListSimulat)
        {
            this.ikPointsDataListSimulat = ikPointsDataListSimulat;
        }

        public void ClearIKDataListSimulat()
        {
            this.ikPointsDataListSimulat = null;
        }

        public void SetSimulateFittingData(Fitting simulateFittingData)
        {
            this.simulateFittingData = simulateFittingData;
        }

        public void ClearSimulateFittingData()
        {
            simulateFittingData = null;
        }

        public MotionDataModelType GetMotionDataModelType()
        {
            return MotionDataModelType.Http;
        }
    }
}
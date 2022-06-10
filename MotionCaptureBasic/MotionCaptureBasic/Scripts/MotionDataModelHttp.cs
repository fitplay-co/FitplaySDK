using System.Collections.Generic;
using MotionCaptureBasic.Interface;
using MotionCaptureBasic.OSConnector;
using UnityEngine;

namespace MotionCaptureBasic
{
    public class MotionDataModelHttp : IMotionDataModel
    {
        private static MotionDataModelHttp instance;
        
        private static readonly object _Synchronized = new object();
        
        private HttpProtocolHandler httpProtocolHandler;
        private MotionDataPreprocessor montionDataPreprocessor;
        private List<Vector3> ikPointsDataList;

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
            var bodymessage = httpProtocolHandler.BodyMessageBase;

            if (!(bodymessage is IKBodyUpdateMessage ikBodyUpdateMessage))
            {
                return null;
            }
            
            List<KeyPointItem> filteredKeyPoints;

            if (isLocalCoordinates)
            {
                filteredKeyPoints = montionDataPreprocessor.FilteringKeyPoints(ikBodyUpdateMessage.pose_landmark.keypoints3D);
            }
            else
            {
                filteredKeyPoints = montionDataPreprocessor.FilteringKeyPoints(ikBodyUpdateMessage.pose_landmark.keypoints);
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

        public ActionDetectionItem GetActionDetectionData()
        {
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
            //TODO: OS层API实现注册后SDK再补上
            return false;
        }

        public bool SubscribeActionDetection()
        {
            //TODO: OS层API实现注册后SDK再补上
            return false;
        }

        public bool SubscribeGroundLocation()
        {
            //TODO: OS层API实现注册后SDK再补上
            return false;
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
    }
}
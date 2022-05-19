using System.Collections.Generic;
using MotionCaptureBasic.Interface;
using MotionCaptureBasic.OSConnector;
using UnityEngine;

namespace MotionCaptureBasic
{
    public class MotionDataPreprocessor
    {
        private Vector3 scaleFactor;
        private List<KeyPointItem> downSizedKeyPoints;

        /// <summary>
        /// 结点数据预处理类实力化，需要传入各种调节参数进行初始化
        /// </summary>
        public MotionDataPreprocessor()
        {
            this.scaleFactor = Vector3.one;
            downSizedKeyPoints = new List<KeyPointItem>((int) GameKeyPointsType.Count);
            for (int i = 0; i < (int) GameKeyPointsType.Count; i++)
            {
                downSizedKeyPoints.Add(new KeyPointItem());
            }
        }
        
        public List<KeyPointItem> FilteringKeyPoints(List<KeyPointItem> inputList)
        {
            downSizedKeyPoints[(int) GameKeyPointsType.Nose] = inputList[(int) EJointType.Nose];
            downSizedKeyPoints[(int) GameKeyPointsType.LeftShoulder] = inputList[(int) EJointType.RightShoulder];
            downSizedKeyPoints[(int) GameKeyPointsType.RightShoulder] = inputList[(int) EJointType.LeftShoulder];
            downSizedKeyPoints[(int) GameKeyPointsType.LeftElbow] = inputList[(int) EJointType.RightElbow];
            downSizedKeyPoints[(int) GameKeyPointsType.RightElbow] = inputList[(int) EJointType.LeftElbow];
            downSizedKeyPoints[(int) GameKeyPointsType.LeftHand] = inputList[(int) EJointType.RightWrist];
            downSizedKeyPoints[(int) GameKeyPointsType.RightHand] = inputList[(int) EJointType.LeftWrist];
            downSizedKeyPoints[(int) GameKeyPointsType.LeftIndex] = inputList[(int) EJointType.RightIndexKnuckle];
            downSizedKeyPoints[(int) GameKeyPointsType.RightIndex] = inputList[(int) EJointType.LeftIndexKnuckle];
            downSizedKeyPoints[(int) GameKeyPointsType.LeftHip] = inputList[(int) EJointType.RightHip];
            downSizedKeyPoints[(int) GameKeyPointsType.RightHip] = inputList[(int) EJointType.LeftHip];
            downSizedKeyPoints[(int) GameKeyPointsType.LeftKnee] = inputList[(int) EJointType.RightKnee];
            downSizedKeyPoints[(int) GameKeyPointsType.RightKnee] = inputList[(int) EJointType.LeftKnee];
            downSizedKeyPoints[(int) GameKeyPointsType.LeftFoot] = inputList[(int) EJointType.RightAnkle];
            downSizedKeyPoints[(int) GameKeyPointsType.RightFoot] = inputList[(int) EJointType.LeftAnkle];
            downSizedKeyPoints[(int) GameKeyPointsType.LeftFootIndex] = inputList[(int) EJointType.RightFootIndex];
            downSizedKeyPoints[(int) GameKeyPointsType.RightFootIndex] = inputList[(int) EJointType.LeftFootIndex];

            return downSizedKeyPoints;
        }

        /// <summary>
        /// 设置缩放参数
        /// </summary>
        /// <param name="newScaleFactor">对动捕结点进行缩放的缩放系数</param>
        public void ChangeScaleFactor(Vector3 newScaleFactor)
        {
            this.scaleFactor = newScaleFactor;
        }

        public List<Vector3> GetKeyPointsCorrection(List<Vector3> inputList)
        {
            var length = inputList.Count;

            for (int i = 0; i < length; i++)
            {
                inputList[i] = Vector3.Scale(inputList[i], scaleFactor);
            }

            return inputList;
        }
    }
}
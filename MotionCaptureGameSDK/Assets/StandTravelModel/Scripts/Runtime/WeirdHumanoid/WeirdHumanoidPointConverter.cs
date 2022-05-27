using System.Collections.Generic;
using UnityEngine;
using MotionCaptureBasic.Interface;

namespace WeirdHumanoid
{
    public class WeirdHumanoidPointConverter : IKeyPointsConverter
    {
        private Vector3 chestPos;
        private WeirdHumanoidPointsLocater pointsLocater;
        private WeirdHumanoidLimbSizeMapper sizeMapper;

        public WeirdHumanoidPointConverter(WeirdHumanoidPointsLocater pointsLocater)
        {
            this.sizeMapper = new WeirdHumanoidLimbSizeMapper();
            this.pointsLocater = pointsLocater;
        }

        public void ConvertKeyPoints(List<Vector3> keyPoints)
        {
            chestPos = GetChestPoint(keyPoints);

            OverridePointType(GameKeyPointsType.Nose, keyPoints);
            OverridePointType(GameKeyPointsType.LeftShoulder, keyPoints);
            OverridePointType(GameKeyPointsType.RightShoulder, keyPoints);
            OverridePointType(GameKeyPointsType.LeftElbow, keyPoints);
            OverridePointType(GameKeyPointsType.RightElbow, keyPoints);
            OverridePointType(GameKeyPointsType.LeftHand, keyPoints);
            OverridePointType(GameKeyPointsType.RightHand, keyPoints);
            OverridePointType(GameKeyPointsType.LeftHip, keyPoints);
            OverridePointType(GameKeyPointsType.RightHip, keyPoints);
            OverridePointType(GameKeyPointsType.LeftKnee, keyPoints);
            OverridePointType(GameKeyPointsType.RightKnee, keyPoints);
            OverridePointType(GameKeyPointsType.LeftFoot, keyPoints);
            OverridePointType(GameKeyPointsType.RightFoot, keyPoints);
            OverridePointType(GameKeyPointsType.LeftIndex, keyPoints);
            OverridePointType(GameKeyPointsType.RightIndex, keyPoints);
            OverridePointType(GameKeyPointsType.LeftFootIndex, keyPoints);
            OverridePointType(GameKeyPointsType.RightFootIndex, keyPoints);
        }

        private Vector3 GetChestPoint(List<Vector3> keyPoints)
        {
            return (
                keyPoints[(int)GameKeyPointsType.LeftShoulder] +
                keyPoints[(int)GameKeyPointsType.RightShoulder] +
                keyPoints[(int)GameKeyPointsType.LeftHip] +
                keyPoints[(int)GameKeyPointsType.RightHip] ) * 0.25f;
        }

        private void OverridePointType(GameKeyPointsType keyPointsType, List<Vector3> keyPoints)
        {
            keyPoints[(int)keyPointsType] = ConvertPointPos(keyPointsType, keyPoints, chestPos);
        }

        private Vector3 ConvertPointPos(GameKeyPointsType keyPointsType, List<Vector3> keyPoints, Vector3 chestPos)
        {
            var pointIndex = (int)keyPointsType;
            var keyPoint = keyPoints[(int)pointIndex];
            var weirdOffset = pointsLocater.GetLimbOffset(keyPointsType);
            var weirdScale = sizeMapper.GetWeirdLimbSizeScale(keyPointsType, weirdOffset);
            var anchorPos = GetAnchorPosition(keyPointsType, keyPoints, chestPos);
            return ConvertPoint(keyPoint, anchorPos, weirdScale);
        }

        private Vector3 ConvertPoint(Vector3 point, Vector3 anchor, float scale)
        {
            var direct = point - anchor;
            return anchor + direct.normalized * direct.magnitude * scale;
        }

        private Vector3 GetAnchorPosition(GameKeyPointsType keyPointsType, List<Vector3> keyPoints, Vector3 chestPos)
        {
            var anchorType = WeirdHumanoidLimbRelativeGetter.GetAnchorKeyPointType(keyPointsType);
            if(anchorType == keyPointsType)
            {
                return chestPos;
            }
            return keyPoints[(int)anchorType];
        }
    }
}
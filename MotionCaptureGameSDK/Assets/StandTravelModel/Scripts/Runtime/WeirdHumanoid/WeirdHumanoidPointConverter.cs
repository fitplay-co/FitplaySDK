using System.Collections.Generic;
using UnityEngine;
using MotionCaptureBasic.Interface;

namespace WeirdHumanoid
{
    public class WeirdHumanoidPointConverter : IKeyPointsConverter
    {
        private WeirdHumanoidPointsLocater pointsLocater;
        private WeirdHumanoidLimbSizeMapper sizeMapper;

        public WeirdHumanoidPointConverter(WeirdHumanoidPointsLocater pointsLocater)
        {
            this.sizeMapper = new WeirdHumanoidLimbSizeMapper();
            this.pointsLocater = pointsLocater;
        }

        public void ConvertKeyPoints(List<Vector3> keyPoints)
        {
            var chestPos = GetChestPoint(keyPoints);

            OverridePointType(GameKeyPointsType.Nose, keyPoints, chestPos);
            OverridePointType(GameKeyPointsType.LeftShoulder, keyPoints, chestPos);
            OverridePointType(GameKeyPointsType.RightShoulder, keyPoints, chestPos);
            OverridePointType(GameKeyPointsType.LeftElbow, keyPoints, chestPos);
            OverridePointType(GameKeyPointsType.RightElbow, keyPoints, chestPos);
            OverridePointType(GameKeyPointsType.LeftHand, keyPoints, chestPos);
            OverridePointType(GameKeyPointsType.RightHand, keyPoints, chestPos);
            OverridePointType(GameKeyPointsType.LeftHip, keyPoints, chestPos);
            OverridePointType(GameKeyPointsType.RightHip, keyPoints, chestPos);
            OverridePointType(GameKeyPointsType.LeftKnee, keyPoints, chestPos);
            OverridePointType(GameKeyPointsType.RightKnee, keyPoints, chestPos);
            OverridePointType(GameKeyPointsType.LeftFoot, keyPoints, chestPos);
            OverridePointType(GameKeyPointsType.RightKnee, keyPoints, chestPos);
            OverridePointType(GameKeyPointsType.LeftFoot, keyPoints, chestPos);
            OverridePointType(GameKeyPointsType.RightFoot, keyPoints, chestPos);
            OverridePointType(GameKeyPointsType.LeftIndex, keyPoints, chestPos);
            OverridePointType(GameKeyPointsType.RightIndex, keyPoints, chestPos);
        }

        private Vector3 GetChestPoint(List<Vector3> keyPoints)
        {
            return (
                keyPoints[(int)GameKeyPointsType.LeftShoulder] +
                keyPoints[(int)GameKeyPointsType.RightShoulder] +
                keyPoints[(int)GameKeyPointsType.LeftHip] +
                keyPoints[(int)GameKeyPointsType.RightHip] ) * 0.25f;
        }

        private void OverridePointType(GameKeyPointsType keyPointsType, List<Vector3> keyPoints, Vector3 chestPos)
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
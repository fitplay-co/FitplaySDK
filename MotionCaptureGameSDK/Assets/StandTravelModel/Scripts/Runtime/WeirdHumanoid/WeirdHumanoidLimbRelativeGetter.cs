using MotionCaptureBasic.Interface;

namespace WeirdHumanoid
{
    public static class WeirdHumanoidLimbRelativeGetter
    {
        public static GameKeyPointsType GetAnchorKeyPointType(GameKeyPointsType keyPointsType)
        {
            switch(keyPointsType)
            {
                case GameKeyPointsType.Nose:
                    return GameKeyPointsType.Nose;
                case GameKeyPointsType.LeftShoulder:
                    return GameKeyPointsType.LeftShoulder;
                case GameKeyPointsType.RightShoulder:
                    return GameKeyPointsType.RightShoulder;
                case GameKeyPointsType.LeftHip:
                    return GameKeyPointsType.LeftHip;
                case GameKeyPointsType.RightHip:
                    return GameKeyPointsType.RightHip;
                case GameKeyPointsType.LeftElbow:
                    return GameKeyPointsType.LeftShoulder;
                case GameKeyPointsType.RightElbow:
                    return GameKeyPointsType.RightShoulder;
                case GameKeyPointsType.LeftHand:
                    return GameKeyPointsType.LeftElbow;
                case GameKeyPointsType.RightHand:
                    return GameKeyPointsType.RightElbow;
                case GameKeyPointsType.LeftIndex:
                    return GameKeyPointsType.LeftHand;
                case GameKeyPointsType.RightIndex:
                    return GameKeyPointsType.RightHand;
                case GameKeyPointsType.LeftKnee:
                    return GameKeyPointsType.LeftHip;
                case GameKeyPointsType.RightKnee:
                    return GameKeyPointsType.RightHip;
                case GameKeyPointsType.LeftFoot:
                    return GameKeyPointsType.LeftKnee;
                case GameKeyPointsType.RightFoot:
                    return GameKeyPointsType.RightKnee;
                case GameKeyPointsType.LeftFootIndex:
                    return GameKeyPointsType.LeftFoot;
                case GameKeyPointsType.RightFootIndex:
                    return GameKeyPointsType.RightFoot;
            }
            return GameKeyPointsType.Count;
        }
    }
}
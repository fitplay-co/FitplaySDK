using MotionCaptureBasic.Interface;

namespace StandTravelModel.Scripts.Runtime.WeirdHumanoid
{
    public static class WeirdHumanoidLimbRelativeGetter
    {
        private static GameKeyPointsType[] pointsRelatives;

        public static GameKeyPointsType GetAnchorKeyPointType(GameKeyPointsType keyPointsType)
        {
            if(pointsRelatives == null)
            {
                var pointsTypes = System.Enum.GetNames(typeof(GameKeyPointsType));
                pointsRelatives = new GameKeyPointsType[pointsTypes.Length];
                pointsRelatives[(int)GameKeyPointsType.Nose] = GameKeyPointsType.Nose;
                pointsRelatives[(int)GameKeyPointsType.LeftShoulder] = GameKeyPointsType.LeftShoulder;
                pointsRelatives[(int)GameKeyPointsType.RightShoulder] = GameKeyPointsType.RightShoulder;
                pointsRelatives[(int)GameKeyPointsType.LeftHip] = GameKeyPointsType.LeftHip;
                pointsRelatives[(int)GameKeyPointsType.RightHip] = GameKeyPointsType.RightHip;
                pointsRelatives[(int)GameKeyPointsType.LeftElbow] = GameKeyPointsType.LeftShoulder;
                pointsRelatives[(int)GameKeyPointsType.RightElbow] = GameKeyPointsType.RightShoulder;
                pointsRelatives[(int)GameKeyPointsType.LeftHand] = GameKeyPointsType.LeftElbow;
                pointsRelatives[(int)GameKeyPointsType.RightHand] = GameKeyPointsType.RightElbow;
                pointsRelatives[(int)GameKeyPointsType.LeftIndex] = GameKeyPointsType.LeftHand;
                pointsRelatives[(int)GameKeyPointsType.RightIndex] = GameKeyPointsType.RightHand;
                pointsRelatives[(int)GameKeyPointsType.LeftKnee] = GameKeyPointsType.LeftHip;
                pointsRelatives[(int)GameKeyPointsType.RightKnee] = GameKeyPointsType.RightHip;
                pointsRelatives[(int)GameKeyPointsType.LeftFoot] = GameKeyPointsType.LeftKnee;
                pointsRelatives[(int)GameKeyPointsType.RightFoot] = GameKeyPointsType.RightKnee;
                pointsRelatives[(int)GameKeyPointsType.LeftFootIndex] = GameKeyPointsType.LeftFoot;
                pointsRelatives[(int)GameKeyPointsType.RightFootIndex] = GameKeyPointsType.RightFoot;
            }

            var index = (int)keyPointsType;
            if(index >= 0 && index < pointsRelatives.Length)
            {
                return pointsRelatives[index];
            }
            
            return GameKeyPointsType.Count;
        }
    }
}
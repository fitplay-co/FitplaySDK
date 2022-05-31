using MotionCaptureBasic.Interface;

public class WeirdHumanoidLimbSizeMapper
{
    private float[] limbOffsets;

    public WeirdHumanoidLimbSizeMapper()
    {
        var limbOffsetLeftShoulder = 0.35f;
        var limbOffsetRightShoulder = 0.35f;
        var limbOffsetLeftElbow = 0.4f;
        var limbOffsetRightElbow = 0.4f;
        var limbOffsetLeftHand = 0.3f;
        var limbOffsetRightHand = 0.3f;
        var limbOffsetLeftIndex = 0.08f;
        var limbOffsetRightIndex = 0.08f;
        var limbOffsetLeftHip = 0.5f;
        var limbOffsetRightHip = 0.5f;
        var limbOffsetLeftKnee = 0.55f;
        var limbOffsetRightKnee = 0.55f;
        var limbOffsetLeftFoot = 0.55f;
        var limbOffsetRightFoot = 0.55f;
        var limbOffsetNose = 0.4f;
        var limbOffsetLeftFootIndex = 0.15f;
        var limbOffsetRightFootIndex = 0.15f;

        var pointsTypes = System.Enum.GetValues(typeof(GameKeyPointsType));
        limbOffsets = new float[pointsTypes.Length];
        limbOffsets[(int)GameKeyPointsType.LeftShoulder] = limbOffsetLeftShoulder;
        limbOffsets[(int)GameKeyPointsType.RightShoulder] = limbOffsetRightShoulder;
        limbOffsets[(int)GameKeyPointsType.LeftElbow] = limbOffsetLeftElbow;
        limbOffsets[(int)GameKeyPointsType.RightElbow] = limbOffsetRightElbow;
        limbOffsets[(int)GameKeyPointsType.LeftHand] = limbOffsetLeftHand;
        limbOffsets[(int)GameKeyPointsType.RightHand] = limbOffsetRightHand;
        limbOffsets[(int)GameKeyPointsType.LeftHip] = limbOffsetLeftHip;
        limbOffsets[(int)GameKeyPointsType.RightHip] = limbOffsetRightHip;
        limbOffsets[(int)GameKeyPointsType.LeftFoot] = limbOffsetLeftFoot;
        limbOffsets[(int)GameKeyPointsType.RightFoot] = limbOffsetRightFoot;
        limbOffsets[(int)GameKeyPointsType.LeftFootIndex] = limbOffsetLeftFootIndex;
        limbOffsets[(int)GameKeyPointsType.RightFootIndex] = limbOffsetRightFootIndex;
        limbOffsets[(int)GameKeyPointsType.LeftKnee] = limbOffsetLeftKnee;
        limbOffsets[(int)GameKeyPointsType.RightKnee] = limbOffsetRightKnee;
        limbOffsets[(int)GameKeyPointsType.Nose] = limbOffsetNose;
        limbOffsets[(int)GameKeyPointsType.LeftIndex] = limbOffsetLeftIndex;
        limbOffsets[(int)GameKeyPointsType.RightIndex] = limbOffsetRightIndex;
    }

    public float GetWeirdLimbSizeScale(GameKeyPointsType keyPointsType, float weirdSize)
    {
        return weirdSize / GetHumanLimbSize(keyPointsType);
    }

    private float GetHumanLimbSize(GameKeyPointsType keyPointsType)
    {
        return limbOffsets[(int)keyPointsType];
    }
}

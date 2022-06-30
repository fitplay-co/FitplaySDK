using MotionCaptureBasic.Interface;
using UnityEngine;

public class ReconCompAngleGetterWithDirect : ReconCompAngleGetter
{
    private Vector3 baseDirect;

    public ReconCompAngleGetterWithDirect(GameKeyPointsType pointFor, GameKeyPointsType pointMid, GameKeyPointsType pointBak, Vector3 baseDirect) : base(pointFor, pointMid, pointBak)
    {
        this.baseDirect = baseDirect;
    }

    protected override Vector3 GetDirectBakToMid(Vector3 posBak, Vector3 posMid)
    {
        return baseDirect;
    }
}
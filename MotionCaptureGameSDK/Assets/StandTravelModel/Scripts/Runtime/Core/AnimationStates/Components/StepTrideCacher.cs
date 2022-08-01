using UnityEngine;

public class StepStrideCacher
{
    private int lastLeg;
    private float stride;
    private float strideSmooth;

    public void OnUpdate(int leg, float stride)
    {
        if(this.lastLeg > 0 && leg == -1)
        {
            this.stride = stride;
        }
        this.lastLeg = leg;
        this.strideSmooth = Mathf.Lerp(strideSmooth, stride, Time.deltaTime * 2);
    }

    public float GetStride()
    {
        return stride;
    }

    public int GetLeg()
    {
        return lastLeg;
    }

    public float GetStrideSmooth()
    {
        return strideSmooth;
    }
}
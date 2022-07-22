using UnityEngine;

public class StepStrideCacher
{
    private int lastLeg;
    private float stride;

    public void OnUpdate(int leg, float stride)
    {
        if(this.lastLeg > 0 && leg == -1)
        {
            this.stride = stride;
        }
        this.lastLeg = leg;
    }

    public float GetStride()
    {
        return stride;
    }

    public int GetLeg()
    {
        return lastLeg;
    }
}
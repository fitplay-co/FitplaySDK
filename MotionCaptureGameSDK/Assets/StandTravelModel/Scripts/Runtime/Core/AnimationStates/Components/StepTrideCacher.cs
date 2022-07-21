using UnityEngine;

public class StepStrideCacher
{
    private int legUp;
    private float? stepUpStart;
    private float? stepUpEnd;

    public void OnUpdate(int leg, float hipAngle)
    {
        if(leg < 1)
        {
            legUp = leg;
            stepUpStart = hipAngle;
            stepUpEnd = null;
        }
        else
        {
            legUp = leg;
            stepUpEnd = hipAngle;
        }
    }

    public float GetStride()
    {
        if(stepUpStart != null && stepUpEnd != null)
        {
            return Mathf.Abs(stepUpEnd.Value - stepUpStart.Value); 
        }
        return -1;
    }
}
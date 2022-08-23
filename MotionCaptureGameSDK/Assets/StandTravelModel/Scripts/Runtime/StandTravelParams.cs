using System;

[Serializable]
public class StandTravelParams
{
    public float runThrehold = 0.25f;
    public float runThreholdLow = 0.35f;
    public bool useFrequency = true;
    public float runSpeedScale = 1;
    public float sprintThrehold = 0.15f;
    public float runThresholdScale = 1;
    public float runThresholdScaleLow = 0.8f;
    public float sprintSpeedScale = 1.33f;
    public bool useSmoothSwitch = false;
}
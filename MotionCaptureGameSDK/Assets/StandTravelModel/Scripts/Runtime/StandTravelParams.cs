using System;
using UnityEngine;

[Serializable]
public class StandTravelParams
{
    [SerializeField] float runThrehold = 0.25f;
    [SerializeField] float runThreholdLow = 0.35f;
    [SerializeField] float freqThrehold = 0.25f;
    [SerializeField] float freqThreholdLow = 0.35f;
    public bool useFrequency = true;
    public float runSpeedScale = 1;
    [SerializeField] float sprintThrehold = 0.15f;
    [SerializeField] float freqSprintThrehold = 0.15f;
    public float runThresholdScale = 1;
    public float runThresholdScaleLow = 0.8f;
    public float sprintSpeedScale = 1.33f;
    public bool useSmoothSwitch = false;

    public float GetRunThrehold()
    {
        if(useFrequency)
        {
            return freqThrehold;
        }
        return runThrehold;
    }

    public void SetRunThrehold(float value)
    {
        if(useFrequency)
        {
            freqThrehold = value;
        }
        else
        {
            runThrehold = value;
        }
    }

    public float GetRunThreholdLow()
    {
        if(useFrequency)
        {
            return freqThreholdLow;
        }
        return runThreholdLow;
    }

    public void SetRunThreholdLow(float value)
    {
        if(useFrequency)
        {
            freqThreholdLow = value;
        }
        else
        {
            runThreholdLow = value;
        }
    }

    public float GetSprintThrehold()
    {
        if(useFrequency)
        {
            return freqSprintThrehold;
        }
        return sprintThrehold;
    }

    public void SetSprintThrehold(float value)
    {
        if(useFrequency)
        {
            freqSprintThrehold = value;
        }
        else
        {
            sprintThrehold = value;
        }
    }
}
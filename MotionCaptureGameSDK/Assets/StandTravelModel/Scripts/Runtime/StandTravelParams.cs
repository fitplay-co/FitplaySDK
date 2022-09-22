using System;
using UnityEngine;

[Serializable]
public class StandTravelParams
{
    public float runThrehold = 1.2f;
    public float runThreholdLow = 0.8f;
    public float freqThrehold = 2.5f;
    public float freqThreholdLow = 2.3f;
    public bool useFrequency = true;
    public float runSpeedScale = 3;
    public float walkSpeedScale = 1.5f;
    public float sprintThrehold = 0.8f;
    public float  freqSprintThrehold = 0.14f;
    public float runThresholdScale = 0.9f;
    public float runThresholdScaleLow = 0.75f;
    public float sprintSpeedScale = 1.5f;
    public bool useSmoothSwitch = false;
    public bool useOSSpeed = false;
    public bool useOSStepRate = true;
    public bool useOSStepRateSeparate = false;
    public bool useLegActTime = false;

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

    public bool GetUseOSStepRate()
    {
        return useOSStepRate;
    }

    public void SetUseOSStepRate(bool value)
    {
        if(value)
        {
            ClearStepRates();
        }
        this.useOSStepRate = value;
    }

    public bool GetUseOSStepRateSeparate()
    {
        return useOSStepRateSeparate;
    }

    public void SetUseOSStepRateSeparate(bool value)
    {
        if(value)
        {
            ClearStepRates();
        }
        this.useOSStepRateSeparate = value;
    }

    public bool GetUseLegActTime()
    {
        return useLegActTime;
    }

    public void SetUseLegActTime(bool value)
    {
        if(value)
        {
            ClearStepRates();
        }
        this.useLegActTime = value;
    }

    private void ClearStepRates()
    {
        useOSStepRate = false;
        useOSStepRateSeparate = false;
        useLegActTime = false; 
    }
}
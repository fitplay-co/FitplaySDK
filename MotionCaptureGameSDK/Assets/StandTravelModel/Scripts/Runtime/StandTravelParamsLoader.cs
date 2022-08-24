using System;
using UnityEngine;

[Serializable]
public class StandTravelParamsLoader
{
    [SerializeField] private StandTravelParams standTravelParams;

    private const string fileName = "/standTravelParams.json";

    public void Deserialize()
    {
        standTravelParams = JsonHelper.Deserialize<StandTravelParams>(GetFilePath());
        if(standTravelParams == null)
        {
            standTravelParams = new StandTravelParams();
        }
    }

    public void Serialize()
    {
        JsonHelper.Serialize<StandTravelParams>(GetFilePath(), standTravelParams);
    }

    public float GetRunThrehold()
    {
        return standTravelParams.GetRunThrehold();
    }

    public void SetRunThrehold(float value)
    {
        standTravelParams.SetRunThrehold(value);
    }

    public float GetRunThreholdLow()
    {
        return standTravelParams.GetRunThreholdLow();
    }

    public void SetRunThreholdLow(float value)
    {
        standTravelParams.SetRunThreholdLow(value);
    }

    public bool GetUseSmoothSwitch()
    {
        return standTravelParams.useSmoothSwitch;
    }

    public void SetUseSmoothSwitch(bool value)
    {
        standTravelParams.useSmoothSwitch = value;
    }

    public bool GetUseFrequency()
    {
        return standTravelParams.useFrequency;
    }

    public void SetUseFrequency(bool value)
    {
        standTravelParams.useFrequency = value;
    }

    public float GetSprintSpeedScale()
    {
        return standTravelParams.sprintSpeedScale;
    }

    public void SetSprintSpeedScale(float value)
    {
        standTravelParams.sprintSpeedScale = value;
    }

    public float GetRunSpeedScale()
    {
        return standTravelParams.runSpeedScale;
    }

    public void SetRunSpeedScale(float value)
    {
        standTravelParams.runSpeedScale = value;
    }

    public float GetSprintThrehold()
    {
        return standTravelParams.GetSprintThrehold();
    }

    public void SetSprintThrehold(float value)
    {
        standTravelParams.SetSprintThrehold(value);
    }
    
    public float GetRunThresholdScale()
    {
        return standTravelParams.runThresholdScale;
    }

    public void SetRunThresholdScale(float value)
    {
        standTravelParams.runThresholdScale = value;
    }

    public float GetRunThreholdScaleLow()
    {
        return standTravelParams.runThresholdScaleLow;
    }

    public void SetRunThreholdScaleLow(float value)
    {
        standTravelParams.runThresholdScaleLow = value;
    }

    private string GetFilePath()
    {
        return Application.dataPath + "/Resources" + fileName;
    }
}
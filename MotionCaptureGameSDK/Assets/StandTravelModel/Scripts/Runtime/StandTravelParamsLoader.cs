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
        return standTravelParams.runThrehold;
    }

    public void SetRunThrehold(float value)
    {
        standTravelParams.runThrehold = value;
    }

    public bool GetUseFrequency()
    {
        return standTravelParams.useFrequency;
    }

    public void SetUseFrequency(bool value)
    {
        standTravelParams.useFrequency = value;
    }

    public float GetRunSpeedScale()
    {
        return standTravelParams.runSpeedScale;
    }

    public void SetRunSpeedScale(float value)
    {
        standTravelParams.runSpeedScale = value;
    }

    private string GetFilePath()
    {
        return Application.persistentDataPath + fileName;
    }
}
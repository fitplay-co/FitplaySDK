using System;
using StandTravelModel.Scripts.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class StandTravelTestUI : MonoBehaviour
{
    [SerializeField] private Button saveButton;
    [SerializeField] private Toggle useSwitchSpeed;
    [SerializeField] private Toggle useSwitchFreq;
    [SerializeField] private Toggle useSmoothSwitch;
    [SerializeField] private InputField threholdFreq;
    [SerializeField] private InputField threholdFreqLow;
    [SerializeField] private InputField speedThreholdScale;
    [SerializeField] private InputField speedThreholdScaleLow;
    [SerializeField] private InputField sprintThrehold;
    [SerializeField] private InputField speedScale;
    [SerializeField] private InputField sprintSpeedScale;
    [SerializeField] private StandTravelModelManager standTravelModelManager;

    private void Start() {
        
        RefreshToggles();

        UpdateInputField(threholdFreq, standTravelModelManager.GetRunThrehold());
        UpdateInputField(threholdFreqLow, standTravelModelManager.GetRunThreholdLow());
        UpdateInputField(speedThreholdScale, standTravelModelManager.GetRunThresholdScale());
        UpdateInputField(speedThreholdScaleLow, standTravelModelManager.GetRunThresholdScaleLow());
        UpdateInputField(sprintThrehold, standTravelModelManager.GetSprintThrehold());
        UpdateInputField(speedScale, standTravelModelManager.GetRunSpeedScale());
        UpdateInputField(sprintSpeedScale, standTravelModelManager.GetSprintSpeedScale());

        threholdFreq.onEndEdit.AddListener(OnInputThreholdFreq);
        threholdFreqLow.onEndEdit.AddListener(OnInputThreholdFreqLow);
        speedThreholdScale.onEndEdit.AddListener(OnInputSpeedThreholdScale);
        speedThreholdScaleLow.onEndEdit.AddListener(OnInputSpeedThreholdScaleLow);
        sprintThrehold.onEndEdit.AddListener(OnInputSprintThrehold);
        speedScale.onEndEdit.AddListener(OnInputSpeedScale);
        sprintSpeedScale.onEndEdit.AddListener(OnInputSprintSpeedScale);

        saveButton.onClick.AddListener(Save);
    }

    private void RefreshToggles()
    {
        useSwitchSpeed.onValueChanged.RemoveAllListeners();
        useSwitchFreq.onValueChanged.RemoveAllListeners();
        useSmoothSwitch.onValueChanged.RemoveAllListeners();

        useSwitchSpeed.isOn = !standTravelModelManager.GetUseFrequency();
        useSwitchFreq.isOn = standTravelModelManager.GetUseFrequency();
        useSmoothSwitch.isOn = standTravelModelManager.GetUseSmoothSwitch();

        useSwitchSpeed.onValueChanged.AddListener(OnUseSpeed);
        useSwitchFreq.onValueChanged.AddListener(OnUseFreq);
        useSmoothSwitch.onValueChanged.AddListener(OnUseSmooth);
    }

    private void OnUseFreq(bool value)
    {
        standTravelModelManager.SetUseFrequency(value);
        RefreshToggles();
    }

    private void OnUseSpeed(bool value)
    {
        standTravelModelManager.SetUseFrequency(!value);
        RefreshToggles();
    }

    private void OnUseSmooth(bool value)
    {
        standTravelModelManager.SetUseSmoothSwitch(value);
        RefreshToggles();
    }

    private void OnInputThreholdFreq(string content)
    {
        DOInput(content, standTravelModelManager.SetRunThrehold);
    }

    private void OnInputThreholdFreqLow(string content)
    {
        DOInput(content, standTravelModelManager.SetRunThreholdLow);
    }

    private void OnInputSpeedThreholdScale(string content)
    {
        DOInput(content, standTravelModelManager.SetRunThresholdScale);
    }

    private void OnInputSpeedThreholdScaleLow(string content)
    {
        DOInput(content, standTravelModelManager.SetRunThresholdScaleLow);
    }

    private void OnInputSprintThrehold(string content)
    {
        DOInput(content, standTravelModelManager.SetSprintThrehold);
    }

    private void OnInputSpeedScale(string content)
    {
        DOInput(content, standTravelModelManager.SetRunSpeedScale);
    }

    private void OnInputSprintSpeedScale(string content)
    {
        DOInput(content, standTravelModelManager.SetSprintSpeedScale);
    }

    private void DOInput(string content, Action<float> setValue)
    {
        var value = 0f;
        if(float.TryParse(content, out value))
        {
            setValue(value);
        }
    }

    private void UpdateInputField(InputField inputField, float value)
    {
        inputField.text = value.ToString();
    }

    private void Save()
    {
        standTravelModelManager.SerializeParams();
    }
}
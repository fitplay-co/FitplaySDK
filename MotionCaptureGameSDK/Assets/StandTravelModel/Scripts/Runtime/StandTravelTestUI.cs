using System;
using StandTravelModel.Scripts.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class StandTravelTestUI : MonoBehaviour
{
    [SerializeField] private Button saveButton;
    [SerializeField] private Toggle useOSSpeed;
    [SerializeField] private Toggle useSwitchSpeed;
    [SerializeField] private Toggle useSwitchFreq;
    [SerializeField] private Toggle useSmoothSwitch;
    [SerializeField] private InputField threholdFreq;
    [SerializeField] private InputField threholdFreqLow;
    [SerializeField] private InputField speedThreholdScale;
    [SerializeField] private InputField speedThreholdScaleLow;
    [SerializeField] private InputField sprintThrehold;
    [SerializeField] private InputField speedScale;
    [SerializeField] private InputField walkSpeedScale;
    [SerializeField] private InputField sprintSpeedScale;
    [SerializeField] private StandTravelModelManager standTravelModelManager;

    [SerializeField] private Toggle useOSStepRate;
    [SerializeField] private Toggle useOSStepRateSeparate;
    [SerializeField] private Toggle useLegActTime;

    private void Start() {
        Init();
    }

    public void OccupyTandTravelModelManager(StandTravelModelManager standTravelModelManager)
    {
        if(this.standTravelModelManager == null)
        {
            this.standTravelModelManager = standTravelModelManager;
            Init();
        }
    }

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }

    private void Init()
    {
        if(standTravelModelManager != null)
        {
            Refresh();
            SetupEvents();
        }
    }

    private void SetupEvents()
    {
        threholdFreq.onEndEdit.AddListener(OnInputThreholdFreq);
        threholdFreqLow.onEndEdit.AddListener(OnInputThreholdFreqLow);
        speedThreholdScale.onEndEdit.AddListener(OnInputSpeedThreholdScale);
        speedThreholdScaleLow.onEndEdit.AddListener(OnInputSpeedThreholdScaleLow);
        sprintThrehold.onEndEdit.AddListener(OnInputSprintThrehold);
        speedScale.onEndEdit.AddListener(OnInputSpeedScale);
        walkSpeedScale.onEndEdit.AddListener(OnInputeWalkSpeedScale);
        sprintSpeedScale.onEndEdit.AddListener(OnInputSprintSpeedScale);

        saveButton.onClick.AddListener(Save);
    }

    private void Refresh()
    {
        RefreshToggles();
        RefreshInputFields();
    }

    private void RefreshInputFields()
    {
        UpdateInputField(threholdFreq, standTravelModelManager.GetRunThrehold());
        UpdateInputField(threholdFreqLow, standTravelModelManager.GetRunThreholdLow());
        UpdateInputField(speedThreholdScale, standTravelModelManager.GetRunThresholdScale());
        UpdateInputField(speedThreholdScaleLow, standTravelModelManager.GetRunThresholdScaleLow());
        UpdateInputField(sprintThrehold, standTravelModelManager.GetSprintThrehold());
        UpdateInputField(speedScale, standTravelModelManager.GetRunSpeedScale());
        UpdateInputField(sprintSpeedScale, standTravelModelManager.GetSprintSpeedScale());
        UpdateInputField(walkSpeedScale, standTravelModelManager.GetWalkSpeedScale());
    }

    private void RefreshToggles()
    {
        useOSSpeed.onValueChanged.RemoveAllListeners();
        useSwitchSpeed.onValueChanged.RemoveAllListeners();
        useSwitchFreq.onValueChanged.RemoveAllListeners();
        useSmoothSwitch.onValueChanged.RemoveAllListeners();
        useOSStepRate.onValueChanged.RemoveAllListeners();
        useOSStepRateSeparate.onValueChanged.RemoveAllListeners();
        useLegActTime.onValueChanged.RemoveAllListeners();

        useOSSpeed.isOn = standTravelModelManager.GetUseOSSpeed();
        useSwitchSpeed.isOn = !standTravelModelManager.GetUseFrequency();
        useSwitchFreq.isOn = standTravelModelManager.GetUseFrequency();
        useSmoothSwitch.isOn = standTravelModelManager.GetUseSmoothSwitch();
        useOSStepRate.isOn = standTravelModelManager.GetUseOSStepRate();
        useOSStepRateSeparate.isOn = standTravelModelManager.GetUseOSStepRateSeparate();
        useLegActTime.isOn = standTravelModelManager.GetUseLegActTime();


        useOSSpeed.onValueChanged.AddListener(OnUseOSSpeed);
        useSwitchSpeed.onValueChanged.AddListener(OnUseSpeed);
        useSwitchFreq.onValueChanged.AddListener(OnUseFreq);
        useSmoothSwitch.onValueChanged.AddListener(OnUseSmooth);
        useOSStepRate.onValueChanged.AddListener(OnUseOSStepRate);
        useOSStepRateSeparate.onValueChanged.AddListener(OnUseOSStepRateSeparate);
        useLegActTime.onValueChanged.AddListener(OnUseLegActTime);
    }

    private void OnUseFreq(bool value)
    {
        standTravelModelManager.SetUseFrequency(value);
        Refresh();
    }

    private void OnUseSpeed(bool value)
    {
        standTravelModelManager.SetUseFrequency(!value);
        Refresh();
    }

    private void OnUseSmooth(bool value)
    {
        standTravelModelManager.SetUseSmoothSwitch(value);
        Refresh();
    }

    private void OnUseOSSpeed(bool value)
    {
        standTravelModelManager.SetUseOSSpeed(value);
    }

    private void OnUseOSStepRate(bool value)
    {
        standTravelModelManager.SetUseOSStepRate(value);
        Refresh();
    }

    private void OnUseOSStepRateSeparate(bool value)
    {
        standTravelModelManager.SetUseOSStepRateSeparate(value);
        Refresh();
    }

    private void OnUseLegActTime(bool value)
    {
        standTravelModelManager.SetUseLegActTime(value);
        Refresh();
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

    private void OnInputeWalkSpeedScale(string content)
    {
        DOInput(content, standTravelModelManager.SetWalkSpeedScale);
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
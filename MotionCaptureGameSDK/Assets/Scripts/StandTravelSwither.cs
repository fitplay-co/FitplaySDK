using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MotionLib.Scripts;
using StandTravelModel.Scripts.Runtime;
using MotionCaptureBasic.Interface;

namespace Scripts
{
    public class StandTravelSwither : MonoBehaviour
    {
        public float postureTimeout = 10;

        private Dropdown standTravelSwitchOpt;
        private StandTravelModelManager standTravelModelManager;

        private const string joystick_button_0 = "joystick button 0";
        private float timeProgress = 0;

        // Start is called before the first frame update
        public void Start()
        {
            standTravelSwitchOpt = GameObject.Find("StandTravelSwitchOpt").GetComponent<Dropdown>();
            standTravelModelManager = GetComponent<StandTravelModelManager>();
            MotionLibEventHandler.onSwithStandToTravel += OnSwitchStandTravel;
            //MotionLibEventHandler.onMotionChanged += OnSwitchStandTravel;
        }

        // Update is called once per frame
        public void Update()
        {
            if (standTravelModelManager == null)
            {
                Debug.LogError("StandTravelModelManager was not found in StandTravelSwither");
                return;
            }

            var dropdownValue = standTravelSwitchOpt.value;

            switch (dropdownValue) 
            {
                case 0:
                    SwitchByKeyInput();
                    break;
                case 1:
                    SwitchByOsRecon();
                    break;
                case 2:
                    var mode = standTravelModelManager.currentMode;
                    if (mode == MotionMode.Travel) 
                    {
                        var motionDataModel = standTravelModelManager.motionDataModelReference;
                        var osSiwtch = motionDataModel.GetStandDetectionData()?.mode;
                        if (osSiwtch == 0)
                        {
                            var deltaTime = Time.deltaTime;
                            timeProgress += deltaTime;
                            if (timeProgress > postureTimeout)
                            {
                                standTravelModelManager.SwitchStandTravel();
                                timeProgress = 0;
                            }
                        }
                        else 
                        {
                            timeProgress = 0;
                        }
                    }
                    break;
            }
        }

        private void SwitchByKeyInput()
        {
            bool isChangeMode = false;

            isChangeMode |= Input.GetKeyDown(joystick_button_0);
            isChangeMode |= Input.GetKeyDown(KeyCode.Z);

            if (isChangeMode)
            {
                standTravelModelManager.SwitchStandTravel();
            }
        }

        private void SwitchByOsRecon()
        {
            var mode = standTravelModelManager.currentMode;
            var motionDataModel = standTravelModelManager.motionDataModelReference;
            var standDetectionData = motionDataModel.GetStandDetectionData();
            if (standDetectionData == null) 
            {
                return;
            }
            var osSiwtch = standDetectionData.mode;

            //Debug.Log($"OS stand detection value: {osSiwtch}");
            
            switch (mode)
            {
                case MotionMode.Stand:
                    if (osSiwtch == 1)
                    {
                        standTravelModelManager.SwitchStandTravel();
                    }
                    break;
                case MotionMode.Travel:
                    if (osSiwtch == 0) 
                    {
                        standTravelModelManager.SwitchStandTravel();
                    }
                    break;
            }
        }

        private void OnSwitchStandTravel()
        {
            if (standTravelSwitchOpt.value != 2) 
            {
                return;
            }

            //Debug.Log($"posture recognized");

            var mode = standTravelModelManager.currentMode;
            if (mode == MotionMode.Stand)
            {
                standTravelModelManager.SwitchStandTravel();
            }
        }
    }
}


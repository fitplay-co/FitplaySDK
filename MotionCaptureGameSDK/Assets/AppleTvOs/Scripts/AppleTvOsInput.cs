using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MotionCaptureBasic.OSConnector;
using UnityEngine;
using UnityEngine.UI;

namespace AppleTvOs
{
    public class AppleTvOsInput : MonoBehaviour
    {
        [Tooltip("输入窗口的标题")] [SerializeField] private string Title = "";
        [Tooltip("输入框的默认值")] [SerializeField] private string DefaultInputText = "";

        [Tooltip("Commit按钮的默认文字")] [SerializeField]
        private string DefaultOKButtonText = "";

        [Tooltip("Connect按钮的默认文字")] [SerializeField]
        private string DefaultConnectButtonText = "";

        [Tooltip("提示信息文字")] [SerializeField] private string DefaultNoticeText = "";
        [SerializeField] private float AxisValue = 0.5f;
        [SerializeField] private InputField inputField;
        [SerializeField] private Button commitButton;
        [SerializeField] private List<VirtualButton> ButtonList;
        [SerializeField] private bool isIPAddress;

        private List<string> AlphaBetList = new List<string>()
            {".", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "CONNECT"};

        private const string joystickButton14 = "joystick button 14";
        private const string joystickButton15 = "joystick button 15";
        private int IndexId = 2;
        private bool horizontalDown = false;
        private bool verticalDown = false;
        private int totalButtons;

        private Transform buttonGroup;

        private GameObject inputHud;
        private GameObject displayHud;
        private Text notice;
        public bool IsInputMode = true;
        public bool isUseJson;

        void Start()
        {
            inputHud = transform.Find("InputHud").gameObject;
            inputHud.SetActive(true);
            displayHud = transform.Find("DisplayHud").gameObject;
            displayHud.SetActive(false);

            inputHud.transform.Find("Title").GetComponent<Text>().text = Title;
            notice = inputHud.transform.Find("Notice").GetComponent<Text>();
            notice.text = DefaultNoticeText;
            commitButton.GetComponentInChildren<Text>().text = DefaultOKButtonText;
            inputField.text = DefaultInputText;

            buttonGroup = inputHud.transform.Find("buttonGroup");


#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_STANDALONE_OSX
            buttonGroup.gameObject.SetActive(false);
            commitButton.gameObject.SetActive(true);
            commitButton.onClick.AddListener(() => { CheckInputAndSubmit(); });
            if(isIPAddress)
                notice.text = "如果IP输入错误，可以用\"BackSpace\"键删除，IP输入完成后可以使用\"Enter\"键连接服务器！";
#else
            buttonGroup.gameObject.SetActive(true);
            commitButton.gameObject.SetActive(false);
            if(isIPAddress)
                notice.text = "如果IP输入错误，请直接CONNECT，然后通过确定键再次呼叫出输入UI，重新输入.";
            totalButtons = buttonGroup.childCount;
            if (buttonGroup == null) return;
            for (int i = 0; i < totalButtons; i++)
            {
                VirtualButton virtualButton = buttonGroup.Find("Button_" + i).GetComponent<VirtualButton>();
                if (i == IndexId)
                    virtualButton.Selected(true);
                virtualButton.ButtonId = i;
                virtualButton.SetText(AlphaBetList[i]);
                ButtonList.Add(virtualButton);
            }
#endif
            IsInputMode = true;
        }

        void ChangeSelected(int lastIndex, int currIndex)
        {
            if (currIndex < 0) currIndex = (currIndex + totalButtons) % totalButtons;
            buttonGroup.Find("Button_" + lastIndex).GetComponent<VirtualButton>().Selected(false);
            buttonGroup.Find("Button_" + currIndex).GetComponent<VirtualButton>().Selected(true);
            IndexId = currIndex;
        }

        private void Disabled()
        {
            inputHud.gameObject.SetActive(false);
            inputField.text = "";
            IsInputMode = false;
        }

        private bool ValidateIPAddress(string ipAddress)
        {
            Regex validPretext =
                new Regex(
                    @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$");
            return (ipAddress != "" && validPretext.IsMatch(ipAddress.Trim())) ? true : false;
        }

        private void CheckInputAndSubmit()
        {
            if (ValidateIPAddress(inputField.text))
            {
                displayHud.SetActive(true);
                PlayerPrefs.SetString(HttpProtocolHandler.OsIpKeyName, inputField.text);
                HttpProtocolHandler.GetInstance().StartWebSocket(PlayerPrefs.GetString(HttpProtocolHandler.OsIpKeyName), isUseJson);
                Invoke(nameof(Disabled), 0.1f);
            }
            else
            {
                Debug.LogError("输入的IP地址有误！");
            }
        }

        void Update()
        {
            if (!inputHud.gameObject.activeSelf) return;
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_STANDALONE_OSX
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                CheckInputAndSubmit();
            }
#else
            if (Input.GetKeyDown(joystickButton14))
            {
                if (!inputHud.activeSelf)
                {
                    inputHud.SetActive(true);
                    displayHud.SetActive(false);
                    IsInputMode = true;
                    return;
                }

                if (IndexId != totalButtons - 1)
                    inputField.text += AlphaBetList[IndexId];
                else
                {
                   CheckInputAndSubmit();
                }

                return;
            }
#endif

            if (Input.GetKeyDown(joystickButton15) || Input.GetKeyDown(KeyCode.Backspace))
            {
                if (inputField.text.Length > 0)
                    inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
            }

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
#if !UNITY_EDITOR && !UNITY_STANDALONE && !UNITY_STANDALONE_OSX
            if (!horizontalDown)
            {
                if (horizontal >= AxisValue || horizontal <= -AxisValue)
                {
                    horizontalDown = true;
                    int nowId = (horizontal >= AxisValue) ? (IndexId + 1) % totalButtons : (IndexId - 1) % totalButtons;
                    ChangeSelected(IndexId, nowId);
                }
            }
            else if (horizontal < AxisValue && horizontal > -AxisValue)
            {
                horizontalDown = false;
            }

            if (!verticalDown)
            {
                if (vertical >= AxisValue || vertical <= -AxisValue)
                {
                    verticalDown = true;
                    int nowId = (vertical <= -AxisValue) ? (IndexId + 4) % totalButtons : (IndexId - 4) % totalButtons;
                    ChangeSelected(IndexId, nowId);
                }
            }
            else if (vertical < AxisValue && vertical > -AxisValue)
            {
                verticalDown = false;
            }
#endif
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHeightUI : MonoBehaviour
{
    [SerializeField] private Button confirmButton;
    [SerializeField] private InputField inputField;

    private Action<int> onFieldInput;
    private const string joystickButton14 = "joystick button 14";
    private void Awake()
    {
        confirmButton.onClick.AddListener(OnComfirmClick);
        inputField.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnDestroy()
    {
        confirmButton.onClick.RemoveListener(OnComfirmClick);
        inputField.onValueChanged.RemoveListener(OnValueChanged);
    }

    public void Initialize(Action<int> onFieldInput)
    {
        this.onFieldInput = onFieldInput;
    }

    public void Show()
    {
        gameObject.SetActive(true);

        inputField.text = null;
        inputField.placeholder.enabled = true;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnEndEdit(string content)
    {
        var number = 0;
        if (int.TryParse(content, out number))
        {
            onFieldInput(number);
        }
    }

    private void OnValueChanged(string content)
    {
    }

    void Update()
    {
        if (!gameObject.activeSelf) return;
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_STANDALONE_OSX
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnEndEdit(inputField.text);
        }
#else
        if (Input.GetKeyDown(joystickButton14))
        {
            OnEndEdit(inputField.text);
        }
        
#endif
}
    private void OnComfirmClick()
    {
        OnEndEdit(inputField.text);
    }
}

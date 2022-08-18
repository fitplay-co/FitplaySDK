using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHeightUI : MonoBehaviour
{
    [SerializeField] private Button confirmButton;
    [SerializeField] private InputField inputField;

    private Action<int> onFieldInput;

    private void Awake() {
        confirmButton.onClick.AddListener(OnComfirmClick);
        inputField.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnDestroy() {
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
        if(int.TryParse(content, out number))
        {
            onFieldInput(number);
        }
    }

    private void OnValueChanged(string content)
    {
    }

    private void OnComfirmClick()
    {
        OnEndEdit(inputField.text);
    }
}

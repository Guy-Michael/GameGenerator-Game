using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandleInput : MonoBehaviour
{
    TMP_InputField text;
    Button button;
    [SerializeField] Color disabledColor;
    [SerializeField] Color enabledColor;
    void Start()
    {
        text = transform.parent.Find("Code Input").GetComponent<TMP_InputField>();
        text.onValueChanged.AddListener((val) => changeButtonColor());
        button = GetComponent<Button>();
        GetComponent<Button>().onClick.AddListener(OnStartButtonClicked);
        button.interactable = false;
    }

    void changeButtonColor()
    {
        button.interactable = text.text.Length > 0;
        var colors = button.colors;
        colors.normalColor = text.text.Length > 0 ? enabledColor : disabledColor;
        colors.normalColor = enabledColor;
        colors.highlightedColor = enabledColor * 1.7f;
        colors.pressedColor = enabledColor * 1.3f;
        colors.selectedColor = colors.highlightedColor;
        button.colors = colors;
    }
    
    void OnStartButtonClicked()
    {
        TMP_InputField text = transform.parent.Find("Code Input").GetComponent<TMP_InputField>();
        GameEvents.GameTypeSelected.Invoke(text.text);
    }
}

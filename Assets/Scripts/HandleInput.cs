using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandleInput : MonoBehaviour
{
    TMP_InputField text;
    Button button;
    void Start()
    {
        text = transform.parent.Find("Code Input").GetComponent<TMP_InputField>();
        button = GetComponent<Button>();
        GetComponent<Button>().onClick.AddListener(OnStartButtonClicked);
    }

    void Update()
    {
        button.interactable = text.text.Length > 0;
    }
    
    void OnStartButtonClicked()
    {
        TMP_InputField text = transform.parent.Find("Code Input").GetComponent<TMP_InputField>();
        GameEvents.GameTypeSelected.Invoke(text.text);
    }
}

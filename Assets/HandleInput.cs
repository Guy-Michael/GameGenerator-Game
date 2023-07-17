using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandleInput : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnStartButtonClicked);
    }
    
    void OnStartButtonClicked()
    {
        TMP_InputField text = transform.parent.Find("Code Input").GetComponent<TMP_InputField>();
        GameEvents.GameTypeSelected.Invoke(text.text);
    }
}

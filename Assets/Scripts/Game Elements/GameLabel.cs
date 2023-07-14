using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameLabel : MonoBehaviour
{
    string value;
    Button button;
    TextMeshProUGUI text;

    public void Init(string value)
    {
        this.value = value;
        button = GetComponentInChildren<Button>();
        text = GetComponentInChildren<TextMeshProUGUI>();

        text.text = value;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.Text;

public class RoundAmountIndicator : MonoBehaviour
{
    Image background;
    TextMeshProUGUI text;

    void Start()
    {
        background = transform.Find("Button").GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        Button button = GetComponentInChildren<Button>();
        button.onClick.AddListener(() => SetBackground(true));
    }

    public void SetBackground(bool selected)
    {   
        string message = selected ? "pressed!" : "unpressed!";
        print(message);
        var backgroundColor = background.color;

        backgroundColor.a = selected ? 1 : 0;
        text.color = selected ? Color.white : Color.black;

        background.color = backgroundColor;
    }
}

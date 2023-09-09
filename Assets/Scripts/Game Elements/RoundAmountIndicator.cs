using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoundAmountIndicator : MonoBehaviour
{
    Image background;
    TextMeshProUGUI text;
    public int value;

    public void Init(Action<RoundAmountIndicator> onSelected)
    {
        background = transform.Find("Button").GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        Button button = GetComponentInChildren<Button>();
        value = int.Parse(text.text);
        
        button.onClick.AddListener(() => onSelected(this));
    }

    public void SetSelected(bool selected)
    {   
        var backgroundColor = background.color;

        backgroundColor.a = selected ? 1 : 0;
        text.color = selected ? Color.white : Color.black;

        background.color = backgroundColor;
    }
}

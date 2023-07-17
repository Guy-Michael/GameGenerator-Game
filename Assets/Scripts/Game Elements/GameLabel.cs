using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GameLabel : MonoBehaviour
{
    Button button;
    TextMeshProUGUI text;
    int index;

    public string Content{get => text?.text;}

    public void Init(int index, Action<int> onLabelClick)
    {
        this.index = index;
        text = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(() => SetLabelSelected(true));
        button.onClick.AddListener(() => onLabelClick(index));
    }

    public void LoadContent(string value)
    {
        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = value;
    }

    public void SetLabelSelected(bool isSelected)
    {
        Image image = transform.Find("Label").GetComponent<Image>();
        image.color = isSelected ? Color.magenta : Color.white;
    }

    public void Disable()
    {
        button.interactable = false;
    }
}

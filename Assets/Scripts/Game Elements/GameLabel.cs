using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GameLabel : MonoBehaviour
{
    Button button;
    TextMeshProUGUI text;
    string content;
    bool lastInteractableState;

    public string Content { get => content; }

    public void Init(Action<GameLabel> onLabelClick)
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(() => SetLabelSelected(true));
        button.onClick.AddListener(() => onLabelClick(this));
        lastInteractableState = true;
        button.interactable = lastInteractableState;
    }

    public void LoadContent(string value)
    {
        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = value;
        this.content = value;
    }

    public void SetLabelSelected(bool isSelected)
    {
        Image image = transform.Find("Label").GetComponent<Image>();
        image.color = isSelected ? Consts.ColorElementHighlight : Consts.ColorLabelDefault;
    }

    public void Reset()
    {
        lastInteractableState = true;
        button.interactable = lastInteractableState;
    }
    
    public void Disable()
    {
        lastInteractableState = false;
        button.interactable = lastInteractableState;
    }

    public void SetInteractable(bool interactable)
    {
        Image image = transform.Find("Label").GetComponent<Image>();
        if(interactable)
        {
            image.color = Color.white;
            text.text = content;
            button.interactable = lastInteractableState;

        }

        else
        {
            image.color = Color.grey;
            text.text = "";
            button.interactable = false;

        }

    }
}

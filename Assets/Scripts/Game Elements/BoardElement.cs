using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.Events;
using TMPro;

public class BoardElement : MonoBehaviour
{
    Dictionary<Player, Sprite> winSprites;
    public Sprite themeSprite {get; private set;}
    private Sprite lastAppliedSprite;
    private bool lastInteractableState;
    Image image;
    Button button;
    GameObject label;

    public string MatchingContent {get => image.sprite.name;}
    public void LoadSprites(Sprite initialSprite, Dictionary<Player, Sprite> winSprites)
    {
        this.themeSprite = initialSprite;

        image = transform.Find("Image").GetComponent<Image>();
        image.sprite = this.themeSprite;
        lastAppliedSprite = this.themeSprite;
        this.winSprites = winSprites;
    }

    private void LoadText(Dictionary<Player, Sprite> winSprites, string content)
    {
        TextMeshProUGUI text = label.GetComponentInChildren<TextMeshProUGUI>();
        if(GameUtils.AssertHebewText(content))
        {
            text.isRightToLeftText = true;
        }

        text.text = content;
    }

    public void Init(Action<BoardElement> onClickCallback)
    {
        button = transform.Find("Image").GetComponent<Button>();
        button.onClick.AddListener(() => SetBorderColorSelected(true));
        button.onClick.AddListener(() => onClickCallback(this));
        lastInteractableState = true;
        button.interactable = lastInteractableState;
    }

    public void SetBorderColorSelected(bool isSelected)
    {
        Image border = transform.Find("Border").GetComponent<Image>();
        border.color = isSelected ? Consts.ColorElementHighlight : Consts.ColorTileDefault;
    }

    public void SetPlayerThumbnail(Player player)
    {
        lastAppliedSprite = winSprites[player];
        image.sprite = lastAppliedSprite;
    }

    public void ResetElement()
    {
        SetBorderColorSelected(false);
        lastAppliedSprite = themeSprite;
        image.sprite = lastAppliedSprite;
        lastInteractableState = true;
        button.interactable = lastInteractableState;
        // label.SetActive(true);
    }

    public void Disable()
    {
        lastInteractableState = false;
        button.interactable = lastInteractableState;
    }

    internal void SetInteractable(bool interactable)
    {
        Image image = transform.Find("Image").GetComponent<Image>();
        
        if(interactable)
        {
            image.color = Color.white;
            // image.sprite = lastAppliedSprite;
            button.interactable = lastInteractableState;
        }

        else
        {
            SetBorderColorSelected(false);
            image.color = Color.grey;
            // image.sprite = null;
            button.interactable = false;
        }

    }
}

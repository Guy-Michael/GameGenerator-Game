using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.Events;
using TMPro;

public class PoolElement : MonoBehaviour
{
    Dictionary<Player, Sprite> winSprites;
    public Sprite themeSprite {get; private set;}
    private Sprite lastAppliedSprite;
    private bool lastInteractableState;
    Image image;
    Button button;
    string matchingContent;
    public string MatchingContent {get => matchingContent; private set {matchingContent = value;}}
    public void LoadSprites(Sprite initialSprite, Dictionary<Player, Sprite> winSprites)
    {
        InitFields(winSprites, initialSprite.name);
        this.themeSprite = initialSprite;
        image = transform.Find("Image").GetComponent<Image>();
        image.sprite = this.themeSprite;
        lastAppliedSprite = this.themeSprite;
    }

    public void LoadText(Dictionary<Player, Sprite> winSprites, string content)
    {
        InitFields(winSprites, content);

        image = transform.Find("Image").GetComponent<Image>();
        image.enabled = false;
        
        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
        GameUtils.FlipTextComponentIfHebrew(text);
        text.text = content;
    }

    private void InitFields(Dictionary<Player, Sprite> winSprites, string content)
    {
        this.winSprites = winSprites;
        MatchingContent = content;
    }

    public void Init(Action<PoolElement> onClickCallback)
    {
        button = GetComponentInChildren<Button>();
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
    
    public void SetBorderColorOnMatch(bool isCorrect)
    {
        Image border = transform.Find("Border").GetComponent<Image>();
        border.color = isCorrect ? Consts.ColorLabelRight : Consts.ColorLabelWrong;
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
    }

    public void Disable()
    {
        lastInteractableState = false;
        button.interactable = lastInteractableState;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    internal void SetInteractable(bool interactable)
    {
        Image image = transform.Find("Image").GetComponent<Image>();
        
        if(interactable)
        {
            SetBorderColorSelected(false);  //To remove correct \ wrong match border colors
            image.color = Color.white;
            image.sprite = lastAppliedSprite;
            button.interactable = lastInteractableState;
        }

        else
        {
            image.color = Color.grey;
            image.sprite = null;
            button.interactable = false;
        }
    }
}

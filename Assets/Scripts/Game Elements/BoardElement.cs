using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.Events;
using TMPro;
using UnityEngine.EventSystems;

public class BoardElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Sprite VSprite;
    [SerializeField] Sprite XSprite;

    Dictionary<Player, Sprite> winSprites;
    public Sprite themeSprite {get; private set;}
    private Sprite lastAppliedSprite;
    private bool lastInteractableState;
    Image image;
    Image matchFeedbackImage;
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
        matchFeedbackImage = transform.Find("Feedback").GetComponent<Image>();
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

    public void SetMatchFeedback(bool isCorrect)
    {
        Image border = transform.Find("Border").GetComponent<Image>();
        border.color = isCorrect ? Consts.ColorLabelRight : Consts.ColorLabelWrong;
        matchFeedbackImage.sprite = isCorrect ? VSprite : XSprite;
        var color = matchFeedbackImage.color;
        color.a = 1;
        matchFeedbackImage.color = color;
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
        HideFeedbackMarker();
    }

    private void HideFeedbackMarker()
    {
        var color = matchFeedbackImage.color;
        color.a = 0;
        matchFeedbackImage.color = color;

        matchFeedbackImage.sprite = null;
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
            SetBorderColorSelected(false); //To remove correct \ wrong match border colors
            image.color = Color.white;
            button.interactable = lastInteractableState;
            HideFeedbackMarker();
        }

        else
        {
            image.color = Color.grey;
            button.interactable = false;
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var rect = GetComponent<RectTransform>();
        rect.localScale *= 1.2f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        var rect= GetComponent<RectTransform>();
        rect.localScale = new Vector3(1, 1, 1);
    }
}

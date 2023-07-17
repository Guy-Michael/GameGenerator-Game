using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.Events;

public class GameTile : MonoBehaviour
{
    Dictionary<Player, Sprite> winSprites;
    Sprite sprite;
    Image image;
    Button button;
    int index;

    public string SpriteName {get => image.sprite.name;}
    public void LoadSprites(Sprite initialSprite, Dictionary<Player, Sprite> winSprites)
    {
        this.sprite = initialSprite;

        image = transform.Find("Image").GetComponent<Image>();
        image.sprite = this.sprite;
        this.winSprites = winSprites;
    }

    public void Init(int index, Action<int> onClickCallback)
    {
        this.index = index;

        button = transform.Find("Image").GetComponent<Button>();
        button.onClick.AddListener(() => SetBorderColorSelected(true));
        button.onClick.AddListener(() => onClickCallback(index));

    }

    public void SetBorderColorSelected(bool isSelected)
    {
        Image border = transform.Find("Border").GetComponent<Image>();
        border.color = isSelected ? Color.magenta : Color.black;
    }

    public void SetPlayerThumbnail(Player player)
    {
        image.sprite = winSprites[player];
    }

    public void Disable()
    {
        button.interactable = false;
    }
}

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
    public void LoadSprites(Sprite initialSprite, Dictionary<Player, Sprite> winSprites)
    {
        this.sprite = initialSprite;

        image = transform.Find("Image").GetComponent<Image>();
        this.winSprites = winSprites;
    }

    public void Init(Action<int> onClickCallback)
    {
        button = transform.Find("Image").GetComponent<Button>();

        InitIndex();

        button.onClick.AddListener(ChangeBorderColorToSelected);
        button.onClick.AddListener(() => onClickCallback(index));
        image.sprite = this.sprite;

    }

    private void InitIndex()
    {
        string indexPortionOfName = gameObject.name.Split(" ")[1];
        index = int.Parse(indexPortionOfName);
    }

    private void ChangeBorderColorToSelected()
    {
        Image border = transform.Find("Border").GetComponent<Image>();
        border.color = Color.magenta;
    }

    public void SetPlayerThumbnail(Player player)
    {
        image.sprite = winSprites[player];
    }
}

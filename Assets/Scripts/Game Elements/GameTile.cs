using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameTile : MonoBehaviour
{
    Sprite sprite;
    Image image;
    public void Init(Sprite initialSprite)
    {
        this.sprite = initialSprite;

        image = transform.Find("Image").GetComponent<Image>();
        image.sprite = this.sprite;
    }
}

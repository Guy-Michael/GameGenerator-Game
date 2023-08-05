using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InputValidator : MonoBehaviour
{
    [SerializeField] Sprite activeSprite;
    [SerializeField] Sprite inactiveSprite;

    Image image;
    new RectTransform transform;
    Button button;

    Vector2 activeScale = new(246, 112);
    Vector2 inactiveScale = new(224, 87);

    void Start()
    {
        image = GetComponent<Image>();
        transform = GetComponent<RectTransform>();
        button = GetComponent<Button>();
    }

    public void SetButtonActive(bool active)
    {
        if(active)
        {
            image.sprite = activeSprite;
            transform.sizeDelta = activeScale;
            button.interactable = true;
        }

        else
        {
            button.interactable = false;
            image.sprite = inactiveSprite;
            transform.sizeDelta = inactiveScale;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenMatch : MonoBehaviour
{
    [SerializeField] Color correctMatchColor;
    [SerializeField] Color incorrectMatchColor;
    [SerializeField] bool isCorrect;
    Image image;
    Image border;
    TextMeshProUGUI caption;
    public void Init(Sprite sprite, string text, bool correct)
    {
        image = transform.Find("Image").GetComponent<Image>();
        image.sprite = sprite;

        caption = transform.Find("Caption").GetComponentInChildren<TextMeshProUGUI>();
        caption.text = text;

        isCorrect = correct;
        ChangeBorderColorByMatchCorrectness();


        border = transform.Find("Border").GetComponent<Image>();
    }

    private void ChangeBorderColorByMatchCorrectness()
    {
        if(border == null)
        {
            border = transform.Find("Border").GetComponent<Image>();
        }

        border.color = isCorrect ? correctMatchColor : incorrectMatchColor;
    }
}

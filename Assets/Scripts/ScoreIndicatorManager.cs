using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class ScoreIndicatorManager : MonoBehaviour
{
    [SerializeField] Sprite winSprite;
    [SerializeField] Sprite tie;
    List<Image> setIndicators;
    Player player;
    int currentSet;
    public bool gameWon;
    
    public void Init()
    {
        player = GetComponentInParent<Character>().player;
        currentSet = 0;

        setIndicators = new();
        foreach(Transform child in transform)
        {
            setIndicators.Add(child.transform.Find("Image").GetComponent<Image>());
        }
    }

    public void IncrementScore(SetOutcome outcome)
    {
        if(outcome == SetOutcome.Tie)
        {
            setIndicators[currentSet].sprite = tie;
        }

        else if(outcome == player.ToOutcome())
        {
            setIndicators[currentSet].sprite = winSprite;
        }

        Color color = setIndicators[currentSet].color;
        color.a = 255;
        setIndicators[currentSet].color = color;

        currentSet++;

        if(currentSet >= 2)
        {

            InvokeGameWonEvent();
        }
    }
    
    private async void InvokeGameWonEvent()
    {
        gameWon = true;
        await GameEvents.GameWon.Invoke();
    }
}
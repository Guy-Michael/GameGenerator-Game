using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class ScoreIndicatorManager : MonoBehaviour
{
    [SerializeField] Sprite alienWinSprite;
    [SerializeField] Sprite astronautWinSprite;
    [SerializeField] Sprite tieSprite;
    List<(Image image, Image background)> setIndicators;
    Dictionary<SetOutcome, Sprite> outcomes;
    int currentSet;
    public bool gameWon;
    
    public void Init()
    {
        currentSet = 0;

        outcomes = new()
        {
            {SetOutcome.AlienWin, alienWinSprite},
            {SetOutcome.AstronautWin, astronautWinSprite},
            {SetOutcome.Tie, tieSprite}
        };

        setIndicators = new();
        foreach(Transform child in transform)
        {
            setIndicators.Add((child.transform.Find("Image").GetComponent<Image>(), child.transform.Find("Background").GetComponent<Image>()));
        }
    }

    public void IncrementPlayerScore(SetOutcome outcome)
    {
        setIndicators[currentSet].image.sprite = outcomes[outcome];

        Color color = setIndicators[currentSet].image.color;
        color.a = 255;
        setIndicators[currentSet].image.color = color;

        Color colorBackground = setIndicators[currentSet].background.color;
        colorBackground.a = 255;
        setIndicators[currentSet].background.color = colorBackground;

        currentSet++;

        if(currentSet >= 2)
        {
            gameWon = true;
            InvokeGameWonEvent();
        }
    }
    
    private async void InvokeGameWonEvent()
    {
        gameWon = true;
        await GameEvents.GameEnded.Invoke();
    }
}
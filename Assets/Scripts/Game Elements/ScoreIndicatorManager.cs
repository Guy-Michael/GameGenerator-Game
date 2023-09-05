using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class ScoreIndicatorManager : MonoBehaviour
{
    [SerializeField] Sprite alienWinSprite;
    [SerializeField] Sprite astronautWinSprite;
    [SerializeField] Sprite tieSprite;
    List<Image> setIndicators;
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
            setIndicators.Add(child.transform.Find("Image").GetComponent<Image>());
        }
    }

    public void IncrementPlayerScore(SetOutcome outcome)
    {
        print(outcomes[outcome]);
        print(setIndicators[currentSet]);
        setIndicators[currentSet].sprite = outcomes[outcome];
        print(outcomes[outcome].name);

        // if(outcome == SetOutcome.Tie)
        // {
        //     setIndicators[currentSet].sprite = tieSprite;
        // }

        // else if(outcome == player.ToOutcome())
        // {
        //     setIndicators[currentSet].sprite = alienWinSprite;
        // }

        Color color = setIndicators[currentSet].color;
        color.a = 255;
        setIndicators[currentSet].color = color;

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
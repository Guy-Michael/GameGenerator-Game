using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoundsManager : MonoBehaviour
{
    [SerializeField] Sprite alienWinSprite;
    [SerializeField] Sprite astronautWinSprite;
    [SerializeField] Sprite tieSprite;
    List<(Image image, Image background)> setIndicators;
    Dictionary<SetOutcome, Sprite> outcomeSprites;
    Dictionary<SetOutcome, int> scores;
    int setsCompletedCount;
    int maxSetsCount;
    public bool gameWon;
    
    public void Init()
    {
        setsCompletedCount = 0;
        maxSetsCount = AnalyticsManager.numberOfRounds;

        outcomeSprites = new()
        {
            {SetOutcome.AlienWin, alienWinSprite},
            {SetOutcome.AstronautWin, astronautWinSprite},
            {SetOutcome.Tie, tieSprite}
        };

        scores = new()
        {
            {SetOutcome.AlienWin, 0},
            {SetOutcome.AstronautWin, 0},
            {SetOutcome.Tie, 0}
        };

        setIndicators = new();
        foreach(Transform child in transform)
        {   
            setIndicators.Add((child.transform.Find("Image").GetComponent<Image>(), child.transform.Find("Background").GetComponent<Image>()));
        }

        foreach((Image image, Image background) indicator in setIndicators)
        {
            indicator.image.transform.parent.gameObject.SetActive(false);
        }

        for(int i = 0; i < maxSetsCount; i++)
        {
            setIndicators[i].image.transform.parent.gameObject.SetActive(true);
        }
    }

    public void IncrementPlayerScore(SetOutcome outcome)
    {
        scores[outcome]++;
        setIndicators[setsCompletedCount].image.sprite = outcomeSprites[outcome];

        Color color = setIndicators[setsCompletedCount].image.color;
        color.a = 255;
        setIndicators[setsCompletedCount].image.color = color;

        Color colorBackground = setIndicators[setsCompletedCount].background.color;
        colorBackground.a = 255;
        setIndicators[setsCompletedCount].background.color = colorBackground;

        setsCompletedCount++;

        if(setsCompletedCount >= maxSetsCount)
        {
            gameWon = true;
            InvokeGameEndedEvent();
        }
    }

    private SetOutcome CalculateFinalOutcome()
    {
        if(scores[SetOutcome.AlienWin] == scores[SetOutcome.AstronautWin])
        {
            return SetOutcome.Tie;
        }

        return scores[SetOutcome.AlienWin] > scores[SetOutcome.AstronautWin] ? SetOutcome.AlienWin : SetOutcome.AstronautWin;
    }
    
    private async void InvokeGameEndedEvent()
    {
        gameWon = true;
        print("invoking game ended from rounds");
        SetOutcome outcome = CalculateFinalOutcome();
        await GameEvents.GameEnded.Invoke(outcome);
    }
}
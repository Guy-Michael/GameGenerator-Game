using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class VisualFeedback : MonoBehaviour
{
    [SerializeField] Image spaceship;
    [SerializeField] TextMeshProUGUI caption;
    [SerializeField] Sprite alienWinSprite;
    [SerializeField] Sprite astronautWinSprite;

    void Start()
    {
        SetOutcome outcome = AnalyticsManager.outcome;
        string name = string.Empty;
        Sprite winSprite = null;

        switch(outcome)
        {
            case SetOutcome.AlienWin:
            {
                name = AnalyticsManager.analytics[Player.Alien].name;
                winSprite = alienWinSprite;        
                break;
            }
            
            case SetOutcome.AstronautWin:
            {
                name = AnalyticsManager.analytics[Player.Astronaut].name;
                winSprite = astronautWinSprite;
                break;
            }
        }

        string outcomeCaption = TextConsts.GameFeedbackText.GenerateWinningText(name);
        caption.text = outcomeCaption;
        caption.isRightToLeftText = false;
        spaceship.sprite = winSprite;
    }
}

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
    [SerializeField] Sprite tieSprite;

    void Start()
    {
        SetOutcome outcome = AnalyticsManager.outcome;
        string outcomeCaption = string.Empty;
        Sprite outcomeSprite = null;
        switch(outcome)
        {
            case SetOutcome.AlienWin:
            {
                name = AnalyticsManager.analytics[Player.Alien].name;
                outcomeSprite = alienWinSprite;        
                outcomeCaption = TextConsts.GameFeedbackText.GenerateWinningText(name);
                break;
            }
            
            case SetOutcome.AstronautWin:
            {
                name = AnalyticsManager.analytics[Player.Astronaut].name;
                outcomeSprite = astronautWinSprite;
                outcomeCaption = TextConsts.GameFeedbackText.GenerateWinningText(name);
                break;
            }

            case SetOutcome.Tie:
            {
                outcomeCaption = TextConsts.GameFeedbackText.TieCaption;
                outcomeSprite = tieSprite;
                spaceship.rectTransform.anchoredPosition = new Vector2(1, 3.5f);
                spaceship.rectTransform.sizeDelta = new Vector2(870, 875);
                break;
            }
        }

        caption.text = outcomeCaption;
        caption.isRightToLeftText = false;
        spaceship.sprite = outcomeSprite;
    }
}

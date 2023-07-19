using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreIndicatorManager : MonoBehaviour
{
    [SerializeField] Sprite player1;
    [SerializeField] Sprite player2;
    [SerializeField] Sprite tie;
    Dictionary<Player, int> roundScore;
    Dictionary<Player, Sprite> setSprites;
    Dictionary<Player, TextMeshProUGUI> roundScoreIndicators;
    List<Image> setIndicators;
    int currentSet;
    void Start()
    {
        roundScore = new();
        roundScore[Player.Player1] = 0;
        roundScore[Player.Player2] = 0;
        
        setSprites = new();
        setSprites[Player.Player1] = player1;
        setSprites[Player.Player2] = player2;
        setSprites[Player.Tie] = tie;
        currentSet = 0;

        roundScoreIndicators = new();
        roundScoreIndicators[Player.Player1] = transform.Find("Round Indicators/Player 1 Score/Current").GetComponent<TextMeshProUGUI>();
        roundScoreIndicators[Player.Player2] = transform.Find("Round Indicators/Player 2 Score/Current").GetComponent<TextMeshProUGUI>();

        setIndicators = new();
        foreach(Transform child in transform.Find("Set Indicators").transform)
        {
            setIndicators.Add(child.transform.Find("Image").GetComponent<Image>());
        }
    }

    public void IncrementScore(Player player)
    {
        roundScore[player]++;
        roundScoreIndicators[player].text = roundScore[player].ToString();

        if(roundScore[player] >= 3)
        {
            roundScore[Player.Player1] = 0;
            roundScore[Player.Player2] = 0;
            WinSet(player);
            // GameEvents.SetWon.Invoke();
        } 
    }

    private void WinSet(Player player)
    {
        setIndicators[currentSet].sprite = setSprites[player];
        Color color = setIndicators[currentSet].color;
        color.a = 255;
        setIndicators[currentSet].color = color;

        currentSet++;

        if(currentSet > 2)
        {
            InvokeGameWonEvent();
        }
    }

    private void InvokeGameWonEvent()
    {
        GameEvents.GameWon.Invoke();
    }
}
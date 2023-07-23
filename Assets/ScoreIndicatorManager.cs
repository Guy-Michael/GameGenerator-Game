using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreIndicatorManager : MonoBehaviour
{
    [SerializeField] Sprite astronaut;
    [SerializeField] Sprite alien;
    [SerializeField] Sprite tie;
    Dictionary<Player, int> roundScore;
    Dictionary<Player, Sprite> setSprites;
    Dictionary<Player, TextMeshProUGUI> roundScoreIndicators;
    List<Image> setIndicators;
    int currentSet;
    void Start()
    {
        roundScore = new();
        roundScore[Player.Astronaut] = 0;
        roundScore[Player.Alien] = 0;
        
        setSprites = new();
        setSprites[Player.Astronaut] = astronaut;
        setSprites[Player.Alien] = alien;
        setSprites[Player.Tie] = tie;
        currentSet = 0;

        roundScoreIndicators = new();
        roundScoreIndicators[Player.Astronaut] = transform.Find("Round Indicators/Astronaut Score/Current").GetComponent<TextMeshProUGUI>();
        roundScoreIndicators[Player.Alien] = transform.Find("Round Indicators/Alien Score/Current").GetComponent<TextMeshProUGUI>();

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
            roundScore[Player.Astronaut] = 0;
            roundScore[Player.Alien] = 0;
            WinSet(player);
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
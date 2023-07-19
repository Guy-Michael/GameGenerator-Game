using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class EndScreenPlayerData : MonoBehaviour
{
    internal void Init(string name, int score, int numberOfMistakes, float playTime)
    {
        var nameText = transform.Find("Name").GetComponent<TextMeshProUGUI>();
        var scoreText = transform.Find("Grade").GetComponent<TextMeshProUGUI>();
        var mistakesText = transform.Find("Mistake Count").GetComponent<TextMeshProUGUI>();
        var playTimeText = transform.Find("Play Time").GetComponent<TextMeshProUGUI>();
    
        nameText.text = name;
        scoreText.text = $"ציון - {reverseNumberForDisplay(score)}";
        mistakesText.text = $"מספר טעויות - {reverseNumberForDisplay(numberOfMistakes)}";
        playTimeText.text = $"זמן - {reverseNumberForDisplay((int) playTime)} שניות";
    }

    private string reverseNumberForDisplay(int number)
    {
        string str = number.ToString();
        List<char> chars = str.ToList();
        chars.Reverse();
        return string.Join("", chars);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenManager : MonoBehaviour
{
    void Start()
    {
        var analytics = AnalyticsManager.analytics;
        print($"score: {analytics[Player.Player1].score}, play time: {analytics[Player.Player1].playTime}, mistakes: {analytics[Player.Player1].numberOfMistakes}");
    }
}

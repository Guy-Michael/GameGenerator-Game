using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerAnalytics
{
    public List<(Sprite sprite, string Caption, bool isCorrect)> moves;
    public int numberOfMistakes;
    public float playTime;
    public string name;
    public int score;
}
public static class AnalyticsManager
{
    public static Dictionary<Player, PlayerAnalytics> analytics;
    private static TimerHandler timerHandlerInstance;
    private static TimerHandler TimerHandlerInstance
    {
        get 
        {
            if(timerHandlerInstance == null)
            {
                timerHandlerInstance =  GameObject.Find("Timer").GetComponent<TimerHandler>();
            }

            return timerHandlerInstance;
        }
    }
    static AnalyticsManager()
    {
        PlayerAnalytics temp = new();
        temp.moves = new();

        analytics = new();
        analytics.Add(Player.Astronaut, temp);
        analytics.Add(Player.Alien, temp);
    }

    public static void IncrementNumberOfMistakes(Player player)
    {
        SetProperty(player, (analytics) =>
        {
            analytics.numberOfMistakes += 1;
            return analytics;
        });
    }

    public static void IncrementPlaytime(Player player)
    {
        SetProperty(player, (analytics) =>
        {
            float timeToIncrementBy = TimerHandlerInstance.GetElapsedTime();
            analytics.playTime  += timeToIncrementBy;
            return analytics;
        });
    }

    public static void IncrementScore(Player player)
    {
        SetProperty(player, (analytics) =>
        {
            analytics.score += 10;
            return analytics;
        });
    }

    public static void SetPlayerName(Player player, string name)
    {
        SetProperty(player, (analytics) =>
        {
            analytics.name = name;
            return analytics;
        });
    }

    private static void SetProperty(Player player, Func<PlayerAnalytics, PlayerAnalytics> setter)
    {
        var a = analytics[player];
        a = setter(a); 
        analytics[player] = a;
    }

    public static void RecordMove(Player player, string caption, Sprite sprite, bool isMoveCorrect)
    {
        analytics[player].moves.Add((sprite, caption, isMoveCorrect));
    }


}

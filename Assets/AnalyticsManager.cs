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
    private static TimerHandler timerHandler;
    
    static AnalyticsManager()
    {
        Debug.Log(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + ": analytics constructed");
        PlayerAnalytics temp = new();
        temp.moves = new();

        analytics = new();
        analytics.Add(Player.Player1, temp);
        analytics.Add(Player.Player2, temp);

        if(SceneTransitionManager.IsCurrentSceneGame)
        {
            timerHandler = GameObject.Find("Timer").GetComponent<TimerHandler>();
        }
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
            float timeToIncrementBy = timerHandler.GetElapsedTime();
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

        Debug.Log(analytics[player].name);
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

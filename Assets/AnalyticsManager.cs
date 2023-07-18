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
        PlayerAnalytics temp = new();
        temp.moves = new();

        analytics = new();
        analytics.Add(Player.Player1, temp);
        analytics.Add(Player.Player2, temp);

        timerHandler = GameObject.Find("Timer").GetComponent<TimerHandler>();

    }

    public static void IncrementNumberOfMistakes(Player player)
    {
        PlayerAnalytics a = analytics[player];
        a.numberOfMistakes++;
        analytics[player] = a;
    }

    public static void IncrementPlaytime(Player player)
    {
        PlayerAnalytics a = analytics[player];
        a.playTime += timerHandler.GetElapsedTime();
        analytics[player] = a;
    }

    public static void IncrementScore(Player player)
    {
        PlayerAnalytics a = analytics[player];
        a.score += 10;
        analytics[player] = a;
    }

    public static void UpdateName(Player player)
    {

    }


}

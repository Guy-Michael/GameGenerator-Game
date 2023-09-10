using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using UnityEditor.PackageManager.Requests;
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
            timerHandlerInstance = timerHandlerInstance ?? GameObject.Find("Timer").GetComponent<TimerHandler>();
            return timerHandlerInstance;
        }
    }
    public static string gameCode = "100";
    public static int numberOfRounds;
    public static SetOutcome outcome;
    static AnalyticsManager()
    {
        Reset();
    }

    public static void Reset()
    {
        analytics = new()
        {
            {
                Player.Alien, 
                new PlayerAnalytics()
                {
                    name = TextConsts.defaultPlayerNames[Player.Alien],
                    moves = new()
                }
            },
            {
                Player.Astronaut, 
                new PlayerAnalytics()
                {
                    name = TextConsts.defaultPlayerNames[Player.Astronaut],
                    moves = new()
                }
            }
        };
        SetPlayerName(Player.Astronaut, TextConsts.defaultPlayerNames[Player.Astronaut]);
        SetPlayerName(Player.Alien, TextConsts.defaultPlayerNames[Player.Alien]);
        SetNumberOfRounds(1);
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

    public static void SetNumberOfRounds(int amount)
    {
        numberOfRounds = amount;
    }
}

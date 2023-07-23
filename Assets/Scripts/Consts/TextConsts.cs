using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextConsts
{
    public static Dictionary<Player, string> defaultPlayerNames;
    public static string playerWonTurn = "תשובה נכונה!".Reverse();
    public static string playerWonSet = "ניצחת בסיבוב הזה!".Reverse() +  "\n" + "עוד קצת ונחזור הביתה".Reverse();
    public static string playerLostTurn = "לא נורא,".Reverse() + "\n" + "אולי בפעם הבאה!".Reverse();

    static TextConsts()
    {
        defaultPlayerNames = new();
        defaultPlayerNames[Player.Alien] = "Bob".Reverse();
        defaultPlayerNames[Player.Astronaut] = "Alice".Reverse();
    }
}

public static class StringExtensions
{
    public static string Reverse(this string str)
    {
        string res = "";
        for(int i = str.Length - 1; i >= 0; i--)
        {
            res += str[i];
        }

        return res;
    }
}
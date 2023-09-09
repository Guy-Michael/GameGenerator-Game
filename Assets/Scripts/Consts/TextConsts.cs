using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class TextConsts
{
    public static Dictionary<Player, string> defaultPlayerNames;   
    
    public static class TurnFeedbackText
    {
        public static readonly string playerWonTurn = "תשובה נכונה!".Reverse();
        public static readonly string playerWonSet = "ניצחת בסיבוב הזה!".Reverse() +  "\n" + "עוד קצת ונחזור הביתה".Reverse();
        public static readonly string playerLostTurn =  "לא נורא,".Reverse() + "\n" + "אולי בפעם הבאה!".Reverse();
    };

    public static class GameFeedbackText
    {
        public static readonly string TieCaption = "תיקו!".Reverse()
                                                    + "\n"
                                                    + "לא הצלחתם לעלות לחללית..".Reverse()
                                                    + "\n"
                                                    + "שחקו שוב על מנת לחזור הביתה".Reverse(); 
       
        public static string GenerateWinningText(string name)
        {
            return $"כל הכבוד {name}!".Reverse()
                    + "\n".Reverse()
                    + "בהצלחה בדרך הביתה".Reverse();
        }
    }

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
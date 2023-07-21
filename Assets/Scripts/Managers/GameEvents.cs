using UnityEngine.Events;

public static class GameEvents
{
    public static UnityEvent PlayerGotMatch = new();
    public static UnityEvent PlayerFailedMatch = new();
    public static UnityEvent TurnEnded = new();
    public static UnityEvent GameStarted = new();
    public static UnityEvent<string> GameTypeSelected = new();
    public static UnityEvent RoundEnded = new();
    public static UnityEvent SetWon = new();
    public static UnityEvent GameWon = new();
}
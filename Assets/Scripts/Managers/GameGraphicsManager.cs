using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class GameGraphicsManager
{
    private static Dictionary<Player, Character> playersInGame;
    private static Dictionary<Player, Character> playersInPre;
    
    static GameGraphicsManager()
    {
        InitGameCharacters();
        InitPreCharacters();
    }

    private static void InitPreCharacters()
    {
        playersInPre = new();

        string rootPath = "First Player Decleration Elements";
        Character astronaut = GameObject.Find($"{rootPath}/Astronaut").GetComponent<Character>();
        Character alien = GameObject.Find($"{rootPath}/Alien").GetComponent<Character>();

        playersInPre.Add(Player.Astronaut, astronaut);
        playersInPre.Add(Player.Alien, alien);

    }

    private static void InitGameCharacters()
    {
        playersInGame = new();

        Character astronaut = GameObject.Find("Game/Astronaut").GetComponent<Character>();
        Character alien = GameObject.Find("Game/Alien").GetComponent<Character>();

        playersInGame.Add(Player.Astronaut, astronaut);
        playersInGame.Add(Player.Alien, alien);
    }

    public static void SetFirstPlayer(Player firstPlayer)
    {
        playersInPre[firstPlayer].SetSprite(PlayerState.Active);
        playersInPre[firstPlayer.Other()].SetSprite(PlayerState.Lost);
    }

    public static void SetPlayerSpriteOnTurnEnd(Player player, PlayerState state)
    {
        playersInGame[player].SetSprite(state);
    }

    public static void SetActivePlayerTint(Player activePlayer)
    {
        playersInGame[activePlayer].SetActiveInGame(true);
        playersInGame[activePlayer.Other()].SetActiveInGame(false);
    }

    public static void ResetPlayersSprites()
    {
        playersInGame[Player.Alien].SetSprite(PlayerState.Idle);
        playersInGame[Player.Astronaut].SetSprite(PlayerState.Idle);
    }
}

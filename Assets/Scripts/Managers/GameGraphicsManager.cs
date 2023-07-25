using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class GameGraphicsManager
{
    private static Dictionary<Player, Character> playersInGame;
    
    static GameGraphicsManager()
    {
        InitGameCharacters();
    }

    private static void InitGameCharacters()
    {
        playersInGame = new();

        Character astronaut = GameObject.Find("Game/Astronaut").GetComponent<Character>();
        Character alien = GameObject.Find("Game/Alien").GetComponent<Character>();

        playersInGame.Add(Player.Astronaut, astronaut);
        playersInGame.Add(Player.Alien, alien);
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

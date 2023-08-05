using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameGraphicsManager : MonoBehaviour
{
    private static Dictionary<Player, Character> playersInGame;

    public void InitGameCharacters()
    {
        playersInGame = new();

        Character astronaut = GameObject.Find("Game/Astronaut").GetComponent<Character>();
        Character alien = GameObject.Find("Game/Alien").GetComponent<Character>();

        playersInGame.Add(Player.Astronaut, astronaut);
        playersInGame.Add(Player.Alien, alien);
    }

    public void SetPlayerSpriteOnTurnEnd(Player player, PlayerState state)
    {
        playersInGame[player].SetSprite(state);
    }

    public void SetActivePlayerTint(Player activePlayer)
    {
        playersInGame[activePlayer].SetActiveInGame(true);
        playersInGame[activePlayer.Other()].SetActiveInGame(false);
    }

    public void ResetPlayersSprites()
    {
        playersInGame[Player.Alien].SetSprite(PlayerState.Idle);
        playersInGame[Player.Astronaut].SetSprite(PlayerState.Idle);
    }
}

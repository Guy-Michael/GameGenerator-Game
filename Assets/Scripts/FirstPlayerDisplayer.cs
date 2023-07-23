using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPlayerDisplayer : MonoBehaviour
{
    Dictionary<Player, Character> players;
    public void Init(Player firstPlayer)
    {
        Character astronaut = transform.Find("Astronaut").GetComponent<Character>();
        Character alien = transform.Find("Alien").GetComponent<Character>();

        players = new();
        players.Add(Player.Astronaut, astronaut);
        players.Add(Player.Alien, alien);

        players[firstPlayer].SetSprite(PlayerState.Active);
        players[firstPlayer.Other()].SetSprite(PlayerState.Lost);

        TMPro.TextMeshProUGUI FirstPlayerName = GameObject.Find("Captions/Player Name").GetComponent<TMPro.TextMeshProUGUI>();
        FirstPlayerName.text = AnalyticsManager.analytics[firstPlayer].name;
    }

}

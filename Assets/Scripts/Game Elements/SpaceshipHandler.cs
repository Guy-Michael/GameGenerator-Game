using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpaceshipHandler : MonoBehaviour
{
    [SerializeField] RectTransform[] players;
    RectTransform spaceshipRectTransform;
    Dictionary<Player, RectTransform> playerRectTransforms;
    TextMeshProUGUI turnEndMessage;
    int selectedPlayerIndex = 0;
    
    void Start()
    {
        spaceshipRectTransform = GetComponent<RectTransform>();
        
        playerRectTransforms = new();
        playerRectTransforms.Add(Player.Astronaut, players[0]);
        playerRectTransforms.Add(Player.Alien, players[1]);

        turnEndMessage = transform.Find("Turn End Message").GetComponent<TextMeshProUGUI>();
    }

    public void SetActivePlayer(Player player)
    {
        selectedPlayerIndex = ((int)player);
        MovePositionToAboveCurrentPlayer();
    }

    public void ToggleActivePlayer()
    {
        selectedPlayerIndex = (selectedPlayerIndex + 1) % players.Length;
        MovePositionToAboveCurrentPlayer();
    }

    private void MovePositionToAboveCurrentPlayer()
    {
        float nextPlayerX = players[selectedPlayerIndex].anchoredPosition.x;
        float currentY = spaceshipRectTransform.anchoredPosition.y;
        spaceshipRectTransform.anchoredPosition = new Vector2(nextPlayerX, currentY);
    }

    public void SetTurnEndMessage(bool hasWon)
    {
        turnEndMessage.text = hasWon ? TextConsts.TurnFeedbackText.playerWonTurn : TextConsts.TurnFeedbackText.playerLostTurn;
    }

    public void ResetTurnEndMessage()
    {
        turnEndMessage.text = "";
    }

    internal void DisplayWonMessage()
    {
        turnEndMessage.text = TextConsts.TurnFeedbackText.playerWonSet;
    }
}

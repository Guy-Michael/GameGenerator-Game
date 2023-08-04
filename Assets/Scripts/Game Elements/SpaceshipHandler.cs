using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpaceshipHandler : MonoBehaviour
{
    [SerializeField] RectTransform[] players;
    RectTransform spaceshipRectTransform;
    TextMeshProUGUI turnEndMessage;
    Button continueButton;
    int selectedPlayerIndex = 0;
    
    public void Init(Action onContinueClicked)
    {
        spaceshipRectTransform = GetComponent<RectTransform>();
        turnEndMessage = transform.Find("Turn End Message").GetComponent<TextMeshProUGUI>();
        continueButton = GetComponentInChildren<Button>();
        continueButton.onClick.AddListener(()=>onContinueClicked());
        SetContinueButtonVisible(false);
    }

    public void SetActivePlayer(Player player)
    {
        selectedPlayerIndex = ((int)player);
        MovePositionToAboveCurrentPlayer();
        SetButtonInitialSide(player);
    }

    public void ToggleActivePlayer()
    {
        selectedPlayerIndex = (selectedPlayerIndex + 1) % players.Length;
        MovePositionToAboveCurrentPlayer();
        ToggleButtonSide();
    }

    private void MovePositionToAboveCurrentPlayer()
    {
        float nextPlayerX = players[selectedPlayerIndex].anchoredPosition.x;
        float currentY = spaceshipRectTransform.anchoredPosition.y;
        spaceshipRectTransform.anchoredPosition = new Vector2(nextPlayerX, currentY);

    }


    private void SetButtonInitialSide(Player player)
    {
        RectTransform buttonRectTransform = continueButton.GetComponent<RectTransform>();
        Vector2 continueButtonPosition = buttonRectTransform.anchoredPosition;
        
        continueButtonPosition.x = Mathf.Abs(continueButtonPosition.x);
        continueButtonPosition.x *= player == Player.Alien ? 1 : -1;
        buttonRectTransform.anchoredPosition = continueButtonPosition;
    }

    private void ToggleButtonSide()
    {
        RectTransform buttonRectTransform = continueButton.GetComponent<RectTransform>();
        Vector2 continueButtonPosition = buttonRectTransform.anchoredPosition;
        continueButtonPosition.x *= -1;
        buttonRectTransform.anchoredPosition = continueButtonPosition;
    }

    public void SetContinueButtonVisible(bool isVisible)
    {
        continueButton.gameObject.SetActive(isVisible);
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

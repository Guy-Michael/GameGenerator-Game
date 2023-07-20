using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerSetupManager : MonoBehaviour
{
    [SerializeField] TMP_InputField player1InputField;
    [SerializeField] TMP_InputField player2InputField;
    [SerializeField] Button startButton;
    
    void Start()
    {
        startButton.onClick.AddListener(OnGameStart); 
    }

    void OnGameStart()
    {
        AnalyticsManager.SetPlayerName(Player.Player1, player1InputField.text);
        AnalyticsManager.SetPlayerName(Player.Player2, player2InputField.text);
        SceneTransitionManager.MoveToGameScene();
    }
}

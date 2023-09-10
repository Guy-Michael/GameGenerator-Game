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
    InputValidator validator;
    RoundsIndicatorManager roundAmountManager;
    void Start()
    {
        AnalyticsManager.Reset();
        validator = startButton.GetComponent<InputValidator>();
        roundAmountManager = GameObject.Find("Rounds Container").GetComponent<RoundsIndicatorManager>();
        startButton.onClick.AddListener(OnGameStart); 
    }

    void Update()
    {
        validator.SetButtonActive(IsInputValid());
    }

    void OnGameStart()
    {
        if(!IsInputValid()) return;

        AnalyticsManager.SetPlayerName(Player.Astronaut, player1InputField.text);
        AnalyticsManager.SetPlayerName(Player.Alien, player2InputField.text);
        AnalyticsManager.SetNumberOfRounds(roundAmountManager.currentNumberOfRounds);
        SceneTransitionManager.MoveToScene(SceneNames.Game);
    }

    bool IsInputValid()
    {
        return player1InputField.text.Length > 0 && 
                player2InputField.text.Length > 0 &&
                roundAmountManager.currentNumberOfRounds != 0;

    }
}

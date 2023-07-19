using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum Player
{
    Player1,
    Player2
}

public class GameManager : MonoBehaviour
{
    GameLoader contentLoader;
    TileManager gameBoard;
    LabelManager words;
    public static Player currentPlayer;
    Dictionary<Player, List<int>> movesMade;
    public static Dictionary<Player, (Sprite sprite, string caption, bool isCorrect)> analyticsMoveRecords;
    SpaceshipHandler spaceship;
    int lastSelectedTileIndex;
    int lastSelectedLabelIndex;

    void Start()
    {

        //This should be randomized.
        currentPlayer = Player.Player1;
        
        movesMade = new();
        movesMade.Add(Player.Player1, new List<int>());
        movesMade.Add(Player.Player2, new List<int>());

        lastSelectedLabelIndex = -1;
        lastSelectedTileIndex = -1;

        //BAD WAY TO SEARCH, SHOULD CHANGE. MAYBE USE SERIALIZED FIELDS
        gameBoard = GameObject.Find("Tiles").GetComponent<TileManager>();
        gameBoard.InitTiles(OnTileClick);
        
        words = GameObject.Find("Labels").GetComponent<LabelManager>();
        words.Init(OnLabelClick);

        spaceship = GameObject.Find("Spaceship").GetComponent<SpaceshipHandler>();

        contentLoader = GetComponent<GameLoader>();
        IAssetImporter importer = GetComponent<LocalAssetImporter>();
        contentLoader.InitializeGameGraphics(importer);


        GameEvents.PlayerGotMatch.AddListener(OnPlayerGotMatch);
        GameEvents.PlayerFailedMatch.AddListener(OnPlayerFailedMatch);
        GameEvents.TurnEnded.AddListener(OnTurnEnded);
        GameEvents.GameWon.AddListener(OnGameWon);

        GameEvents.GameStarted.Invoke();
    }

    

    void OnTileClick(int tileIndex)
    {
        if(lastSelectedTileIndex != -1)
        {
            gameBoard[lastSelectedTileIndex].SetBorderColorSelected(false);
        }

        lastSelectedTileIndex = tileIndex;
        CheckForMatch();
    }

    void OnLabelClick(int labelIndex)
    {
        if(lastSelectedLabelIndex != -1)
        {
            words[lastSelectedLabelIndex].SetLabelSelected(false);
        }
        lastSelectedLabelIndex = labelIndex;
        CheckForMatch();
    }

    void CheckForMatch()
    {
        if(lastSelectedLabelIndex == -1 || lastSelectedTileIndex == -1)
        {
            return;
        } 


        string labelText = words[lastSelectedLabelIndex].Content;
        string tileSpriteName = gameBoard[lastSelectedTileIndex].SpriteName;


        //Capture sprite and caption for anlytics.
        bool isMoveCorrect = false;
        if(labelText.Equals(tileSpriteName))
        {
            isMoveCorrect = true;
            GameEvents.PlayerGotMatch.Invoke();
        }

        else
        {
            GameEvents.PlayerFailedMatch.Invoke();
        }


        //Move RecordMove to analytics manager!
        AnalyticsManager.RecordMove(currentPlayer, labelText, gameBoard[lastSelectedTileIndex].sprite, isMoveCorrect);
        AnalyticsManager.IncrementPlaytime(currentPlayer);
        GameEvents.TurnEnded.Invoke();
    }

    private void OnPlayerGotMatch()
    {
        gameBoard[lastSelectedTileIndex].SetPlayerThumbnail(currentPlayer);
        movesMade[currentPlayer].Add(lastSelectedTileIndex);
        gameBoard.DisableTile(lastSelectedTileIndex);
        words.DisableLabel(lastSelectedLabelIndex);
        
        AnalyticsManager.IncrementScore(currentPlayer);

        if(CheckForCurrentPlayerWin())
        {
            GameEvents.GameWon.Invoke();
        }
    }

    private void OnPlayerFailedMatch()
    {
        AnalyticsManager.IncrementNumberOfMistakes(currentPlayer);
    }

    private void OnTurnEnded()
    {

        currentPlayer = (Player)(((int)currentPlayer + 1) % 2);
        words[lastSelectedLabelIndex]?.SetLabelSelected(false);
        gameBoard[lastSelectedTileIndex]?.SetBorderColorSelected(false);


        lastSelectedLabelIndex = -1;
        lastSelectedTileIndex = -1;
    }

    private bool CheckForCurrentPlayerWin()
    {
        List<int> currentPlayerMoves = movesMade[currentPlayer];
        foreach((int a, int b, int c) triplet in GetWinningTriplets())
        {
            if(currentPlayerMoves.Contains(triplet.a) &&
                currentPlayerMoves.Contains(triplet.b) &&
                currentPlayerMoves.Contains(triplet.c))
                {
                    return true;
                }
        }

        return false;
    }

    private void OnGameWon()
    {
        SceneTransitionManager.MoveToNextScene();
    }

    private (int, int, int)[] GetWinningTriplets()
    {
        (int a, int b, int c)[] winningTriplets = new (int, int, int)[]
        {
            (0, 1, 2), //Top Row
            (3, 4, 5), // Middle Row
            (6, 7, 8), // Bottom Row
            (0, 3, 6), // Left Column
            (1, 4, 7), // Middle Column
            (2, 5, 8), // Right Column
            (0, 4, 8), // Main Diagonal
            (2, 4, 6) // Sub Diagonal
        };

        return winningTriplets;
    }
}

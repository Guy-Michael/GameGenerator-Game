using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Player
{
    Player1,
    Player2, 
    Tie
}

public class GameManager : MonoBehaviour
{
    [SerializeField] bool devMode;
    public static Dictionary<Player, (Sprite sprite, string caption, bool isCorrect)> analyticsMoveRecords;
    Dictionary<Player, List<int>> movesMade;
    GameLoader contentLoader;
    TileManager gameBoard;
    LabelManager words;
    ScoreIndicatorManager scoreManager;
    public static Player currentPlayer;
    SpaceshipHandler spaceship;
    TimerHandler timerHandler;
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

        scoreManager = GameObject.Find("Score Indicators").GetComponent<ScoreIndicatorManager>();

        timerHandler = GameObject.Find("Timer").GetComponent<TimerHandler>();

        GameEvents.PlayerGotMatch.AddListener(OnPlayerGotMatch);
        GameEvents.PlayerFailedMatch.AddListener(OnPlayerFailedMatch);
        GameEvents.TurnEnded.AddListener(OnTurnEnded);
        GameEvents.GameWon.AddListener(OnGameWon);
        GameEvents.RoundEnded.AddListener(OnRoundEnded);

        GameEvents.GameStarted.Invoke();
    }

    void OnTileClick(int tileIndex)
    {
        if(lastSelectedTileIndex != -1)
        {
            gameBoard[lastSelectedTileIndex].SetBorderColorSelected(false);
        }

        lastSelectedTileIndex = tileIndex;
        
        if(!devMode) CheckForMatch();
        if(devMode) DevModeWin();
    }

    private void DevModeWin()
    {
        gameBoard[lastSelectedTileIndex].SetPlayerThumbnail(currentPlayer);
        movesMade[currentPlayer].Add(lastSelectedTileIndex);
        gameBoard.DisableTile(lastSelectedTileIndex);
        AnalyticsManager.IncrementScore(currentPlayer);

        if(CheckForCurrentPlayerWin())
        {
            scoreManager.IncrementScore(currentPlayer);
            GameEvents.RoundEnded.Invoke();
        }

        else
        {
            GameEvents.TurnEnded.Invoke();
        }
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

        AnalyticsManager.RecordMove(currentPlayer, labelText, gameBoard[lastSelectedTileIndex].sprite, isMoveCorrect);
        AnalyticsManager.IncrementPlaytime(currentPlayer);
        
        if(CheckForCurrentPlayerWin())
        {
            GameEvents.RoundEnded.Invoke();
        }

        else
        {
            GameEvents.TurnEnded.Invoke();
        }
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
            scoreManager.IncrementScore(currentPlayer);
        }
    }

    private void OnRoundEnded()
    {
        LineRenderer line = DrawLineRendererOnWinningTriplet();
        timerHandler.PauseTimer();
        
        Timer.Fire(3f, () => 
        {
            timerHandler.ResumeTimer();
            GameEvents.TurnEnded.Invoke();
            gameBoard.ResetAll();
            words.ResetAll();
            Destroy(line);
            movesMade[Player.Player1].Clear();
            movesMade[Player.Player2].Clear();

        });
    }

    private LineRenderer DrawLineRendererOnWinningTriplet()
    {
        LineRenderer line = gameBoard.gameObject.AddComponent<LineRenderer>();
        line.positionCount = 2;
        (int a, int b, int c) winningTriplet = GetWinningTriplet();

        Vector3 start = gameBoard[winningTriplet.a].transform.position;
        start.z = 1;

        Vector3 end = gameBoard[winningTriplet.c].transform.position;
        end.z = 1;

        line.SetPosition(0, start);
        line.SetPosition(1, end);
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;

        return line;
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

    private (int, int, int) GetWinningTriplet()
    {
        List<int> currentPlayerMoves = movesMade[currentPlayer];
        foreach((int a, int b, int c) triplet in GetWinningTriplets())
        {
            if(currentPlayerMoves.Contains(triplet.a) &&
                currentPlayerMoves.Contains(triplet.b) &&
                currentPlayerMoves.Contains(triplet.c))
            {
                return triplet;
            }
        }

        return (-1, -1 , -1);
    }

    private bool CheckForCurrentPlayerWin()
    {
        List<int> currentPlayerMoves = movesMade[currentPlayer];
        print(currentPlayerMoves.Count);
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

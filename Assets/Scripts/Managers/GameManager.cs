using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Player
{
    Astronaut,
    Alien, 
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
    TimerHandler timerHandler;
    int lastSelectedTileIndex;
    int lastSelectedLabelIndex;
    

    void Start()
    {
        InitIntroScreen();
    }

    private void InitIntroScreen()
    {
        ChooseAndDisplayFirstPlayer();

        GameObject firstPlayerScreen = GameObject.Find("First Player Decleration Elements");
        GameObject gameScreen = GameObject.Find("Game");

        gameScreen.SetActive(false);

        Button b = GameObject.Find("First Player Decleration Elements").GetComponentInChildren<Button>();
        b.onClick.AddListener(() => OnGameStart(firstPlayerScreen, gameScreen));
    }

    private static void ChooseAndDisplayFirstPlayer()
    {
        int randomIndex = Mathf.RoundToInt(UnityEngine.Random.value);
        currentPlayer = (Player)Enum.GetValues(typeof(Player)).GetValue(randomIndex);

        TMPro.TextMeshProUGUI FirstPlayerName = GameObject.Find("Captions/Player Name").GetComponent<TMPro.TextMeshProUGUI>();
        FirstPlayerName.text = AnalyticsManager.analytics[currentPlayer].name;
    }

    private void OnGameStart(GameObject firstPlayerScreen, GameObject gameScreen)
    {
        firstPlayerScreen.SetActive(false);
        gameScreen.SetActive(true);

        InitializeMoves();
        InitializeGameElements();
        InitializeGameTheme();
        AddEventListeners();
        GameEvents.GameStarted.Invoke();
    }

    private void InitializeMoves()
    {
        movesMade = new();
        movesMade.Add(Player.Astronaut, new List<int>());
        movesMade.Add(Player.Alien, new List<int>());

        lastSelectedLabelIndex = -1;
        lastSelectedTileIndex = -1;
    }

    private void InitializeGameElements()
    {
        gameBoard = GameObject.Find("Tiles").GetComponent<TileManager>();
        gameBoard.InitTiles(OnTileClick);

        words = GameObject.Find("Labels").GetComponent<LabelManager>();
        words.Init(OnLabelClick);

        SpaceshipHandler spaceship = GameObject.Find("Spaceship").GetComponent<SpaceshipHandler>();
        spaceship.SetActivePlayer(currentPlayer);

        scoreManager = GameObject.Find("Score Indicators").GetComponent<ScoreIndicatorManager>();
        timerHandler = GameObject.Find("Timer").GetComponent<TimerHandler>();
    }

    private void AddEventListeners()
    {
        GameEvents.PlayerGotMatch.AddListener(OnPlayerGotMatch);
        GameEvents.PlayerFailedMatch.AddListener(OnPlayerFailedMatch);
        GameEvents.TurnEnded.AddListener(OnTurnEnded);
        GameEvents.GameWon.AddListener(OnGameWon);
        GameEvents.RoundEnded.AddListener(OnRoundEnded);
    }

    private void InitializeGameTheme()
    {
        contentLoader = GetComponent<GameLoader>();
        IAssetImporter importer = GetComponent<LocalAssetImporter>();
        contentLoader.InitializeGameGraphics(importer);
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
        
        Timer.Fire(2f, () => 
        {
            timerHandler.ResumeTimer();
            GameEvents.TurnEnded.Invoke();
            gameBoard.ResetAll();
            words.ResetAll();
            Destroy(line);
            movesMade[Player.Astronaut].Clear();
            movesMade[Player.Alien].Clear();

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

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool devMode;
    [SerializeField] int delayBetweenTurnsInMillis;
    Dictionary<Player, List<int>> correctMovesMadeInCurrentSet;
    GameLoader contentLoader;
    TileManager gameBoard;
    LabelManager words;
    ScoreIndicatorManager scoreManager;
    TimerHandler timerHandler;
    SpaceshipHandler spaceshipHandler;
    public static Player currentPlayer;
    int lastSelectedTileIndex;
    int lastSelectedLabelIndex;
    [SerializeField] bool shuffleGameOnNewRound;

    void Start()
    {
        InitIntroScreen();
    }

    private void InitIntroScreen()
    {

        GameObject firstPlayerScreen = GameObject.Find("First Player Decleration Elements");
        GameObject gameScreen = GameObject.Find("Game");
        
        ChooseAndDisplayFirstPlayer(firstPlayerScreen);

        gameScreen.SetActive(false);

        Button b = GameObject.Find("First Player Decleration Elements").GetComponentInChildren<Button>();
        b.onClick.AddListener(() => OnGameStart(firstPlayerScreen, gameScreen));
    }

    private static void ChooseAndDisplayFirstPlayer(GameObject firstPlayerScreen)
    {
        int randomIndex = Mathf.RoundToInt(UnityEngine.Random.value);
        currentPlayer = (Player)Enum.GetValues(typeof(Player)).GetValue(randomIndex);

        firstPlayerScreen.GetComponent<FirstPlayerDisplayer>().Init(currentPlayer);
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
        correctMovesMadeInCurrentSet = new();
        correctMovesMadeInCurrentSet.Add(Player.Astronaut, new List<int>());
        correctMovesMadeInCurrentSet.Add(Player.Alien, new List<int>());

        lastSelectedLabelIndex = -1;
        lastSelectedTileIndex = -1;
    }

    private void InitializeGameElements()
    {
        gameBoard = GameObject.Find("Tiles").GetComponent<TileManager>();
        gameBoard.InitTiles(OnTileClick);

        words = GameObject.Find("Labels").GetComponent<LabelManager>();
        words.Init(OnLabelClick);

        spaceshipHandler = GameObject.Find("Spaceship").GetComponent<SpaceshipHandler>();
        spaceshipHandler.SetActivePlayer(currentPlayer);

        scoreManager = GameObject.Find("Score Indicators").GetComponent<ScoreIndicatorManager>();
        timerHandler = GameObject.Find("Timer").GetComponent<TimerHandler>();

        GameGraphicsManager.SetActivePlayerTint(currentPlayer);
    }

    private void AddEventListeners()
    {
        GameEvents.PlayerGotMatch.AddListener(OnPlayerGotMatch);
        GameEvents.PlayerFailedMatch.AddListener(OnPlayerFailedMatch);
        GameEvents.TurnEnded.AddAsyncListener(OnTurnEnded);
        GameEvents.GameWon.AddListener(OnGameWon);
        GameEvents.SetEnded.AddAsyncListener(OnSetEnded);
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

   void OnLabelClick(int labelIndex)
    {
        if(lastSelectedLabelIndex != -1)
        {
            words[lastSelectedLabelIndex].SetLabelSelected(false);
        }
        lastSelectedLabelIndex = labelIndex;
        CheckForMatch();
    }

    private async void DevModeWin()
    {
        gameBoard[lastSelectedTileIndex].SetPlayerThumbnail(currentPlayer);
        correctMovesMadeInCurrentSet[currentPlayer].Add(lastSelectedTileIndex);
        gameBoard.DisableTile(lastSelectedTileIndex);
        AnalyticsManager.IncrementScore(currentPlayer);

        if(GameUtils.HasWonSet(correctMovesMadeInCurrentSet[currentPlayer]))
        {
            scoreManager.IncrementScore(currentPlayer);
            await GameEvents.SetEnded.Invoke();
        }

        else
        {
            await GameEvents.TurnEnded.Invoke();
        }

        MoveControlToOtherPlayer();
    }

    async void CheckForMatch()
    {
        if(lastSelectedLabelIndex == -1 || lastSelectedTileIndex == -1)
        {
            return;
        } 

        string labelText = words[lastSelectedLabelIndex].Content;
        string tileSpriteName = gameBoard[lastSelectedTileIndex].SpriteName;

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

        AnalyticsManager.RecordMove(currentPlayer, labelText, gameBoard[lastSelectedTileIndex].themeSprite, isMoveCorrect);
        AnalyticsManager.IncrementPlaytime(currentPlayer);
        
        if(GameUtils.HasWonSet(correctMovesMadeInCurrentSet[currentPlayer]))
        {
            await GameEvents.SetEnded.Invoke();
        }

        else
        {
            await GameEvents.TurnEnded.Invoke();
        }

        MoveControlToOtherPlayer();
    }

    private void OnPlayerGotMatch()
    {
        spaceshipHandler.SetTurnEndMessage(true);
        GameGraphicsManager.SetPlayerSpriteOnTurnEnd(currentPlayer, PlayerState.Active);
        gameBoard[lastSelectedTileIndex].SetPlayerThumbnail(currentPlayer);
        correctMovesMadeInCurrentSet[currentPlayer].Add(lastSelectedTileIndex);
        
        gameBoard.DisableTile(lastSelectedTileIndex);
        words.DisableLabel(lastSelectedLabelIndex);
        
        AnalyticsManager.IncrementScore(currentPlayer);

        if(GameUtils.HasWonSet(correctMovesMadeInCurrentSet[currentPlayer]))
        {
            scoreManager.IncrementScore(currentPlayer);
        }
    }

    private void OnPlayerFailedMatch()
    {
        spaceshipHandler.SetTurnEndMessage(false);
        GameGraphicsManager.SetPlayerSpriteOnTurnEnd(currentPlayer, PlayerState.Lost);
        AnalyticsManager.IncrementNumberOfMistakes(currentPlayer);
    }

    private async Task OnSetEnded()
    {
        (int a, int b, int c) winningTriplet = GameUtils.GetWinningTriplet(correctMovesMadeInCurrentSet[currentPlayer]);
        LineRenderer line = GameUtils.DrawLineRendererOnWinningTriplet(gameBoard, winningTriplet);
        spaceshipHandler.DisplayWonMessage();
        SetupTurnEnded(true);
        
        await Task.Delay(delayBetweenTurnsInMillis);

        SetupTurnStarted();
        Destroy(line);
        words.ResetAll();
        gameBoard.ResetAll();
        correctMovesMadeInCurrentSet[Player.Astronaut].Clear();
        correctMovesMadeInCurrentSet[Player.Alien].Clear();
    }

    private async Task OnTurnEnded()
    {
        SetupTurnEnded(false);
        await Task.Delay(delayBetweenTurnsInMillis);
        SetupTurnStarted();
    }

    private void SetupTurnStarted()
    {
        SetControlsEnabled(true);
        gameBoard.SetTilesEnabled(true);
        words.SetLabelsEnabled(true);
        spaceshipHandler.ToggleActivePlayer();
        timerHandler.RestartTimer();
        spaceshipHandler.ResetTurnEndMessage();
        GameGraphicsManager.ResetPlayersSprites();

        if(shuffleGameOnNewRound)
        {
            words.Shuffle();
        }
    }

    private void SetupTurnEnded(bool keepTilesVisible)
    {
        gameBoard.SetTilesEnabled(keepTilesVisible);
        SetControlsEnabled(false);
        words.SetLabelsEnabled(false);
        timerHandler.MakeTimerInvisible();
    }

    private void MoveControlToOtherPlayer()
    {
        currentPlayer = (Player)(((int)currentPlayer + 1) % 2);
        words[lastSelectedLabelIndex]?.SetLabelSelected(false);
        gameBoard[lastSelectedTileIndex]?.SetBorderColorSelected(false);
        GameGraphicsManager.SetActivePlayerTint(currentPlayer);

        lastSelectedLabelIndex = -1;
        lastSelectedTileIndex = -1;
    }

    private void SetControlsEnabled(bool enabled)
    {
        if(enabled)
        {
            words.SetLabelsEnabled(enabled);
            gameBoard.SetTilesEnabled(enabled);
        }
    }

    private void OnGameWon()
    {
        SceneTransitionManager.MoveToNextScene();
    }
}

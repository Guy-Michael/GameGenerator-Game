using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool devMode;
    Dictionary<Player, List<int>> correctMovesMadeInCurrentSet;
    GameLoader contentLoader;
    BoardElementManager board;
    PoolElementManager pool;
    Dictionary<Player, ScoreIndicatorManager> scoreManagers;
    TimerHandler timerHandler;
    SpaceshipHandler spaceshipHandler;
    public static Player currentPlayer;
    BoardElement lastSelectedBoardElement;
    PoolElement lastSelectedPoolElement;
    [SerializeField] bool shuffleGameOnNewRound;
    bool hasSetJustEnded;
    bool hasJustMadeCorrectMatch;
    GameGraphicsManager graphicsManager;

    void Start()
    {
        InitIntroScreen();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            AnalyticsManager.outcome = SetOutcome.AstronautWin;
            SceneTransitionManager.MoveToScene(SceneNames.FeedbackScreen);
        }
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
        correctMovesMadeInCurrentSet = new()
        {
            { Player.Astronaut, new List<int>() },
            { Player.Alien, new List<int>() }
        };
    }

    private void InitializeGameElements()
    {
        graphicsManager = GetComponent<GameGraphicsManager>();
        graphicsManager.InitGameCharacters();

        board = GameObject.Find("Board").GetComponentInChildren<BoardElementManager>();
        board.InitElements(OnTileClick);

        pool = GameObject.Find("Pool").GetComponentInChildren<PoolElementManager>();
        pool.InitElements(OnPoolElementClick);

        spaceshipHandler = GameObject.Find("Spaceship").GetComponent<SpaceshipHandler>();
        spaceshipHandler.Init(SetupTurnStarted);
        spaceshipHandler.SetActivePlayer(currentPlayer);

        scoreManagers = new();

        ScoreIndicatorManager astronautScore = GameObject.Find("Astronaut/Set Indicators").GetComponent<ScoreIndicatorManager>();
        astronautScore.Init();
        scoreManagers[Player.Astronaut] = astronautScore;

        ScoreIndicatorManager alienScore = GameObject.Find("Alien/Set Indicators").GetComponent<ScoreIndicatorManager>();
        alienScore.Init();
        scoreManagers[Player.Alien] = alienScore;

        timerHandler = GameObject.Find("Timer").GetComponent<TimerHandler>();

        graphicsManager.SetActivePlayerTint(currentPlayer);
    }

    private void AddEventListeners()
    {
        GameEvents.PlayerGotMatch.AddListener(OnPlayerGotMatch);
        GameEvents.PlayerFailedMatch.AddListener(OnPlayerFailedMatch);
        GameEvents.TurnEnded.AddListener(OnTurnEnded);
        GameEvents.GameWon.AddAsyncListener(OnGameWon);
        GameEvents.SetEnded.AddListener(OnSetEnded);
    }

    private void InitializeGameTheme()
    {
        contentLoader = GetComponent<GameLoader>();
        IAssetImporter importer = GetComponent<LocalAssetImporter>();
        contentLoader.InitializeGameGraphics(importer, AnalyticsManager.gameCode);
    }

    void OnTileClick(BoardElement element)
    {
        lastSelectedBoardElement?.SetBorderColorSelected(false);
        lastSelectedBoardElement = element;
        
        if(!devMode) CheckForMatch();
        if(devMode) DevModeWin();
    }

   void OnPoolElementClick(PoolElement element)
    {
        lastSelectedPoolElement?.SetBorderColorSelected(false);

        lastSelectedPoolElement = element;
        CheckForMatch();
    }

    private void DevModeWin()
    {
        lastSelectedBoardElement.SetPlayerThumbnail(currentPlayer);
        correctMovesMadeInCurrentSet[currentPlayer].Add(lastSelectedBoardElement.transform.GetSiblingIndex());
        lastSelectedBoardElement.Disable();
        AnalyticsManager.IncrementScore(currentPlayer);

        if(GameUtils.HasWonSet(correctMovesMadeInCurrentSet[currentPlayer]))
        {
            scoreManagers[currentPlayer].IncrementScore(currentPlayer.ToOutcome());
            GameEvents.SetEnded.Invoke();
        }

        else
        {
            GameEvents.TurnEnded.Invoke();
        }

        MoveControlToOtherPlayer();
    }

    void CheckForMatch()
    {
        if(lastSelectedPoolElement == null || lastSelectedBoardElement == null)
        {
            return;
        } 

        string poolMatchingContent = lastSelectedPoolElement.MatchingContent;
        string boardMatchingContent = lastSelectedBoardElement.MatchingContent;
        hasJustMadeCorrectMatch = false;
        if(poolMatchingContent.Equals(boardMatchingContent))
        {
            hasJustMadeCorrectMatch = true;
            GameEvents.PlayerGotMatch.Invoke();
        }

        else
        {
            GameEvents.PlayerFailedMatch.Invoke();
        }

        AnalyticsManager.RecordMove(currentPlayer, poolMatchingContent, lastSelectedBoardElement.themeSprite, hasJustMadeCorrectMatch);
        AnalyticsManager.IncrementPlaytime(currentPlayer);

        if(GameUtils.HasWonSet(correctMovesMadeInCurrentSet[currentPlayer]))
        {
            GameEvents.SetEnded.Invoke();
        }

        else
        {
            GameEvents.TurnEnded.Invoke();
        }
    }

    private void OnPlayerGotMatch()
    {
        spaceshipHandler.SetTurnEndMessage(true);
        graphicsManager.SetPlayerSpriteOnTurnEnd(currentPlayer, PlayerState.Active);

        correctMovesMadeInCurrentSet[currentPlayer].Add(lastSelectedBoardElement.transform.GetSiblingIndex());
        
        lastSelectedBoardElement.SetMatchFeedback(true);
        lastSelectedPoolElement.SetBorderColorOnMatch(true);
        
        AnalyticsManager.IncrementScore(currentPlayer);

        if(GameUtils.HasWonSet(correctMovesMadeInCurrentSet[currentPlayer]))
        {
            scoreManagers[currentPlayer].IncrementScore(currentPlayer.ToOutcome());
        }
    }

    private void OnPlayerFailedMatch()
    {
        lastSelectedBoardElement.SetMatchFeedback(false);
        lastSelectedPoolElement.SetBorderColorOnMatch(false);
        spaceshipHandler.SetTurnEndMessage(false);
        graphicsManager.SetPlayerSpriteOnTurnEnd(currentPlayer, PlayerState.Lost);
        AnalyticsManager.IncrementNumberOfMistakes(currentPlayer);
    }

    private void OnSetEnded()
    {
        (int a, int b, int c) winningTriplet = GameUtils.GetWinningTriplet(correctMovesMadeInCurrentSet[currentPlayer]);
        GameUtils.DrawLineRendererOnWinningTriplet(board, winningTriplet);
        lastSelectedBoardElement.SetPlayerThumbnail(currentPlayer);
        
        if(!IsGameWon())
        {
            spaceshipHandler.DisplayWonMessage();
        }
        
        hasSetJustEnded = true;
        SetupTurnEnded(true);
    }

    private void OnTurnEnded()
    {
        SetupTurnEnded(false);
    }

    private void SetupTurnStarted()
    {
        SetControlsEnabled(true);
        board.SetElementsEnabled(true);
        pool.SetElementsEnabled(true);
        spaceshipHandler.ToggleActivePlayer();
        timerHandler.RestartTimer();
        spaceshipHandler.ResetTurnEndMessage();
        spaceshipHandler.SetContinueButtonVisible(false);
        graphicsManager.ResetPlayersSprites();

        if(hasJustMadeCorrectMatch)
        {
            lastSelectedBoardElement.SetPlayerThumbnail(currentPlayer);
            lastSelectedPoolElement.Hide();
            hasJustMadeCorrectMatch = false;
        }

        if(hasSetJustEnded)
        {
            pool.Shuffle();
            GameUtils.DestroyLineRenderer();
            pool.ResetAll();
            board.ResetAll();
            correctMovesMadeInCurrentSet[Player.Astronaut].Clear();
            correctMovesMadeInCurrentSet[Player.Alien].Clear();

            hasSetJustEnded = false;
        }

        lastSelectedPoolElement = null;
        lastSelectedBoardElement = null;

        MoveControlToOtherPlayer();
    }

    private void SetupTurnEnded(bool elementsEnabled)
    {
        board.SetElementsEnabled(elementsEnabled);
        
        if(!IsGameWon())
        {
            spaceshipHandler.SetContinueButtonVisible(true);

        }
        SetControlsEnabled(false);
        pool.SetElementsEnabled(false);
        timerHandler.HideTimer();
    }

    private void MoveControlToOtherPlayer()
    {
        currentPlayer = (Player)(((int)currentPlayer + 1) % 2);
        lastSelectedPoolElement?.SetBorderColorSelected(false);
        lastSelectedBoardElement?.SetBorderColorSelected(false);
        graphicsManager.SetActivePlayerTint(currentPlayer);
    }

    private void SetControlsEnabled(bool enabled)
    {
        if(enabled)
        {
            pool.SetElementsEnabled(enabled);
            board.SetElementsEnabled(enabled);
        }
    }

    private async Task OnGameWon()
    {
        GameEvents.RemoveAllListeners();
        AnalyticsManager.outcome = currentPlayer.ToOutcome();
        await Task.Delay(3000);
        SceneTransitionManager.MoveToScene(SceneNames.FeedbackScreen);
    }

    private bool IsGameWon()
    {
        return scoreManagers[Player.Astronaut].gameWon || scoreManagers[Player.Alien].gameWon;
    }
}

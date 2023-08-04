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
    BoardElementManager elements;
    PoolElementManager words;
    Dictionary<Player, ScoreIndicatorManager> scoreManagers;
    TimerHandler timerHandler;
    SpaceshipHandler spaceshipHandler;
    public static Player currentPlayer;
    BoardElement lastSelectedBoardElement;
    PoolElement lastSelectedPoolElement;
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
        correctMovesMadeInCurrentSet = new()
        {
            { Player.Astronaut, new List<int>() },
            { Player.Alien, new List<int>() }
        };
    }

    private void InitializeGameElements()
    {
        elements = GameObject.Find("Board").GetComponentInChildren<BoardElementManager>();
        elements.InitElements(OnTileClick);

        words = GameObject.Find("Pool").GetComponentInChildren<PoolElementManager>();
        words.InitElements(OnPoolElementClick);

        spaceshipHandler = GameObject.Find("Spaceship").GetComponent<SpaceshipHandler>();
        spaceshipHandler.SetActivePlayer(currentPlayer);

        scoreManagers = new();

        ScoreIndicatorManager astronautScore = GameObject.Find("Astronaut/Set Indicators").GetComponent<ScoreIndicatorManager>();
        astronautScore.Init();
        scoreManagers[Player.Astronaut] = astronautScore;

        ScoreIndicatorManager alienScore = GameObject.Find("Alien/Set Indicators").GetComponent<ScoreIndicatorManager>();
        alienScore.Init();
        scoreManagers[Player.Alien] = alienScore;

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
        contentLoader.InitializeGameGraphics(importer, AnalyticsManager.gameCode);
    }

    void OnTileClick(BoardElement element)
    {
        if(lastSelectedBoardElement != null)
        {
            lastSelectedBoardElement.SetBorderColorSelected(false);
        }

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

    private async void DevModeWin()
    {
        lastSelectedBoardElement.SetPlayerThumbnail(currentPlayer);
        correctMovesMadeInCurrentSet[currentPlayer].Add(lastSelectedBoardElement.transform.GetSiblingIndex());
        lastSelectedBoardElement.Disable();
        AnalyticsManager.IncrementScore(currentPlayer);

        if(GameUtils.HasWonSet(correctMovesMadeInCurrentSet[currentPlayer]))
        {
            scoreManagers[currentPlayer].IncrementScore(currentPlayer.ToOutcome());
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
        if(lastSelectedPoolElement == null || lastSelectedBoardElement == null)
        {
            return;
        } 

        string poolMatchingContent = lastSelectedPoolElement.MatchingContent;
        string boardMatchingContent = lastSelectedBoardElement.MatchingContent;
        bool isMoveCorrect = false;
        if(poolMatchingContent.Equals(boardMatchingContent))
        {
            isMoveCorrect = true;
            GameEvents.PlayerGotMatch.Invoke();
        }

        else
        {
            GameEvents.PlayerFailedMatch.Invoke();
        }

        AnalyticsManager.RecordMove(currentPlayer, poolMatchingContent, lastSelectedBoardElement.themeSprite, isMoveCorrect);
        AnalyticsManager.IncrementPlaytime(currentPlayer);

        if(GameUtils.HasWonSet(correctMovesMadeInCurrentSet[currentPlayer]))
        {
            await GameEvents.SetEnded.Invoke();
        }

        else
        {
            await GameEvents.TurnEnded.Invoke();
        }
    }

    private void OnPlayerGotMatch()
    {
        spaceshipHandler.SetTurnEndMessage(true);
        GameGraphicsManager.SetPlayerSpriteOnTurnEnd(currentPlayer, PlayerState.Active);
        
        
        lastSelectedBoardElement?.SetPlayerThumbnail(currentPlayer);

        
        correctMovesMadeInCurrentSet[currentPlayer].Add(lastSelectedBoardElement.transform.GetSiblingIndex());
        
        lastSelectedBoardElement.Disable();
        lastSelectedPoolElement.gameObject.SetActive(false);
        
        AnalyticsManager.IncrementScore(currentPlayer);

        if(GameUtils.HasWonSet(correctMovesMadeInCurrentSet[currentPlayer]))
        {
            scoreManagers[currentPlayer].IncrementScore(currentPlayer.ToOutcome());
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
        LineRenderer line = GameUtils.DrawLineRendererOnWinningTriplet(elements, winningTriplet);
        spaceshipHandler.DisplayWonMessage();
        SetupTurnEnded(true);
        MoveControlToOtherPlayer();
        
        await Task.Delay(delayBetweenTurnsInMillis);

        SetupTurnStarted();
        Destroy(line);
        words.ResetAll();
        elements.ResetAll();
        correctMovesMadeInCurrentSet[Player.Astronaut].Clear();
        correctMovesMadeInCurrentSet[Player.Alien].Clear();
    }

    private async Task OnTurnEnded()
    {
        SetupTurnEnded(false);
        await Task.Delay(delayBetweenTurnsInMillis);
        SetupTurnStarted();
        MoveControlToOtherPlayer();
    }

    private void SetupTurnStarted()
    {
        SetControlsEnabled(true);
        elements.SetElementsEnabled(true);
        words.SetElementsEnabled(true);
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
        elements.SetElementsEnabled(keepTilesVisible);
        SetControlsEnabled(false);
        words.SetElementsEnabled(false);
        timerHandler.MakeTimerInvisible();
    }

    private void MoveControlToOtherPlayer()
    {
        currentPlayer = (Player)(((int)currentPlayer + 1) % 2);
        lastSelectedPoolElement?.SetBorderColorSelected(false);
        lastSelectedBoardElement?.SetBorderColorSelected(false);
        GameGraphicsManager.SetActivePlayerTint(currentPlayer);

        lastSelectedPoolElement = null;        
        lastSelectedBoardElement = null;
    }

    private void SetControlsEnabled(bool enabled)
    {
        if(enabled)
        {
            words.SetElementsEnabled(enabled);
            elements.SetElementsEnabled(enabled);
        }
    }

    private void OnGameWon()
    {
        SceneTransitionManager.MoveToNextScene();
    }
}

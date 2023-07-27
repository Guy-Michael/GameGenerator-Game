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
    ElementManager elements;
    LabelManager words;
    ScoreIndicatorManager scoreManager;
    TimerHandler timerHandler;
    SpaceshipHandler spaceshipHandler;
    public static Player currentPlayer;
    Element lastSelectedTile;
    GameLabel lastSelectedLabel;
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
    }

    private void InitializeGameElements()
    {
        elements = GameObject.Find("Tiles").GetComponent<ElementManager>();
        elements.InitElements(OnTileClick);

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

    void OnTileClick(Element element)
    {
        if(lastSelectedTile != null)
        {
            lastSelectedTile.SetBorderColorSelected(false);
        }

        lastSelectedTile = element;
        
        if(!devMode) CheckForMatch();
        if(devMode) DevModeWin();
    }

   void OnLabelClick(GameLabel label)
    {
        if(lastSelectedLabel != null)
        {
            lastSelectedLabel.SetLabelSelected(false);
        }

        lastSelectedLabel = label;
        CheckForMatch();
    }

    private async void DevModeWin()
    {
        lastSelectedTile.SetPlayerThumbnail(currentPlayer);
        correctMovesMadeInCurrentSet[currentPlayer].Add(lastSelectedTile.transform.GetSiblingIndex());
        lastSelectedTile.Disable();
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
        if(lastSelectedLabel == null || lastSelectedTile == null)
        {
            return;
        } 

        string labelText = lastSelectedLabel.Content;
        string tileSpriteName = lastSelectedTile.SpriteName;

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

        AnalyticsManager.RecordMove(currentPlayer, labelText, lastSelectedTile.themeSprite, isMoveCorrect);
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
        
        
        lastSelectedTile?.SetPlayerThumbnail(currentPlayer);

        
        correctMovesMadeInCurrentSet[currentPlayer].Add(lastSelectedTile.transform.GetSiblingIndex());
        
        lastSelectedTile.Disable();
        lastSelectedLabel.gameObject.SetActive(false);
        
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
        elements.SetElementsEnabled(keepTilesVisible);
        SetControlsEnabled(false);
        words.SetLabelsEnabled(false);
        timerHandler.MakeTimerInvisible();
    }

    private void MoveControlToOtherPlayer()
    {
        currentPlayer = (Player)(((int)currentPlayer + 1) % 2);
        lastSelectedLabel.SetLabelSelected(false);
        lastSelectedTile?.SetBorderColorSelected(false);
        GameGraphicsManager.SetActivePlayerTint(currentPlayer);

        lastSelectedLabel = null;        
        lastSelectedTile = null;
    }

    private void SetControlsEnabled(bool enabled)
    {
        if(enabled)
        {
            words.SetLabelsEnabled(enabled);
            elements.SetElementsEnabled(enabled);
        }
    }

    private void OnGameWon()
    {
        SceneTransitionManager.MoveToNextScene();
    }
}

using System.Collections;
using System.Collections.Generic;
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
    Player currentPlayer;
    Dictionary<Player, List<int>> movesMade;
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
        GameEvents.TurnEnded.AddListener(OnTurnEnded);

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

        if(labelText.Equals(tileSpriteName))
        {
            GameEvents.PlayerGotMatch.Invoke();
        }

        else
        {
            GameEvents.PlayerFailedMatch.Invoke();
        }

        GameEvents.TurnEnded.Invoke();
    }

    private void OnPlayerGotMatch()
    {
        gameBoard[lastSelectedTileIndex].SetPlayerThumbnail(currentPlayer);
        movesMade[currentPlayer].Add(lastSelectedTileIndex);
        gameBoard.DisableTile(lastSelectedTileIndex);
        words.DisableLabel(lastSelectedLabelIndex);
    }

    private void OnTurnEnded()
    {
        currentPlayer = (Player)(((int)currentPlayer + 1) % 2);
        words[lastSelectedLabelIndex]?.SetLabelSelected(false);
        gameBoard[lastSelectedTileIndex]?.SetBorderColorSelected(false);


        lastSelectedLabelIndex = -1;
        lastSelectedTileIndex = -1;
    }
}

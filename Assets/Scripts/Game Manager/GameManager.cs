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
    GameLoader loader;
    TileManager gameBoard;
    LabelManager words;
    Player currentPlayer;
    Dictionary<Player, List<int>> movesMade;
    List<int> player1Tiles;
    List<int> player2Tiles;
    void Start()
    {
        loader = GetComponent<GameLoader>();
        IAssetImporter importer = GetComponent<LocalAssetImporter>();
        loader.InitializeGameGraphics(importer);

        //This should be randomized.
        currentPlayer = Player.Player1;
        
        movesMade = new();
        movesMade.Add(Player.Player1, new List<int>());
        movesMade.Add(Player.Player2, new List<int>());

        //BAD WAY TO SEARCH, SHOULD CHANGE. MAYBE USE SERIALIZED FIELDS
        gameBoard = GameObject.Find("Tiles").GetComponent<TileManager>();
        words = GameObject.Find("Labels").GetComponent<LabelManager>();

        gameBoard.InitTiles(OnTileClick);


    }

    void OnTileClick(int tileIndex)
    {
        movesMade[currentPlayer].Add(tileIndex);
        CheckForWin();

        gameBoard[tileIndex].SetPlayerThumbnail(currentPlayer);
        currentPlayer = (Player)(((int)currentPlayer + 1) % 2);
    }

    void CheckForWin()
    {

    }

    
}

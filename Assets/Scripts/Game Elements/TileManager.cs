using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class TileManager : MonoBehaviour
{
    GameTile[] gameTiles;
    public void InitTiles(Action<int> onClickCallback)
    {
        gameTiles = GetComponentsInChildren<GameTile>();

        for(int i = 0; i < gameTiles.Length; i++)
        {
            gameTiles[i].Init(i, onClickCallback);
        }
    }

    public GameTile this[int index]
    {
        get => (index >= 0) && index < gameTiles.Length ? gameTiles[index] : null; //throw new IndexOutOfRangeException($"There are {gameTiles.Length} tile indices but index {index} was accessed.");
    }

    public void LoadSprites(Sprite[] sprites, Dictionary<Player, Sprite> winSprites)
    {
        gameTiles = GetComponentsInChildren<GameTile>();

        //CHANGE THIS.
        int shorterArray = Mathf.Min(sprites.Length, gameTiles.Length);
        
        for (int i = 0; i < shorterArray; i++)
        {
            gameTiles[i].LoadSprites(sprites[i], winSprites);    
        }
    }

    public void ResetAll()
    {
        foreach(GameTile tile in gameTiles)
        {
            tile.Reset();
        }
    }

    public void DisableTile(int index)
    {
        this[index].Disable();
    }

}

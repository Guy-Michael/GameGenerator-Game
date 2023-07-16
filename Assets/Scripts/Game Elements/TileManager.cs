using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileManager : MonoBehaviour
{
    GameTile[] gameTiles;
    public void InitTiles(Action<int> onClickCallback)
    {
        gameTiles = GetComponentsInChildren<GameTile>();

        foreach (GameTile tile in gameTiles)
        {
            tile.Init(onClickCallback);
        }
    }

    public GameTile this[int index]
    {
        get => gameTiles[index - 1];
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

}

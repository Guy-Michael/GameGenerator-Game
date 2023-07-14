using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    GameTile[] gameTiles;
    public void Init(Sprite[] sprites)
    {
        gameTiles = GetComponentsInChildren<GameTile>();
        int shorterArray = Mathf.Min(sprites.Length, gameTiles.Length);
        
        for (int i = 0; i < shorterArray; i++)
        {
            gameTiles[i].Init(sprites[i]);    
        }
    }
}

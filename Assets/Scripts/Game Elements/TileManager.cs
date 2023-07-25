using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class TileManager : MonoBehaviour
{
    GameTile[] gameTiles;
    public GameTile this[int index]
    {
        get => (index >= 0) && index < gameTiles.Length ? gameTiles[index] : null;
    }

    public void InitTiles(Action<int> onClickCallback)
    {
        gameTiles = GetComponentsInChildren<GameTile>();

        for(int i = 0; i < gameTiles.Length; i++)
        {
            gameTiles[i].Init(i, onClickCallback);
        }
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

    public void Shuffle()
    {
        IEnumerable<int> indecies = Enumerable.Range(0, gameTiles.Length).OrderBy(s => UnityEngine.Random.value);
        for(int i = 0 ; i < gameTiles.Length; i++)
        {
            gameTiles[i].transform.SetSiblingIndex(indecies.ElementAt(i));
        }
    }


    public void ResetAll()
    {
        foreach(GameTile tile in gameTiles)
        {
            tile.ResetTile();
        }
    }

    public void DisableTile(int index)
    {
        this[index].Disable();
    }

    internal void SetTilesEnabled(bool enabled)
    {
        foreach(GameTile tile in gameTiles)
        {
            tile.SetInteractable(enabled);
        }
    }
}

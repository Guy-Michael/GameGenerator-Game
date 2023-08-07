using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    IAssetImporter assetImporter;
    [SerializeField] BoardElementManager gameBoard;
    [SerializeField] PoolElementManager pool;

    public void InitializeGameGraphics(IAssetImporter assetImporter, string gameCode)
    {
        this.assetImporter = assetImporter;
        Dictionary<Player, Sprite> winThumbnails = assetImporter.ImportWinThumbnails();

        (Sprite[] boardElements, string[] poolElements) = assetImporter.ImportData(gameCode);
        
        boardElements = boardElements[0..9].OrderBy(s=>UnityEngine.Random.value).ToArray();
        poolElements = poolElements[0..12].OrderBy(s=>UnityEngine.Random.value).ToArray();
        
        gameBoard.LoadSprites(boardElements, winThumbnails);
        pool.LoadTexts(poolElements, winThumbnails);
    
    }
}

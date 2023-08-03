using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    IAssetImporter assetImporter;
    [SerializeField] BoardElementManager gameBoard;
    [SerializeField] PoolElementManager pool;

    public void InitializeGameGraphics(IAssetImporter assetImporter)
    {
        this.assetImporter = assetImporter;
        Dictionary<string, Sprite> assets = assetImporter.ImportTiles();
        Dictionary<Player, Sprite> winThumbnails = assetImporter.ImportWinThumbnails();
        string[] additionalData = assetImporter.ImportAdditionalLabels();

        


        gameBoard.LoadSprites(assets.Values.ToArray(), winThumbnails, assets.Keys.ToArray());
        pool.LoadSprites(assets.Values.ToArray(), winThumbnails);
    }
}

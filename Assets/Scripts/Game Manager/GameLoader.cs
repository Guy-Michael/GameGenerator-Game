using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    IAssetImporter assetImporter;
    [SerializeField] TileManager gameBoard;
    [SerializeField] LabelManager labelBoard;

    public void InitializeGameGraphics(IAssetImporter assetImporter)
    {
        this.assetImporter = assetImporter;
        Dictionary<string, Sprite> assets = assetImporter.ImportTiles();
        Dictionary<Player, Sprite> winThumbnails = assetImporter.ImportWinThumbnails();
        string[] additionalData = assetImporter.ImportAdditionalLabels();

        gameBoard.LoadSprites(assets.Values.ToArray(), winThumbnails);
        labelBoard.LoadContent(assets.Keys.ToArray(), additionalData);
    }
}

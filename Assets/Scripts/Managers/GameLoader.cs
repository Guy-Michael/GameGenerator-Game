using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    IAssetImporter assetImporter;
    [SerializeField] ElementManager gameBoard;
    [SerializeField] LabelManager labelBoard;

    public void InitializeGameGraphics(IAssetImporter assetImporter)
    {
        this.assetImporter = assetImporter;
        Dictionary<string, Sprite> assets = assetImporter.ImportTiles();
        Dictionary<Player, Sprite> winThumbnails = assetImporter.ImportWinThumbnails();
        string[] additionalData = assetImporter.ImportAdditionalLabels();

        


        gameBoard.LoadSprites(assets.Values.ToArray(), winThumbnails, assets.Keys.ToArray());
        labelBoard.LoadContent(assets.Keys.ToArray(), additionalData);
    }
}

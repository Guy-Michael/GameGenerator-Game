using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    IAssetImporter assetImporter;
    [SerializeField] TileManager gameBoard;
    [SerializeField] LabelManager labelBoard;

    public void Init(IAssetImporter assetImporter)
    {
        this.assetImporter = assetImporter;
        Dictionary<string, Sprite> assets = assetImporter.ImportAssets();
        string[] additionalData = assetImporter.ImportAdditionalLabels();

        gameBoard.Init(assets.Values.ToArray());
        labelBoard.init(assets.Keys.ToArray(), additionalData);
    }
}

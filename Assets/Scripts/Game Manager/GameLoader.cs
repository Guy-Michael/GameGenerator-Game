using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    IAssetImporter assetFetcher;
    [SerializeField] TileManager gameBoard;
    [SerializeField] LabelManager labelBoard;

    public void Init(IAssetImporter AssetImporter)
    {
        this.assetFetcher = AssetImporter;
        Dictionary<string, Sprite> assets = AssetImporter.ImportAssets();

        gameBoard.Init(assets.Values.ToArray());
        labelBoard.init(assets.Keys.ToArray());
    }
}

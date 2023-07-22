using UnityEngine;
using System.Collections.Generic;

public interface IAssetImporter
{
    public Dictionary<string, Sprite> ImportTiles();
    public Dictionary<Player, Sprite> ImportWinThumbnails();
    public string[] ImportAdditionalLabels();
    public string[] GetGameCodes();
}
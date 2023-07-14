using UnityEngine;
using System.Collections.Generic;

public interface IAssetImporter
{
    public Dictionary<string, Sprite> ImportAssets();
    public string[] ImportAdditionalLabels();
}
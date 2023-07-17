using System.Collections.Generic;
using UnityEngine;

public class MockAssetImporter : MonoBehaviour, IAssetImporter
{
    [SerializeField] Sprite testSprite;

    public string[] GetGameCodes()
    {
        throw new System.NotImplementedException();
    }

    public string[] ImportAdditionalLabels()
    {
        throw new System.NotImplementedException();
    }

    public Dictionary<string, Sprite> ImportTiles()
    {
        Dictionary<string, Sprite> dict = new();


        for (int i = 0; i < 9; i++)
        {
            dict.Add(Random.value.ToString("0.000"), testSprite);
        }

        return dict;
    }

    public Dictionary<Player, Sprite> ImportWinThumbnails()
    {
        throw new System.NotImplementedException();
    }
}
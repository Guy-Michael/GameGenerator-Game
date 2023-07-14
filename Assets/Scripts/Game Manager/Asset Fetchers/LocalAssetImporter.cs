using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class LocalAssetImporter : MonoBehaviour, IAssetImporter
{
    public Dictionary<string, Sprite> ImportAssets()
    {
        Sprite[] countrySprites = Resources.LoadAll("Graphics/Countries", typeof(Sprite)).Cast<Sprite>().ToArray();
    
        Dictionary<string, Sprite> sprites = new();
    
        foreach(Sprite sprite in countrySprites)
        {
            string name = sprite.name.Split(".")[0];
            sprites.Add(name, sprite);
        }

        return sprites;
    }
}
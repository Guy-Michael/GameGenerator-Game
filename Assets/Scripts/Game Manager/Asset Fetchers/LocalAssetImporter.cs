using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class LocalAssetImporter : MonoBehaviour, IAssetImporter
{
    public string[] ImportAdditionalLabels()
    {
        TextAsset additionalCountriesJson = (TextAsset) Resources.Load("Text/AdditionalCountries.json", typeof(string));
        // string[] additionalCountries = JsonConvert.DeserializeObject<string[]>(additionalCountriesJson.text);
        // return additionalCountries;
        return new string[]{"country1", "country2", "country3"};
    }

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
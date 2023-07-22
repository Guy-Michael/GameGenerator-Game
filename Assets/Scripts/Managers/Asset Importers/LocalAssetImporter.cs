using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class LocalAssetImporter : MonoBehaviour, IAssetImporter
{
    public string[] GetGameCodes()
    {
        return new string[] {"101", "102"};
    }

    public string[] ImportAdditionalLabels()
    {
        TextAsset asset = Resources.Load<TextAsset>("Text/AdditionalCountries");
        string[] additionalCountries = JsonConvert.DeserializeObject<string[]>(asset.text);

        return additionalCountries;
    }

    public Dictionary<string, Sprite> ImportTiles()
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

    public Dictionary<Player, Sprite> ImportWinThumbnails()
    {
        Dictionary<Player, Sprite> winThumbnails = new();
        winThumbnails.Add(Player.Astronaut, Resources.Load<Sprite>("Graphics/Win Thumbnails/Astronaut"));
        winThumbnails.Add(Player.Alien, Resources.Load<Sprite>("Graphics/Win Thumbnails/Alien"));

        return winThumbnails;
    }
}
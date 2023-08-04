using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class LocalAssetImporter : MonoBehaviour, IAssetImporter
{
    public string[] GetGameCodes()
    {
        //Temporary. this should be fetched from the DB.
        //100 and 101 should still be present locally. rest should be imported.
        return new string[] {"100", "101"};
    }

    public string[] ImportAdditionalLabels()
    {
        TextAsset asset = Resources.Load<TextAsset>("Text/AdditionalCountries");
        string[] additionalCountries = JsonConvert.DeserializeObject<string[]>(asset.text);

        return additionalCountries;
    }

    public (Sprite[] boardAssets, string[] poolAssets) ImportData(string gameCode)
    {
        Sprite[]sprites = null;
        string[] strings = null;
        switch(gameCode)
        {
            case "100": //countries
            {
                sprites = Resources.LoadAll("Graphics/Countries", typeof(Sprite)).Cast<Sprite>().ToArray();
                break;
            }
         
            case "101": //logos
            {
                sprites = Resources.LoadAll("Themes/Logos", typeof(Sprite)).Cast<Sprite>().ToArray();
                break;
            }
        }
             
        strings = sprites.Select(sprite => sprite.name).ToArray();
        return (sprites, strings);
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
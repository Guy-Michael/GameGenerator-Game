using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class LocalAssetImporter : MonoBehaviour, IAssetImporter
{
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
        winThumbnails.Add(Player.Player1, Resources.Load<Sprite>("Graphics/Win Thumbnails/Alien"));
        winThumbnails.Add(Player.Player2, Resources.Load<Sprite>("Graphics/Win Thumbnails/Astronaut"));

        return winThumbnails;
    }
}
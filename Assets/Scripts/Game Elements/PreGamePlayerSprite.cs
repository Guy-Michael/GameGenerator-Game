using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreGamePlayerSprite : MonoBehaviour
{
    [SerializeField] Sprite alien;
    [SerializeField] Sprite astronaut;
    Dictionary<Player, Sprite> sprites;

    public void Init(Player player)
    {
        sprites = new();
        sprites.Add(Player.Alien, alien);
        sprites.Add(Player.Astronaut, astronaut);

        Image image = GetComponentInChildren<Image>();
        image.sprite = sprites[player];
    }
}

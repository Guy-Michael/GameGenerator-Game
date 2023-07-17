using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipHandler : MonoBehaviour
{
    [SerializeField] RectTransform[] players;
    RectTransform spaceshipRectTransform;
    int selectedPlayerIndex = 0;
    
    void Start()
    {
        spaceshipRectTransform = GetComponent<RectTransform>();
        GameEvents.TurnEnded.AddListener(ToggleActivePlayer);
    }

    public void ToggleActivePlayer()
    {
        selectedPlayerIndex = (selectedPlayerIndex + 1) % players.Length;
        
        float nextPlayerX = players[selectedPlayerIndex].anchoredPosition.x;
        float y = spaceshipRectTransform.anchoredPosition.y;
        spaceshipRectTransform.anchoredPosition = new Vector2(nextPlayerX, y);
    }
}

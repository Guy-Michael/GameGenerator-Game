using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Sprite active;
    [SerializeField] Sprite idle;
    [SerializeField] Sprite lost;
    [SerializeField] Color NotSelectedTint;
    Image image;
    void Start()
    {
        image = GetComponentInChildren<Image>();
        TMPro.TextMeshProUGUI name = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        name.text = AnalyticsManager.analytics[player].name;
    }

    public void SetSprite(PlayerState state)
    {
        switch(state)
        {
            case PlayerState.Active:
            {
                image.sprite = active;
                break;
            }

            case PlayerState.Idle:
            {
                image.sprite = idle;
                break;
            }

            case PlayerState.Lost:
            {
                image.sprite = lost;
                break;
            }
        }
    }

    public void SetActiveInGame(bool isActive)
    {
        image.sprite = isActive ? active : idle; 
        image.color = isActive ? Color.white : NotSelectedTint;
    }



}

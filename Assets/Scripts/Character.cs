using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState
{
    Active,
    Idle,
    Lost
}

public class Character : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Sprite active;
    [SerializeField] Sprite idle;
    [SerializeField] Sprite lost;

    void Start()
    {
        TMPro.TextMeshProUGUI name = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        name.text = AnalyticsManager.analytics[player].name;
    }



}

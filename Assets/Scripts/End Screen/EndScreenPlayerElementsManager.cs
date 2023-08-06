using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenPlayerElementsManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] EndScreenPlayerData dataContainer;
    [SerializeField] EndScreenPlayerSprite playerSprite;

    void Start()
    {
        dataContainer = GetComponentInChildren<EndScreenPlayerData>();
        playerSprite = GetComponentInChildren<EndScreenPlayerSprite>();

        PlayerAnalytics analytics = AnalyticsManager.analytics[player];
        dataContainer.Init(analytics.name, analytics.score, analytics.numberOfMistakes, analytics.playTime);
        
        //Change to actual win value
        playerSprite.Init(true);
    }
}

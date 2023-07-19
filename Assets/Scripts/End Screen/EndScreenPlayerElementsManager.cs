using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenPlayerElementsManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] EndScreenPlayerData dataContainer;
    [SerializeField] EndScreenPlayerSprite playerSprite;
    [SerializeField] EndScreenMatchManager matchManager;

    void Start()
    {
        dataContainer = GetComponentInChildren<EndScreenPlayerData>();
        playerSprite = GetComponentInChildren<EndScreenPlayerSprite>();
        matchManager = GetComponentInChildren<EndScreenMatchManager>();

        PlayerAnalytics analytics = AnalyticsManager.analytics[player];
        dataContainer.Init(analytics.name, analytics.score, analytics.numberOfMistakes, analytics.playTime);
        
        //Change to actual win value
        playerSprite.Init(true);

        matchManager.Init(analytics.moves);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenMatchManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] EndScreenMatch matchPrefab;
    [SerializeField] Transform matchContainer;

    void Start()
    {
        var list = AnalyticsManager.analytics[player].moves;
        foreach(var item in list)
        {
            EndScreenMatch match = Instantiate(matchPrefab, matchContainer);
            print("is match null? : " + match == null);
            match.Init(item.sprite, item.Caption, item.isCorrect);
        }
    }
}

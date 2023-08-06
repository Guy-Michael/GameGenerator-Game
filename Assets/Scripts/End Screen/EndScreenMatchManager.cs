using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenMatchManager : MonoBehaviour
{
    [SerializeField] EndScreenMatch matchPrefab;
    [SerializeField] Transform matchContainer;
    [SerializeField] Player player;
    
    void Start()
    {
        var matchList = AnalyticsManager.analytics[player].moves;
        var correctMatches = matchList.Where(match => match.isCorrect);
        var wrongMatches = matchList.Where(match => !match.isCorrect);

        foreach((Sprite sprite, string caption, bool isCorrect) in correctMatches)
        {
            EndScreenMatch match = Instantiate(matchPrefab, matchContainer);
            match.Init(sprite, caption, isCorrect);
        }
        
        foreach((Sprite sprite, string caption, bool isCorrect) in wrongMatches)
        {
            EndScreenMatch match = Instantiate(matchPrefab, matchContainer);
            match.Init(sprite, caption, isCorrect);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenMatchManager : MonoBehaviour
{
    [SerializeField] EndScreenMatch matchPrefab;
    [SerializeField] Transform matchContainer;

    public void Init(List<(Sprite sprite, string caption, bool isCorrect)> matchList)
    {
        foreach((Sprite sprite, string caption, bool isCorrect) item in matchList)
        {
            EndScreenMatch match = Instantiate(matchPrefab, matchContainer);
            match.Init(item.sprite, item.caption, item.isCorrect);
        }
    }
}

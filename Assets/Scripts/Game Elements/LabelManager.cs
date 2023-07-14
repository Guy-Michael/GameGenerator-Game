using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelManager : MonoBehaviour
{
    GameLabel[] gameLabels;
    public void init(string[] labelTexts)
    {
        this.gameLabels = GetComponentsInChildren<GameLabel>();
        int shorterArray = Mathf.Min(gameLabels.Length, labelTexts.Length);
        
        for(int i = 0; i < shorterArray; i++)
        {
            gameLabels[i].Init(labelTexts[i]);
        }
    }
}

using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class LabelManager : MonoBehaviour
{
    GameLabel[] gameLabels;
    const int numberOfLabels = 12;
    const int numberOfGameTiles = 9;
    public void init(string[] realData, string[] additionalData)
    {
        this.gameLabels = GetComponentsInChildren<GameLabel>();
        List<string> dataToUse = new();

        if(realData.Length >= numberOfGameTiles)
        {
            dataToUse.AddRange(realData[0..numberOfGameTiles]);
        }

        int remainingAmount = numberOfLabels - numberOfGameTiles;

        dataToUse.AddRange(additionalData[0..remainingAmount]);

        dataToUse = dataToUse.OrderBy(s => Random.value).ToList();

        for(int i = 0; i < dataToUse.Count; i++)
        {
            gameLabels[i].Init(dataToUse[i]);
        }
    }
}

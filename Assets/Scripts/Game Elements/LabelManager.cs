using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LabelManager : MonoBehaviour
{
    GameLabel[] gameLabels;
    const int numberOfLabels = 12;
    const int numberOfGameTiles = 9;

    public GameLabel this[int index]
    {
        get => (index >= 0) && index < gameLabels.Length ? gameLabels[index] : null;// throw new IndexOutOfRangeException($"There are {gameLabels.Length} label indices but index {index} was accessed.");
    }

    //Initializes functionality.
    public void Init(Action<int> onLabelClickCallback)
    {
        this.gameLabels = GetComponentsInChildren<GameLabel>();
        for(int i = 0; i < gameLabels.Length; i++)
        {
            gameLabels[i].Init(i, onLabelClickCallback);
        }
    }

    //This is for initializing the graphics
    public void LoadContent(string[] realData, string[] additionalData)
    {
        List<string> dataToUse = new();

        if(realData.Length >= numberOfGameTiles)
        {
            dataToUse.AddRange(realData[0..numberOfGameTiles]);
        }

        int remainingAmount = numberOfLabels - numberOfGameTiles;
        dataToUse.AddRange(additionalData[0..remainingAmount]);
        dataToUse = dataToUse.OrderBy(s => UnityEngine.Random.value).ToList();

        for(int i = 0; i < dataToUse.Count; i++)
        {
            gameLabels[i].LoadContent(dataToUse[i]);
        }
    }

    public void DisableLabel(int index)
    {
        this[index].Disable();
    }


}

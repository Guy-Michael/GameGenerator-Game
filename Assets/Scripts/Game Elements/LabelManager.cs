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
        get => (index >= 0) && index < gameLabels.Length ? gameLabels[index] : null;
    }

    public void Init(Action<GameLabel> onLabelClickCallback)
    {
        this.gameLabels = GetComponentsInChildren<GameLabel>();
        foreach(GameLabel label in gameLabels)
        {
            label.Init(onLabelClickCallback);
        }
    }
    
    public void LoadContent(string[] realData, string[] additionalData)
    {
        List<string> dataToUse = new();

        if(realData.Length >= numberOfGameTiles)
        {
            dataToUse.AddRange(realData[0..numberOfGameTiles]);
        }

        int remainingAmount = numberOfLabels - numberOfGameTiles;
        dataToUse.AddRange(additionalData[0..remainingAmount]);

        for(int i = 0; i < dataToUse.Count; i++)
        {
            gameLabels[i].LoadContent(dataToUse[i]);
        }

        Shuffle();
    }

    public void Shuffle()
    {

        //get transform of all active labels.
        //shuffle transforms
        //apply transforms in new order.


        // IEnumerable<int> indecies = Enumerable.Range(0, gameLabels.Length).OrderBy(s => UnityEngine.Random.value);
        // for(int i = 0 ; i < gameLabels.Length; i++)
        // {
        //     gameLabels[i].transform.SetSiblingIndex(indecies.ElementAt(i));
        // }

        // this.gameLabels = GetComponentsInChildren<GameLabel>();
    }

    public void ResetAll()
    {
        foreach(GameLabel label in gameLabels)
        {
            label.gameObject.SetActive(true);
            label.Reset();
        }
    }

    public void DisableLabel(int index)
    {
        
        gameLabels[index].gameObject.SetActive(false);
        // var list = gameLabels.ToList();
        // Destroy(list[index].gameObject);
        // list.RemoveAt(index);
        // gameLabels = list.ToArray();

    }

    public void SetLabelsEnabled(bool enabled)
    {
        foreach(GameLabel label in gameLabels)
        {
            label.SetInteractable(enabled);
        }
    }
}

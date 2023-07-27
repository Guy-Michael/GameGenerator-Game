using System.Reflection.Emit;
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
        List<GameLabel> activeLabels = gameLabels.Where(label => label.gameObject.activeInHierarchy).ToList();
        List<Vector2> positions = activeLabels.Select(label => label.GetComponent<RectTransform>().anchoredPosition).ToList();

        positions = positions.OrderBy((p) => UnityEngine.Random.value).ToList();
        for(int i = 0; i < activeLabels.Count; i++)
        {
            activeLabels[i].GetComponent<RectTransform>().anchoredPosition = positions[i];
        }
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
    }

    public void SetLabelsEnabled(bool enabled)
    {
        foreach(GameLabel label in gameLabels)
        {
            label.SetInteractable(enabled);
        }
    }
}

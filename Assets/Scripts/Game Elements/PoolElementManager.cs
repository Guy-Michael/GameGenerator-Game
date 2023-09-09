using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;
using System.Collections;

public class PoolElementManager : MonoBehaviour
{
    PoolElement[] gameElements;
    GridLayoutGroup layoutGroup;

    public PoolElement this[int index]
    {
        get => (index >= 0) && index < gameElements.Length ? gameElements[index] : null;
    }

    public void InitElements(Action<PoolElement> onClickCallback)
    {
        layoutGroup = GetComponent<GridLayoutGroup>();
        gameElements = GetComponentsInChildren<PoolElement>();

        foreach(PoolElement element in gameElements)
        {
            element.Init(onClickCallback);
        }
    }

    public void LoadSprites(Sprite[] sprites, Dictionary<Player, Sprite> winSprites)
    {
        gameElements = GetComponentsInChildren<PoolElement>();
        int shorterArray = Mathf.Min(sprites.Length, gameElements.Length);
        
        for (int i = 0; i < shorterArray; i++)
        {
            gameElements[i].LoadSprites(sprites[i], winSprites);
        }
    }

    public void LoadTexts(string[] captions, Dictionary<Player, Sprite> winSprites)
    {
        gameElements = GetComponentsInChildren<PoolElement>();
        
        for (int i = 0; i < captions.Length; i++)
        {
            gameElements[i].LoadText(winSprites, captions[i]);
        }
    }

    public void Shuffle()
    {
        IEnumerable<int> indecies = Enumerable.Range(0, gameElements.Length).OrderBy(s => UnityEngine.Random.value);
        layoutGroup.enabled = true;
        for(int i = 0 ; i < gameElements.Length; i++)
        {
            gameElements[i].transform.SetSiblingIndex(indecies.ElementAt(i));
        }

        StartCoroutine(TurnOfGridLayout());

        IEnumerator TurnOfGridLayout()
        {
            yield return null;
            layoutGroup.enabled = false;
        }
    }


    public void ResetAll()
    {
        foreach(PoolElement element in gameElements)
        {
            element.gameObject.SetActive(true);
            element.ResetElement();
        }
    }

    public void DisableElement(int index)
    {
        this[index].Disable();
    }

    internal void SetElementsEnabled(bool enabled)
    {
        foreach(PoolElement Element in gameElements)
        {
            Element.SetInteractable(enabled);
        }
    }
}

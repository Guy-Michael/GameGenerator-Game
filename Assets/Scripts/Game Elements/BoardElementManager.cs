using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class BoardElementManager : MonoBehaviour
{
    BoardElement[] gameElements;
    public BoardElement this[int index]
    {
        get => (index >= 0) && index < gameElements.Length ? gameElements[index] : null;
    }

    public void InitElements(Action<BoardElement> onClickCallback)
    {
        gameElements = GetComponentsInChildren<BoardElement>();

        foreach(BoardElement element in gameElements)
        {
            element.Init(onClickCallback);
        }
    }

    public void LoadSprites(Sprite[] sprites, Dictionary<Player, Sprite> winSprites, string[] captions = null)
    {
        gameElements = GetComponentsInChildren<BoardElement>();
        int shorterArray = Mathf.Min(sprites.Length, gameElements.Length);
        
        for (int i = 0; i < shorterArray; i++)
        {
            gameElements[i].LoadSprites(sprites[i], winSprites);
        }
    }

    public void Shuffle()
    {
        IEnumerable<int> indecies = Enumerable.Range(0, gameElements.Length).OrderBy(s => UnityEngine.Random.value);
        for(int i = 0 ; i < gameElements.Length; i++)
        {
            gameElements[i].transform.SetSiblingIndex(indecies.ElementAt(i));
        }
    }


    public void ResetAll()
    {
        foreach(BoardElement element in gameElements)
        {
            element.ResetElement();
        }
    }

    public void DisableElement(int index)
    {
        this[index].Disable();
    }

    internal void SetElementsEnabled(bool enabled)
    {
        foreach(BoardElement Element in gameElements)
        {
            Element.SetInteractable(enabled);
        }
    }
}

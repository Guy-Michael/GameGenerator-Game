using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenPlayerSprite : MonoBehaviour
{
    [SerializeField] Sprite winSprite;
    [SerializeField] Sprite loseSprite;
    [SerializeField] Sprite tieSprite;

    public void Init(bool hasWon)
    {
        GetComponent<Image>().sprite = hasWon ? winSprite : loseSprite;
    }
}

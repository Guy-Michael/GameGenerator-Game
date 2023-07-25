using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPlayerDisplayer : MonoBehaviour
{
    [SerializeField] Sprite astronautWin;
    [SerializeField] Sprite astronautLose;
    [SerializeField] Sprite alienWin;
    [SerializeField] Sprite alienLose;

    public void Init(Player firstPlayer)
    {

        string rootPath = "First Player Decleration Elements";
        PreGamePlayerSprite p1 = GameObject.Find($"{rootPath}/First Player").GetComponent<PreGamePlayerSprite>();
        PreGamePlayerSprite p2 = GameObject.Find($"{rootPath}/Last Player").GetComponent<PreGamePlayerSprite>();
        p1.Init(firstPlayer);
        p2.Init(firstPlayer.Other());

        TMPro.TextMeshProUGUI FirstPlayerName = GameObject.Find("Captions/Player Name").GetComponent<TMPro.TextMeshProUGUI>();
        FirstPlayerName.text = AnalyticsManager.analytics[firstPlayer].name;
    }

}

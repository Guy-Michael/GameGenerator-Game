using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPlayerDisplayer : MonoBehaviour
{
    public void Init(Player firstPlayer)
    {
        GameGraphicsManager.SetFirstPlayer(firstPlayer);
        TMPro.TextMeshProUGUI FirstPlayerName = GameObject.Find("Captions/Player Name").GetComponent<TMPro.TextMeshProUGUI>();
        FirstPlayerName.text = AnalyticsManager.analytics[firstPlayer].name;
    }

}

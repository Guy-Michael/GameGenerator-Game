using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundsIndicatorManager : MonoBehaviour
{
    public int currentNumberOfRounds;
    void Start()
    {
        this.currentNumberOfRounds = 0;
        RoundAmountIndicator[] indicators = GetComponentsInChildren<RoundAmountIndicator>();
        foreach( RoundAmountIndicator indicator in indicators)
        {
            indicator.Init(OnIndicatorSelected);
        }        
    }

    private void OnIndicatorSelected(int amount)
    {
        currentNumberOfRounds = amount;
    }

}

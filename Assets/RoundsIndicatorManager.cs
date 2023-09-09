using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundsIndicatorManager : MonoBehaviour
{
    RoundAmountIndicator currentIndicator;
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

    private void OnIndicatorSelected(RoundAmountIndicator indicator)
    {
        if(indicator == currentIndicator)
        {
            return;
        }

        currentIndicator?.SetSelected(false);
        
        currentIndicator = indicator;
        currentIndicator.SetSelected(true);
        currentNumberOfRounds = indicator.value;

    }

}

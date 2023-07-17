using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerHandler : MonoBehaviour
{

    [SerializeField] float timerDuration;
    TimerInstance timer;
    TextMeshProUGUI text;
    
    void Awake()
    {
        GameEvents.GameStarted.AddListener(RestartTimer);
        GameEvents.TurnEnded.AddListener(RestartTimer);

        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        if(timer == null || text == null) return;

        string remainingTime = timer.RemainingTime.ToString("0");
        text.text = remainingTime;
    }

    private void RestartTimer()
    {
        timer?.StopAndKill();
        timer = Timer.Fire(timerDuration, InvokeTurnEndedEvent);
    }

    private void InvokeTurnEndedEvent()
    {
        GameEvents.TurnEnded.Invoke();
    }
}

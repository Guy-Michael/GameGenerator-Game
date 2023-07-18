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
        GameEvents.GameStarted.AddListener(StartTimer);
        GameEvents.TurnEnded.AddListener(RestartTimer);

        text = transform.parent.GetComponentInChildren<TextMeshProUGUI>();
    }

    public float GetElapsedTime()
    {
        return timer.timeElapsed;
    }

    void Update()
    {
        if(timer == null || text == null) return;

        string remainingTime = timer.RemainingTime.ToString("0");
        text.text = remainingTime;
    }

    private void StartTimer()
    {
        timer = Timer.Fire(timerDuration, InvokeTurnEndedEvent);
    }
    private void RestartTimer()
    {
        timer?.StopAndKill();
        StartTimer();
    }

    private void InvokeTurnEndedEvent()
    {
        GameEvents.TurnEnded.Invoke();
    }
}

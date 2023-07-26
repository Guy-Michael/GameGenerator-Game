using System;
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
        text = GetComponent<TextMeshProUGUI>();
    }

    public float GetElapsedTime()
    {
        return timer.timeElapsed;
    }

    void Update()
    {
        if(timer == null || text == null || !timer.active) return;

        TimeSpan timeSpan = TimeSpan.FromSeconds(timer.RemainingTime); 
        string remainingTime = $"00:{timeSpan.Seconds.ToString().PadLeft(2, '0')}";
        // string remainingTime = "0:" + timer.RemainingTime.ToString();
        text.text = remainingTime;
    }

    private void StartTimer()
    {
        timer = Timer.Fire(timerDuration, InvokeTurnEndedEvent);
    }
    public void RestartTimer()
    {
        timer?.StopAndKill();
        StartTimer();
    }

    public void MakeTimerInvisible()
    {
        timer?.Pause();
        text.text = "";
    }

    public void ResumeTimer()
    {
        timer?.Resume();
    }

    private async void InvokeTurnEndedEvent()
    {
        await GameEvents.TurnEnded.Invoke();
    }
}

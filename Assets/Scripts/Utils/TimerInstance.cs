using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerInstance : MonoBehaviour
{
    bool started;
	float timeStarted;
	float timerDuration;
	float timeElapsed;
	Action action;
	public float RemainingTime {get => timerDuration - timeElapsed;}

    private void Update()
	{
		if (!started)
            return;

		TickTime();
	}

	public void Fire(float duration, Action action)
	{
		started = true;
		this.timerDuration = duration;
		this.timeStarted = Time.realtimeSinceStartup;
		this.timeElapsed = 0;
		this.action = action;
	}

	private void TickTime()
	{
		if(timeElapsed < timerDuration)
		{
		    timeElapsed += Time.deltaTime;
            return;
		}

        started = false;
        action.Invoke();
        Destroy(this);
	}

	public void StopAndKill()
	{
		Destroy(this);
	}


}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerInstance : MonoBehaviour
{
    bool active;
	float timeStarted;
	float timerDuration;
	float timeElapsed;
	Action action;
	public float RemainingTime {get => timerDuration - timeElapsed;}

    private void Update()
	{
		if (!active)
            return;

		TickTime();
	}

	public void Fire(float duration)
	{
		active = true;
		this.timerDuration = duration;
		this.timeStarted = Time.realtimeSinceStartup;
		this.timeElapsed = 0;
	}

	public void Fire(float duration, Action action)
	{
		Fire(duration);
		this.action = action;
	}

	private void TickTime()
	{
		if(timeElapsed < timerDuration)
		{
		    timeElapsed += Time.deltaTime;
            return;
		}

        active = false;
        action?.Invoke();
        Destroy(this);
	}

	public void StopAndKill()
	{
		active = false;
		Destroy(this);
	}


}

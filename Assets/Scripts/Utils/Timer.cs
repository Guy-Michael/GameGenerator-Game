using System;
using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
	bool started;
	float timeStarted;
	float timerDuration;
	float timeElapsed;
	Action action;
	static GameObject timerContainer;


	private static TimerInstance getTimerInstance()
	{
		if(timerContainer == null)
		{
			 timerContainer = new GameObject("Timer Container");
		}

		return timerContainer.AddComponent<TimerInstance>();
	}

	public static TimerInstance Fire(float duration)
	{
		TimerInstance timer = getTimerInstance();
		timer.Fire(duration);
		return timer;
	}
	public static TimerInstance Fire(float duration, Action action)
	{
		TimerInstance timer = getTimerInstance();
		timer.Fire(duration, action);
		return timer;
	}
}
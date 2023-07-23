using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Threading.Tasks;
using UnityEngine;

public static class GameEvents
{
    public static UnityEvent PlayerGotMatch = new();
    public static UnityEvent PlayerFailedMatch = new();
    public static UnityEventAsync TurnEnded = new();
    public static UnityEvent GameStarted = new();
    public static UnityEvent<string> GameTypeSelected = new();
    public static UnityEventAsync SetEnded = new();
    public static UnityEvent SetWon = new();
    public static UnityEvent GameWon = new();
}

public class UnityEventAsync: UnityEvent
{
    List<Func<Task>> asyncListeners;
    
    public UnityEventAsync() : base()
    {
        asyncListeners = new();
    }

    public void AddAsyncListener(Func<Task> asyncCallback)
    {
        asyncListeners.Add(asyncCallback);
    }
    
    public new async Task Invoke()
    {
        Task[] tasks = new Task[asyncListeners.Count];
        for(int i = 0; i < asyncListeners.Count; i++)
        {
            Task task = asyncListeners[i]();
            tasks[i] = task;
        }

        await Task.WhenAll(tasks);
        base.Invoke();
    }
}

public class UnityEventAsync<T> : UnityEvent<T>
{
    List<Func<T, Task>> asyncListeners;

    public UnityEventAsync() : base()
    {
        asyncListeners = new();
    }

    public void AddAsyncListener(Func<T, Task> asyncCallback)
    {
        asyncListeners.Add(asyncCallback);
    }
    
    public new void Invoke(T arg)
    {

        base.Invoke(arg);

        Task[] tasks = new Task[asyncListeners.Count];
        for(int i = 0; i < asyncListeners.Count; i++)
        {
            Task task = asyncListeners[i](arg);
            tasks[i] = task;
        }

        Task.WaitAll(tasks);
    }
}
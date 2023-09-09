using System.Globalization;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Threading.Tasks;
using UnityEngine;

public static class GameEvents
{
    public static UnityEvent PlayerMadeMatch = new();
    public static UnityEvent PlayerFailedMatch = new();
    public static UnityEvent TurnEnded = new();
    public static UnityEvent GameStarted = new();
    public static UnityEvent<string> GameTypeSelected = new();
    public static UnityEvent SetEnded = new();
    public static UnityEvent SetWon = new();
    public static UnityEventAsync<SetOutcome> GameEnded = new();

    public static void RemoveAllListeners()
    {
        PlayerMadeMatch.RemoveAllListeners();
        PlayerFailedMatch.RemoveAllListeners();
        TurnEnded.RemoveAllListeners();
        GameStarted.RemoveAllListeners();
        GameTypeSelected.RemoveAllListeners();
        SetEnded.RemoveAllListeners();
        SetWon.RemoveAllListeners();
        GameEnded.RemoveAllListeners();
    } 
}


public class UnityEventAsync: UnityEvent
{
    protected List<Func<Task>> asyncListeners;
    
    public UnityEventAsync() : base()
    {
        asyncListeners = new();
    }

    public void AddAsyncListener(Func<Task> asyncCallback)
    {
        asyncListeners.Add(asyncCallback);
    }
    
    public async Task InvokeAsync()
    {
        Task[] tasks = new Task[asyncListeners.Count];
        for(int i = 0; i < asyncListeners.Count; i++)
        {
            Task task = asyncListeners[i]();
            tasks[i] = task;
        }

        await Task.WhenAll(tasks);
        for(int i = 0; i < GetPersistentEventCount(); i++)
        {
            Debug.Log(GetPersistentMethodName(i));
        }
        base.Invoke();
    }
}

public class UnityEventAsync<T> : UnityEventAsync
{
    protected new List<Func<T, Task>> asyncListeners;

    public UnityEventAsync() : base()
    {
        this.asyncListeners = new();
    }
    public void AddAsyncListener(Func<T, Task> listener)
    {
        asyncListeners.Add(listener);
    }
    public async Task Invoke(T arg)
    {
        Task[] tasks = new Task[asyncListeners.Count];
        var listener = asyncListeners[0];
        for(int i = 0; i < asyncListeners.Count; i++)
        {
            Task task = asyncListeners[i].Invoke(arg);
            tasks[i] = task;
        }

        await Task.WhenAll(tasks);

        base.Invoke();
    }
}



// public class UnityEventAsync<T> : UnityEventAsync
// {
//     List<Func<T, Task>> asyncListeners;

//     public UnityEventAsync() : base()
//     {
//         asyncListeners = new();
//     }

//     public void AddAsyncListener(Func<T, Task> asyncCallback)
//     {
//         asyncListeners.Add(asyncCallback);
//     }
    
//     public void Invoke(T arg)
//     {

//         Invoke(arg);

//         Task[] tasks = new Task[asyncListeners.Count];
//         for(int i = 0; i < asyncListeners.Count; i++)
//         {
//             Task task = asyncListeners[i](arg);
//             tasks[i] = task;
//         }

//         Task.WaitAll(tasks);
//     }
// }
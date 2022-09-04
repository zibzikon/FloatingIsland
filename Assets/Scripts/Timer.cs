using System;
using System.Threading.Tasks;
using UnityEngine;

public struct Timer 
{
    public enum TimerState
    {
        TimeOut,
        Start,
        Working,
        Stop
    }
    
    public TimerState State { get; private set; }
    
    public float RemainingTime { get; private set; }

    public event Action TimeOut;
    
    public void Start(float time)
    {
        RemainingTime = time;
        Start();
    }
    
    public async void Start()
    {
        State = TimerState.Start;
        
        await Task.Yield();
        
        State = TimerState.Working;
        RemainingTime -= Time.deltaTime;

        while (State == TimerState.Working)
        {
            if (RemainingTime <= 0)
                Reset();
            
            await Task.Yield();
            RemainingTime -= Time.deltaTime;
        }
    }

    public void Stop()
    {
        State = TimerState.Stop;
    }
    
    public void Reset()
    {
        RemainingTime = 0;
        State = TimerState.TimeOut;
        TimeOut?.Invoke();
    }
}

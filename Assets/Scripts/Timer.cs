using UnityEngine;

public struct Timer : IUpdatable
{
    private float _time;

    private bool _timerStarted;
    public bool TimeIsOut { get; private set; }
    
    public void Start(float time)
    {
        _time = time;
        TimeIsOut = false;
        _timerStarted = true;
    }
    
    public void OnUpdate()
    {
        if (_timerStarted)
        {
            _time -= Time.deltaTime;
            if (_time <= 0)
            {
                _timerStarted = false;
                TimeIsOut = true;
            }
        }
    }
}

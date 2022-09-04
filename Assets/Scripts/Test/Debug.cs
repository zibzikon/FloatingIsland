using System;
using UnityEngine;

namespace Test
{
    public class Debug : MonoBehaviour
    {
        private Timer _timer;

        [SerializeField] private bool _startTimer;
        [SerializeField] private float time;

        private void Update()
        {
            if (!_startTimer) return;
            _startTimer = false;
            _timer.Start(time);
            _timer.TimeOut += OnTimeOut;
        }

        private void OnTimeOut()
        {
            UnityEngine.Debug.Log("time out");
            _timer.TimeOut -= OnTimeOut;
        }
    }
}
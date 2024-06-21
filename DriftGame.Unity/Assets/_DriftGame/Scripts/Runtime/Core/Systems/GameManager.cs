using System;
using System.Collections;
using System.Collections.Generic;
using DriftGame.Utils;
using UnityEngine;

namespace DriftGame.Systems
{
    public class GameManager : MonoBehaviour
    {
        private Timer _timer;
        private TimeSpan _levelTime = TimeSpan.FromSeconds(1);

        private void Start()
        {
            _timer = new Timer(_levelTime, OnTimerFinish, OnTick);
            _timer.Invoke();
        }

        private void OnTick()
        {
            Debug.Log("Tick");
            if (_timer.PassedTime >= 10)
            {
                _timer.Stop();
            }
        }

        private void OnTimerFinish()
        {
            Debug.Log("Finish");
        }
    }
}

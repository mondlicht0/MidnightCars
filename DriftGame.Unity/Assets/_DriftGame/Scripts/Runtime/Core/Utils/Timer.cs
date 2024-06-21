using System;
using Cysharp.Threading.Tasks;

namespace DriftGame.Utils
{
    public class Timer
    {
        private bool _isRunning;
        private double _time;
        private TimeSpan _refreshInterval;

        private readonly Action _onFinish;
        private readonly Action _onTick;

        public double PassedTime => _time;

        public Timer(TimeSpan refreshInterval, Action onFinish = null, Action onTick = null)
        {
            _isRunning = false;
            _onFinish = onFinish;
            _onTick = onTick;
            _refreshInterval = refreshInterval;
            _time = 0;
        }

        public void Invoke()
        {
            if (!_isRunning)
            {
                _isRunning = true;
                Runner().Forget();
            }
        }

        private async UniTask Runner()
        {
            while (_isRunning)
            {
                await UniTask.Delay(_refreshInterval);
                _time += _refreshInterval.TotalSeconds;
                _onTick?.Invoke();
            }
        }

        public double Stop()
        {
            _isRunning = false;
            _onFinish?.Invoke();
            return (int)_time;
        }
    }
}
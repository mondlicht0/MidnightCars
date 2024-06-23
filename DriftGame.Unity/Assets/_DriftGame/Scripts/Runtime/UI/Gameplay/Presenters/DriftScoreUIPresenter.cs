using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using DriftGame.Network;
using TMPro;
using UnityEngine;

namespace DriftGame.UI
{
    public class DriftScoreUIPresenter : MonoBehaviour
    {
        private CarNetwork _car;
        [SerializeField] private TextMeshProUGUI _totalScoreText;
        [SerializeField] private TextMeshProUGUI _currentScoreText;
        [SerializeField] private TextMeshProUGUI _factorText;
        [SerializeField] private TextMeshProUGUI _driftAngleText;

        [Header("Colors")] 
        [SerializeField] private Color _normalDriftColor;
        [SerializeField] private Color _nearStopColor;
        [SerializeField] private Color _driftEndedColor;
        
        [Header("Drifting")]
        [SerializeField] private float _minimumSpeed = 5;
        [SerializeField] private float _minimumAngle = 10;
        [SerializeField] private float _driftingDelay = 0.2f;

        private Canvas _canvas;
        
        private float _speed = 0;
        private float _driftAngle = 0;
        private float _driftFactor = 1;
        private float _currentScore;
        private float _totalScore;
        private bool _isDrifting = false;
        private IEnumerator _stopDriftingCoroutine;
        private CancellationTokenSource _cancel;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _canvas.gameObject.SetActive(false);
        }

        private void Update()
        {
            HandleDrift();
            HandleUI();
        }

        private void HandleDrift()
        {
            _speed = _car.RigidBody.velocity.magnitude;
            _driftAngle = Vector3.Angle(_car.transform.forward,
                (_car.RigidBody.velocity + _car.transform.forward).normalized);

            if (_driftAngle > 120)
            {
                _driftAngle = 0;
            }

            bool isInDrift = _driftAngle >= _minimumAngle && _speed > _minimumSpeed;

            if (isInDrift)
            {
                if (!_isDrifting || _cancel != null)
                {
                    StartDrift();
                }
            }

            else
            {
                if (_isDrifting && _cancel == null)
                {
                    StopDrift();
                }
            }

            if (_isDrifting)
            {
                _currentScore += Time.deltaTime * _driftAngle * _driftFactor;
                _driftFactor += Time.deltaTime;
                _canvas.gameObject.SetActive(true);
            }
        }

        private async UniTaskVoid StartDrift()
        {
            if (!_isDrifting)
            {
                await UniTask.Delay(Mathf.RoundToInt(1000 * _driftingDelay));
                _driftFactor = 1;
            }

            if (_cancel != null)
            {
                _cancel.Cancel();
                _cancel = null;
            }

            _currentScoreText.color = _normalDriftColor;
            _isDrifting = true;
        }
        
        private async UniTaskVoid StopDrift()
        {
            _cancel = new CancellationTokenSource();
            await StopDriftingTask(_cancel.Token);
        }

        private async UniTask StopDriftingTask(CancellationToken token)
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(0.1f), cancellationToken: token);
                _currentScoreText.color = _nearStopColor;
                await UniTask.Delay(TimeSpan.FromSeconds(_driftingDelay * 4), cancellationToken: token);
                _totalScore += _currentScore;
                _isDrifting = false;
                _currentScoreText.color = _driftEndedColor;
                await UniTask.Delay(TimeSpan.FromSeconds(_driftingDelay * 4), cancellationToken: token);
                _currentScore = 0;
                _canvas.gameObject.SetActive(false);
            }
            catch
            {

            }
            finally
            {
                _cancel = null;
            }
        }

        private void HandleUI()
        {
            _totalScoreText.text = "Total: " + (_totalScore).ToString("###, ###, 000");
            _factorText.text = _driftFactor.ToString("###, ###, ##0.0") + "X";
            _currentScoreText.text = _currentScore.ToString("###, ###, 000");
            _driftAngleText.text = _driftAngle.ToString("###, ##0") + "*";
        }
    }
}

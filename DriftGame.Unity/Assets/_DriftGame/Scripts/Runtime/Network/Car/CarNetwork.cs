using System;
using System.Threading;
using CarOut.Cars.MVP;
using Cysharp.Threading.Tasks;
using DriftGame.Ads;
using DriftGame.Cars;
using DriftGame.Systems;
using DriftGame.Systems.SaveSystem;
using Fusion;
using UnityEngine;

namespace DriftGame.Network
{
    public class CarNetwork : NetworkBehaviour
    {
        [SerializeField] private CarConfig _carData;
        
        [Header("Drifting")]
        [SerializeField] private float _minimumSpeed = 5;
        [SerializeField] private float _minimumAngle = 10;
        [SerializeField] private float _driftingDelay = 0.2f;
        
        private CarVisual _carVisual;
        private CarPresenter _carPresenter;
        private CarController _controller;
        private Vector2 _moveInput;
        private CancellationTokenSource _cancel;
        private AdsManager _adsManager;
        private NetworkGameManager _networkGameManager;
        
        private float _speed;
        public float DriftAngle { get; private set; }
        public float DriftFactor { get; private set; }
        public float CurrentScore { get; private set; }
        public float TotalScore { get; private set; }
        public bool IsDrifting { get; private set; }
        public Rigidbody RigidBody { get; private set; }
        public event Action OnNearStopDrifting;
        public event Action OnEndDrifting;
        public event Action OnEnded;
        
        private void Awake()
        {
            RigidBody = GetComponent<Rigidbody>();
            _controller = GetComponent<CarController>();
            _carVisual = GetComponentInChildren<CarVisual>();
            
            _carPresenter = new CarPresenter.Builder()
                .WithConfig(_carData)
                .Build(_carVisual, _controller);
        }

        private void Start()
        {
            _adsManager = FindFirstObjectByType<AdsManager>();
            _networkGameManager = FindFirstObjectByType<NetworkGameManager>();
            _adsManager.OnGetRewarded += DoubleScore;
        }

        public override void Spawned()
        {
            if (HasInputAuthority)
            {
                DataPersistenceManager.Instance.LoadGameData();
                _carPresenter.Visual.InitVisual();
            }
        }

        public override void FixedUpdateNetwork()
        {
            if (_networkGameManager == null) return;
            
            if (!_networkGameManager.IsGameOver && HasStateAuthority)
            {
                if (GetInput(out NetworkInputData input))
                {
                    _carPresenter.LogicUpdate(input.Direction, false);
                }
            
                _carPresenter.PhysicsUpdate();
                HandleDrift();
            }
        }

        private void DoubleScore()
        {
            TotalScore *= 2;
        }
        
        private void HandleDrift()
        {
            _speed = RigidBody.velocity.magnitude;
            DriftAngle = Vector3.Angle(transform.forward,
                (RigidBody.velocity + transform.forward).normalized);

            if (DriftAngle > 120)
            {
                DriftAngle = 0;
            }

            bool isInDrift = DriftAngle >= _minimumAngle && _speed > _minimumSpeed;

            if (isInDrift)
            {
                if (!IsDrifting || _cancel != null)
                {
                    StartDrift();
                }
            }

            else
            {
                if (IsDrifting && _cancel == null)
                {
                    StopDrift();
                }
            }

            if (IsDrifting)
            {
                CurrentScore += Time.deltaTime * DriftAngle * DriftFactor;
                DriftFactor += Time.deltaTime;
                //_canvas.gameObject.SetActive(true);
            }
        }

        private async UniTaskVoid StartDrift()
        {
            if (!IsDrifting)
            {
                await UniTask.Delay(Mathf.RoundToInt(1000 * _driftingDelay));
                DriftFactor = 1;
            }

            if (_cancel != null)
            {
                _cancel.Cancel();
                _cancel = null;
            }

            //_currentScoreText.color = _normalDriftColor;
            IsDrifting = true;
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
                //_currentScoreText.color = _nearStopColor;
                OnNearStopDrifting?.Invoke();
                await UniTask.Delay(TimeSpan.FromSeconds(_driftingDelay * 4), cancellationToken: token);
                TotalScore += CurrentScore;
                IsDrifting = false;
                //_currentScoreText.color = _driftEndedColor;
                OnEndDrifting?.Invoke();
                await UniTask.Delay(TimeSpan.FromSeconds(_driftingDelay * 4), cancellationToken: token);
                CurrentScore = 0;
                //_canvas.gameObject.SetActive(false);
                OnEnded?.Invoke();
            }
            catch
            {

            }
            finally
            {
                _cancel = null;
            }
        }
    }
}

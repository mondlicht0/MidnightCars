using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DriftGame.Network;
using Fusion;
using Fusion.Sockets;
using TMPro;
using UnityEngine;

namespace DriftGame.Systems
{
    public class NetworkGameManager : SimulationBehaviour, INetworkRunnerCallbacks
    {
        private CarNetwork _car;
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private Canvas _gameOverMenu;

        private TickTimer _timer;
        private float _currentTime;
        
        public bool IsGameOver { get; private set; }

        public event Action OnTimerEnded;

        private const float GameTime = 15f;

        private void Awake()
        {
        }

        public override void FixedUpdateNetwork()
        {
            if (!IsGameOver)
            {
                float remainingTime = _timer.RemainingTime(Runner).Value;
                _timerText.text = FormatTime(remainingTime);

                if (remainingTime <= 0)
                {
                    IsGameOver = true;
                    OnTimerEnded?.Invoke();
                }
            }
        }

        private string FormatTime(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            return $"{minutes:00}:{seconds:00}";
        }

        private void ShowGameOverMenu()
        {
            IsGameOver = true;
            _gameOverMenu.gameObject.SetActive(true);
            _scoreText.text = "Your score: " + _car.TotalScore.ToString("###, ###, 000");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void OnExitButtonPressed()
        {
            Time.timeScale = 1;
            if (Runner != null)
            {
                Runner.Shutdown();
            }
            Application.Quit();
        }

        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }

        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }

        public async void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (player == runner.LocalPlayer)
            {
                _timer = TickTimer.CreateFromSeconds(Runner, GameTime);
                await UniTask.Delay(1000);
                runner.TryGetPlayerObject(player, out var playerObject);
                _car = playerObject.GetComponent<CarNetwork>();
                _timerText.text = FormatTime(120);
                _gameOverMenu.gameObject.SetActive(false);
                
                OnTimerEnded += ShowGameOverMenu;
            }
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
            runner.AddCallbacks(this);
        }

        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
        {
            runner.RemoveCallbacks(this);
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
        {
            
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
        {
        }

        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
        {
        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
        }
    }
}

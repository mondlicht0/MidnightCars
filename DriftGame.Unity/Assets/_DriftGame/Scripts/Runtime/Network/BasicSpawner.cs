using System;
using System.Collections.Generic;
using DriftGame.UI;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DriftGame.Network
{
    public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
    {
        public static BasicSpawner Instance { get; private set; }

        [SerializeField] private GameOverUIPresenter _gameOverUIPresenter;
        [SerializeField] private NetworkPrefabRef _playerPrefab;
        private Dictionary<PlayerRef, NetworkObject> _spawnedPlayers = new();
        private NetworkRunner _runner;
        private InputHandler _inputHandler;
        private NetworkInputData _accumulatedInput;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }

            _gameOverUIPresenter.OnLevelRetry += () => _runner.Shutdown();
            _gameOverUIPresenter.OnMenu += () => _runner.Shutdown();
        }
        
        private async void StartGame(GameMode mode)
        {
            _runner = gameObject.AddComponent<NetworkRunner>();
            _runner.ProvideInput = true;

            var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
            var sceneInfo = new NetworkSceneInfo();
            if (scene.IsValid) {
                sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
            }

            await _runner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                SessionName = "TestRoom",
                Scene = scene,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
        }
        
        public NetworkObject GetPlayerNetworkObject(PlayerRef player){
            if (_runner.TryGetPlayerObject(player, out var plObject)){
                return plObject.GetComponent<NetworkObject>();
            }

            Debug.LogError("Player Object Not found");
            return null;
        }
        
        private void OnGUI()
        {
            if (_runner == null)
            {
                if (GUI.Button(new Rect(0,0,300,50), "Host"))
                {
                    StartGame(GameMode.Host);
                }
                if (GUI.Button(new Rect(0,40,300,50), "Join"))
                {
                    StartGame(GameMode.Client);
                }
                
                if (GUI.Button(new Rect(0,80,300,50), "Offline"))
                {
                    StartGame(GameMode.Single);
                }
            }
        }

        #region NetworkRunnerCallbacks

        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }

        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer)
            {
                Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3, 1, 0);
                NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
                runner.SetPlayerObject(player, networkPlayerObject);
            }
        }

        private void SetPlayerObject()
        {
            
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (_spawnedPlayers.TryGetValue(player, out NetworkObject networkObject))
            {
                runner.Despawn(networkObject);
                _spawnedPlayers.Remove(player);
            }
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
        }

        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
        {
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

        #endregion
    }
    
    
}

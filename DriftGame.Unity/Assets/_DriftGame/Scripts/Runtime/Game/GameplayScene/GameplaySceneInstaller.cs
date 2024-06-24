using DriftGame.Systems;
using UnityEngine;
using Zenject;

namespace DriftGame
{
    public class GameplaySceneInstaller : MonoInstaller
    {
        [SerializeField] private NetworkGameManager _networkGameManager;

        public override void InstallBindings()
        {
            Container.Bind<NetworkGameManager>().FromInstance(_networkGameManager).AsSingle();
        }

        public void Run(UIRoot uiRoot)
        {
            Debug.Log("Gameplay Scene Loaded");
        }
    }
}
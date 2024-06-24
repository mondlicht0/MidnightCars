using Cysharp.Threading.Tasks;
using DriftGame.Ads;
using DriftGame.Systems;
using UnityEngine;
using Zenject;

namespace DriftGame
{
    public class GameplaySceneInstaller : MonoInstaller, IRunScene
    {
        private UIRoot _uiRoot;
        
        [SerializeField] private NetworkGameManager _networkGameManager;
        [SerializeField] private AdsManager _adsManager;
        [SerializeField] private LevelManager _levelManager;

        public override async void InstallBindings()
        {
            Container.Bind<UIRoot>().FromInstance(_uiRoot).AsSingle().NonLazy();
            Container.Bind<NetworkGameManager>().FromInstance(_networkGameManager).AsSingle();
            Container.Bind<AdsManager>().FromInstance(_adsManager).AsSingle();
            Container.Bind<LevelManager>().FromInstance(_levelManager).AsSingle();
            Debug.Log("Gameplay");
        }

        public void Run(UIRoot uiRoot)
        {
            _uiRoot = uiRoot;
            Debug.Log("Gameplay Scene Loaded - " + _uiRoot == null);
        }
    }
}
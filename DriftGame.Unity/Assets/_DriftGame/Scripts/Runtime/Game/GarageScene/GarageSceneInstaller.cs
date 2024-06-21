using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Object = UnityEngine.Object;

namespace DriftGame
{
    public class GarageSceneInstaller : MonoInstaller
    {
        [SerializeField] private MainMenuPresenter _mainMenu;
        private UIRoot _uiRoot;
        
        public override void InstallBindings()
        {
            
        }

        public void Run(UIRoot uiRoot)
        {
            _uiRoot = uiRoot;
            Debug.Log("Garage scene loaded");
            _mainMenu.OnLevel += LoadGameplay;
        }

        private async void LoadGameplay()
        {
            _uiRoot.ShowLoadingScreen();
            await SceneManager.LoadSceneAsync(Scenes.Gameplay);
            await UniTask.Delay(TimeSpan.FromSeconds(1));

            var sceneInstaller = Object.FindFirstObjectByType<GameplaySceneInstaller>();
            sceneInstaller.Run(_uiRoot);
            
            _uiRoot.HideLoadingScreen();
        }
    }
}
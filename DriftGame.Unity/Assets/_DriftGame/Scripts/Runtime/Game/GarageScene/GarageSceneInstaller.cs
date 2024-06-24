using System;
using Cysharp.Threading.Tasks;
using DriftGame.Systems;
using DriftGame.Systems.SaveSystem;
using DriftGame.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace DriftGame
{
    public class GarageSceneInstaller : MonoInstaller, IRunScene
    {
        [SerializeField] private MainMenuPresenter _mainMenu;
        [SerializeField] private DataPersistenceManager _dataPersistence;
        private UIRoot _uiRoot;
        
        public override void InstallBindings()
        {
            
        }

        private void Start()
        {
            _mainMenu.OnLevel += LoadGameplay;
        }

        public void Run(UIRoot uiRoot)
        {
            _uiRoot = uiRoot;
            Debug.Log("Garage scene loaded");
        }

        private async void LoadGameplay()
        {
            _uiRoot.ShowLoadingScreen();
            await SceneManager.LoadSceneAsync(Scenes.Gameplay);
            await UniTask.Delay(TimeSpan.FromSeconds(1));

            var sceneInstaller = FindFirstObjectByType<GameplaySceneInstaller>();
            sceneInstaller.Run(_uiRoot);
            
            _uiRoot.HideLoadingScreen();
        }
    }
}
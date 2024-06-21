using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace DriftGame.Systems
{
    public class GameEntryPoint
    {
        private static GameEntryPoint _instance;
        private UIRoot _uiRoot;

        private GameEntryPoint()
        {
            UIRoot uiRootPrefab = Resources.Load<UIRoot>("UIRoot");
            _uiRoot = Object.Instantiate(uiRootPrefab);
            Object.DontDestroyOnLoad(_uiRoot.gameObject);
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void LoadGame()
        {
            SetupMobileSettings();
            _instance = new GameEntryPoint();
            _instance.RunGame();
        }

        private static void SetupMobileSettings()
        {
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        private void RunGame()
        {
#if UNITY_EDITOR
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName == Scenes.Gameplay)
            {
                return;
            }

            if (sceneName != Scenes.Boot)
            {
                return;
            }
#endif
            LoadGarage();
        }

        private async UniTask LoadGarage()
        {
            _uiRoot.ShowLoadingScreen();
            await SceneManager.LoadSceneAsync(Scenes.Boot);
            await SceneManager.LoadSceneAsync(Scenes.Garage);
            await UniTask.Delay(TimeSpan.FromSeconds(1));

            var sceneInstaller = Object.FindFirstObjectByType<GarageSceneInstaller>();
            sceneInstaller.Run(_uiRoot);
            
            _uiRoot.HideLoadingScreen();
        }
    }
}

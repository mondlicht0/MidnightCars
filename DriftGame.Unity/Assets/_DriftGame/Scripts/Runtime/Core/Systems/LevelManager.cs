using System;
using Cysharp.Threading.Tasks;
using DriftGame;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace _DriftGame.Scripts.Runtime.Core.Systems
{
    public class LevelManager : MonoBehaviour
    {
        private UIRoot _uiRoot;
        
        public async UniTask LoadLevel()
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
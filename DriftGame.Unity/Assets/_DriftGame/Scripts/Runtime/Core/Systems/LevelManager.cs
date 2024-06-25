using System;
using Cysharp.Threading.Tasks;
using DriftGame.UI;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DriftGame.Systems
{
    public class LevelManager : NetworkBehaviour
    {
        [SerializeField] private GameOverUIPresenter _gameOverUIPresenter;

        private void Awake()
        {
            _gameOverUIPresenter.OnLevelRetry += () => LoadLevel("Gameplay");
        }

        public async UniTask LoadLevel(string sceneName)
        {
            UIRoot.Instance.ShowLoadingScreen();
            await SceneManager.LoadSceneAsync(sceneName);
            await UniTask.Delay(TimeSpan.FromSeconds(1));

            var sceneInstaller = FindObjectOfType<MonoBehaviour>();
            sceneInstaller.TryGetComponent(out IRunScene scene);
            UIRoot.Instance.HideLoadingScreen();
            scene.Run(UIRoot.Instance);
        }
    }
}
using System;
using DriftGame.Ads;
using DriftGame.Systems;
using Fusion;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DriftGame.UI
{
    public class GameOverUIPresenter : NetworkBehaviour
    {
        [SerializeField] private Button _retryButton;
        [SerializeField] private Button _menuButton;
        [SerializeField] private Button _doubleRewardButton;

        private AdsManager _adsManager;
        private LevelManager _levelManager;

        public event Action OnLevelRetry;
        
        [Inject]
        private void Construct(AdsManager adsManager, LevelManager levelManager)
        {
            _adsManager = adsManager;
            _levelManager = levelManager;
        }
        
        private void OnEnable()
        {
            _retryButton.onClick.AddListener(() => OnLevelRetry?.Invoke());
            _menuButton.onClick.AddListener(() => _levelManager.LoadLevel(Scenes.Garage));
            _doubleRewardButton.onClick.AddListener(_adsManager.ShowRewardedAd);
        }

        private void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}

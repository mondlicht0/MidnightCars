using DriftGame.Network;
using Fusion;
using TMPro;
using UnityEngine;

namespace DriftGame.UI
{
    public class DriftScoreUIPresenter : NetworkBehaviour
    {
        private CarNetwork _car;
        
        [SerializeField] private TextMeshProUGUI _totalScoreText;
        [SerializeField] private TextMeshProUGUI _currentScoreText;
        [SerializeField] private TextMeshProUGUI _factorText;
        [SerializeField] private TextMeshProUGUI _driftAngleText;

        [Header("Colors")] 
        [SerializeField] private Color _normalDriftColor;
        [SerializeField] private Color _nearStopColor;
        [SerializeField] private Color _driftEndedColor;

        [SerializeField] private Canvas _canvas;
        
        private void Awake()
        {
            _car = GetComponent<CarNetwork>();

            _car.OnNearStopDrifting += () => _currentScoreText.color = _nearStopColor;
            _car.OnEndDrifting += () => _currentScoreText.color = _driftEndedColor;
            _car.OnEnded += () => _canvas.gameObject.SetActive(false);
        }

        public override void FixedUpdateNetwork()
        {
            HandleUI();
        }

        private void HandleUI()
        {
            _totalScoreText.text = "Total: " + _car.TotalScore.ToString("###, ###, 000");
            _factorText.text = _car.DriftFactor.ToString("###, ###, ##0.0") + "X";
            _currentScoreText.text = _car.CurrentScore.ToString("###, ###, 000");
            _driftAngleText.text = _car.DriftAngle.ToString("###, ##0") + "*";
            
            if (_car.IsDrifting)
            {
                _canvas.gameObject.SetActive(true);
            }
        }
    }
}

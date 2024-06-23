using System;
using UnityEngine.UIElements;

namespace DriftGame.UI
{
    public class LevelsTabPresenter : BasePresenter
    {
        private VisualElement _firstLevel;
        private VisualElement _secondLevel;
        private VisualElement _thirdLevel;
        private VisualElement _fourthLevel;

        private Button _hostButton;
        private Button _joinButton;
        private Button _offlineButton;

        public Action OnLevel;
        
        public LevelsTabPresenter(VisualElement rootElement) : base(rootElement)
        {
            InitPage();
        }

        protected override void InitPage()
        {
            _firstLevel = RootElement.Q("FirstLevel");
            _secondLevel = RootElement.Q("SecondLevel");
            _thirdLevel = RootElement.Q("ThirdLevel");
            _fourthLevel = RootElement.Q("FourthLevel");

            _firstLevel.RegisterCallback<ClickEvent>(evt => OnLevel?.Invoke());
            _secondLevel.RegisterCallback<ClickEvent>(evt => OnLevel?.Invoke());
            _thirdLevel.RegisterCallback<ClickEvent>(evt => OnLevel?.Invoke());
            _fourthLevel.RegisterCallback<ClickEvent>(evt => OnLevel?.Invoke());
        }
    }
}

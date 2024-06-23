using System;
using System.Collections.Generic;
using System.Linq;
using CarOut.Cars.MVP;
using DriftGame.Cars;
using UnityEngine;
using UnityEngine.UIElements;

namespace DriftGame.UI
{
    public class MainMenuPresenter : MonoBehaviour
    {
        [SerializeField] private CarVisual _carVisual;
        private CarPresenter _carPresenter;
        private UIDocument _uiDocument;

        private VisualElement RootElement => _uiDocument.rootVisualElement;

        private CustomizationTabPresenter _customizationTab;
        private LevelsTabPresenter _levelsTab;
        private SettingsTabPresenter _settingsTab;
        private CarShopTabPresenter _carShopTab;

        private VisualElement _customizationTabView;
        private VisualElement _levelsTabView;
        private VisualElement _settingsTabView;
        private VisualElement _carShopTabView;

        private VisualElement _customizationHeaderTab;
        private VisualElement _levelsHeaderTab;
        private VisualElement _settingsHeaderTab;
        private VisualElement _carShopHaderTab;

        private List<BasePresenter> _presenters = new();

        public bool Online;

        public event Action OnLevel;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
        }

        private void Start()
        {
            _carPresenter = new CarPresenter.Builder()
                .WithConfig(null)
                .Build(_carVisual, null);
            
            InitLevelsTab();
            InitSettingsTab();
            InitCarShopTab();
            InitCustomizationTab();
            InitTransitions();
        }

        private void InitTransitions()
        {
            _carShopHaderTab = RootElement.Q("CarShopTab");
            _settingsHeaderTab = RootElement.Q("SettingsTab");
            _customizationHeaderTab = RootElement.Q("CustomizationTab");
            _levelsHeaderTab = RootElement.Q("LevelsTab");
            
            _carShopHaderTab.RegisterCallback<ClickEvent>(evt => _carShopTab.ToggleTo(_presenters.ToArray()));
            _settingsHeaderTab.RegisterCallback<ClickEvent>(evt => _settingsTab.ToggleTo(_presenters.ToArray()));
            _customizationHeaderTab.RegisterCallback<ClickEvent>(evt => _customizationTab.ToggleTo(_presenters.ToArray()));
            _levelsHeaderTab.RegisterCallback<ClickEvent>(evt => _levelsTab.ToggleTo(_presenters.ToArray()));
        }

        private void InitSettingsTab()
        {
            _settingsTabView = RootElement.Q("Settings");
            _settingsTab = new SettingsTabPresenter(_settingsTabView);
            _presenters.Add(_settingsTab);
        }

        private void InitCarShopTab()
        {
            _carShopTabView = RootElement.Q("CarShop");
            _carShopTab = new CarShopTabPresenter(_carShopTabView);
            _presenters.Add(_carShopTab);
        }

        private void InitCustomizationTab()
        {
            _customizationTabView = RootElement.Q("Customization");
            _customizationTab = new CustomizationTabPresenter(_customizationTabView, _carPresenter);
            _presenters.Add(_customizationTab);
        }

        private void InitLevelsTab()
        {
            _levelsTabView = RootElement.Q("Levels");
            _levelsTab = new LevelsTabPresenter(_levelsTabView);
            _levelsTab.OnLevel += () => OnLevel?.Invoke();
            _presenters.Add(_levelsTab);
            Debug.Log("дерево");
        }
    }
}

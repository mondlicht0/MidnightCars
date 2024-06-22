using System;
using CarOut.Cars.MVP;
using UnityEngine;
using UnityEngine.UIElements;

namespace DriftGame.UI
{
    public class MainMenuPresenter : MonoBehaviour
    {
        [SerializeField] private CarVisual _carVisual;
        private UIDocument _uiDocument;

        private VisualElement RootElement => _uiDocument.rootVisualElement;

        private VisualElement _levelTab;
        private VisualElement _customizationTab;
        private VisualElement _settingsTab;
        private VisualElement _carShopTab;

        private ColorPickerElement _colorPicker;
        private VisualElement _upgradeEngineOption;
        private VisualElement _rearSpoilerOption;

        private VisualElement _firstLevel;
        private VisualElement _secondLevel;
        private VisualElement _thirdLevel;
        private VisualElement _fourthLevel;

        public bool Online;

        public event Action OnLevel;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
        }

        private void Start()
        {
            InitLevelsTab();
            InitHeaderTabs();
            InitCustomizationTab();
        }

        private void InitCustomizationTab()
        {
            _colorPicker = RootElement.Q<ColorPickerElement>();
            _colorPicker.OnColorPicked += _carVisual.ChangeColor;
            
            
        }

        private void InitHeaderTabs()
        {
            _levelTab = RootElement.Q("LevelTab");
            _customizationTab = RootElement.Q("CustomizationTab");
            _settingsTab = RootElement.Q("SettingsTab");
            _carShopTab = RootElement.Q("CarShopTab");
        }

        private void InitLevelsTab()
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

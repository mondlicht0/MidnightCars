using DriftGame.Cars;
using UnityEngine.UIElements;

namespace DriftGame.UI
{
    public class CustomizationTabPresenter : BasePresenter
    {
        private ColorPickerElement _colorPicker;
        private VisualElement _upgradeEngineOption;
        private VisualElement _rearSpoilerOption;

        private CarPresenter _carPresenter;
        
        public CustomizationTabPresenter(VisualElement rootElement, CarPresenter carPresenter) : base(rootElement)
        {
            _carPresenter = carPresenter;
            InitPage();
        }

        protected override void InitPage()
        {
            _colorPicker = RootElement.Q<ColorPickerElement>();
            _colorPicker.OnColorPicked += _carPresenter.Visual.ChangeColor;

            _rearSpoilerOption = RootElement.Q("RearSpoilerOption");
            _rearSpoilerOption.RegisterCallback<ClickEvent>(evt => _carPresenter.Visual.AddSpoiler());

            _upgradeEngineOption = RootElement.Q("UpgradeEngine");
            _upgradeEngineOption.RegisterCallback<ClickEvent>(evt => _carPresenter.UpgradeEngine());
        }
    }
}

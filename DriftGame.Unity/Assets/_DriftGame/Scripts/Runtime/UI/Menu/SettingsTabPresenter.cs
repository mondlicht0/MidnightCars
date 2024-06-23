using UnityEngine.UIElements;

namespace DriftGame.UI
{
    public class SettingsTabPresenter : BasePresenter
    {
        private Slider _soundVolume;
        private Slider _musicVolume;
        
        public SettingsTabPresenter(VisualElement rootElement) : base(rootElement)
        {
            InitPage();
        }

        protected override void InitPage()
        {
            
        }
    }
}

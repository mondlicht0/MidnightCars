using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuPresenter : MonoBehaviour
{
    private UIDocument _uiDocument;

    private VisualElement _firstLevel;
    private VisualElement _secondLevel;
    private VisualElement _thirdLevel;
    private VisualElement _fourthLevel;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
    }

    private void InitElements()
    {
        
    }
}

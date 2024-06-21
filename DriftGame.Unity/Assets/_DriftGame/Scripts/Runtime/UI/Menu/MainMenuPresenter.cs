using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuPresenter : MonoBehaviour
{
    private UIDocument _uiDocument;

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
    }

    private void InitLevelsTab()
    {
        _firstLevel = _uiDocument.rootVisualElement.Q("FirstLevel");
        _secondLevel = _uiDocument.rootVisualElement.Q("SecondLevel");
        _thirdLevel = _uiDocument.rootVisualElement.Q("ThirdLevel");
        _fourthLevel = _uiDocument.rootVisualElement.Q("FourthLevel");
        
        _firstLevel.RegisterCallback<ClickEvent>(evt => OnLevel?.Invoke());
        _secondLevel.RegisterCallback<ClickEvent>(evt => Debug.Log("DFDF"));
        _thirdLevel.RegisterCallback<ClickEvent>(evt => Debug.Log("DFDF"));
        _fourthLevel.RegisterCallback<ClickEvent>(evt => Debug.Log("DFDF"));
    }
}

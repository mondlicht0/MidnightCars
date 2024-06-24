using UnityEngine;

public class UIRoot : MonoBehaviour
{
    public static UIRoot Instance { get; private set; }
    
    [SerializeField] private Canvas _loadingScreen;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        HideLoadingScreen();
    }

    public void ShowLoadingScreen()
    {
        _loadingScreen.gameObject.SetActive(true);
    }
    
    public void HideLoadingScreen()
    {
        _loadingScreen.gameObject.SetActive(false);
    }
}

using System.Collections.Generic;
using UnityEngine;

public class UIScreenNavigator : MonoBehaviour
{
    public static UIScreenNavigator Instance { get; private set; }

    [Header("Screen/Popup Root Parents")]
    [SerializeField] private Transform screenRoot;
    [SerializeField] private Transform popupRoot;

    [Header("Initial Screen")]
    [SerializeField] private UIScreen initialScreen;

    [Header("Populated at Runtime")]
    [SerializeField] private UIScreen[] screens;
    [SerializeField] private UIPopup[] popups;

    private Stack<UIScreen> screenHistory = new();
    private Stack<UIPopup> popupHistory = new();

    private UIScreen currentScreen;
    private UIPopup currentPopup;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

       // PopulateScreens();
    }

    private void Start()
    {
        ShowInitialScreen();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) GoBack();
    }

    private void PopulateScreens()
    {
        if (screenRoot == null || popupRoot == null)
        {
            Debug.LogError("❌ ScreenRoot or PopupRoot not assigned.");
            return;
        }

        screens = screenRoot.GetComponentsInChildren<UIScreen>(true);
        popups = popupRoot.GetComponentsInChildren<UIPopup>(true);

        foreach (var s in screens) s.Hide();
        foreach (var p in popups) p.Hide();
    }

    private void ShowInitialScreen()
    {
        if (initialScreen != null)
        {
            currentScreen = initialScreen;
            currentScreen.Show();
        }
        else if (screens.Length > 0)
        {
            currentScreen = screens[0];
            currentScreen.Show();
        }
        else
        {
            Debug.LogWarning("⚠️ No initial screen found.");
        }
    }

    public void ShowScreen(UIScreen screen)
    {
        if (screen == null) return;

        if (currentScreen != null && currentScreen != screen)
        {
            currentScreen.Hide();
            screenHistory.Push(currentScreen);
        }

        currentScreen = screen;
        currentScreen.Show();
    }

    public void ShowPopup(UIPopup popup)
    {
        if (popup == null) return;

        if (currentPopup != null && currentPopup != popup)
        {
            currentPopup.Hide();
            popupHistory.Push(currentPopup);
        }

        currentPopup = popup;
        currentPopup.Show();
    }

    public void HidePopup(UIPopup popup)
    {
        if (popup == null) return;

        popup.Hide();
        currentPopup = null;
    }

    public void GoBack()
    {
        
        if (popupHistory.Count > 0)
        {
            currentPopup?.Hide();
            currentPopup = popupHistory.Pop();
            currentPopup?.Show();
        }
        else if (currentPopup != null)
        {
            currentPopup.Hide();
            currentPopup = null;
        }
        else if (screenHistory.Count > 0)
        {
            if (!currentScreen.isHomeScreen)
            {
                currentScreen?.Hide();
                currentScreen = screenHistory.Pop();
                currentScreen?.Show();
            }
            
        }
        else
        {
            Debug.Log("📍 Already at the root screen.");
        }
    }

    public void ClearHistory()
    {
        screenHistory.Clear();
        popupHistory.Clear();
    }

    // Optional for debugging
    public UIScreen[] GetScreens() => screens;
    public UIPopup[] GetPopups() => popups;
}

using System.Collections.Generic;
using UnityEngine;

public class ScreenNavigator : MonoBehaviour
{
    public static ScreenNavigator Instance { get; private set; }

    [Header("All Screens and Popups")]
    [Tooltip("Assign screen objects manually or auto-scan")]
    [SerializeField] private UIScreen[] screens;
    [SerializeField] private UIPopup[] popups;

    private readonly Dictionary<ScreenId, GameObject> screenMap = new();
    private readonly Dictionary<popupId, GameObject> popupMap = new();
    private readonly List<GameObject> openPopups = new();
    private readonly Stack<ScreenId> screenHistory = new();

    private ScreenId currentScreen;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); return; }

        RegisterScreens();
    }

    private void RegisterScreens()
    {
        screenMap.Clear();

        foreach (var screen in screens)
        {
            if (screen != null && screen.gameObject != null)
            {
                if (!screenMap.ContainsKey(screen.screenId))
                    screenMap[screen.screenId] = screen.gameObject;
            }
        }
        foreach (var popup in popups)
        {
            if (popup != null && popup.gameObject != null)
            {
                if (!popupMap.ContainsKey(popup.popupId))
                    popupMap[popup.popupId] = popup.gameObject;
            }
        }

        // Find and show initial home screen
        foreach (var screen in screens)
        {
            if (screen.isSplash)
            {
                NavigateTo(screen.screenId, false); // don’t push first time
                break;
            }
        }
    }

    /// <summary>
    /// Navigate to a screen
    /// </summary>
    public void NavigateTo(ScreenId screenId, bool addToHistory = true)
    {
        foreach (var kvp in screenMap)
        {
            kvp.Value.SetActive(kvp.Key == screenId);
        }

        if (addToHistory && currentScreen != screenId)
        {
            screenHistory.Push(currentScreen);
        }

        currentScreen = screenId;

        Debug.Log($"🧭 Navigated to: {screenId}");
    }

    /// <summary>
    /// Used by UI buttons
    /// </summary>
    public void NavigateToById(int screenIdValue)
    {
        NavigateTo((ScreenId)screenIdValue);
    }

    public void ShowPopup(popupId popup)
    {
        if (popupMap.TryGetValue(popup, out var popupGO))
        {
            popupGO.SetActive(true);
            if (!openPopups.Contains(popupGO))
                openPopups.Add(popupGO);
        }
    }

    public void ClosePopup(popupId popup)
    {
        if (popupMap.TryGetValue(popup, out var popupGO))
        {
            popupGO.SetActive(false);
            openPopups.Remove(popupGO);
        }
    }

    public void CloseAllPopups()
    {
        foreach (var popup in openPopups)
            popup.SetActive(false);
        openPopups.Clear();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Works on Android too
        {
            GoBack();
        }
    }
    public GameObject GetScreen(ScreenId id)
    {
        return screenMap.TryGetValue(id, out var obj) ? obj : null;
    }

    /// <summary>
    /// 🔙 Back to previous screen, unless we're on home
    /// </summary>
    public void GoBack()
    {
        UIScreen current = GetUIScreen(currentScreen);
        if (current != null && current.isHomeScreen)
        {
            Debug.Log("🏠 Already at home screen. Back ignored.");
            return;
        }

        if (screenHistory.Count > 0)
        {
            ScreenId previous = screenHistory.Pop();
            NavigateTo(previous, false);
        }
        else
        {
            Debug.Log("🔚 No previous screen in stack.");
        }
    }

    private UIScreen GetUIScreen(ScreenId id)
    {
        foreach (var screen in screens)
            if (screen.screenId == id)
                return screen;
        return null;
    }

    public ScreenId GetCurrentScreen() => currentScreen;
}

using System;
using System.Collections;
using UnityEngine;

public class AuthLoginManager : MonoBehaviour
{
    public static AuthLoginManager Instance { get; private set; }
    public FirebaseUserData CurrentUser { get; private set; }

    public event Action<FirebaseUserData> OnLoginSuccess;
    public event Action<string> OnLoginError;

    private IAuthProvider authProvider;
    private ITokenCacheService tokenCache;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            tokenCache = new PlayerPrefsTokenCacheService();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(InitializeSession());
    }

    public void SetAuthProvider(IAuthProvider provider)
    {
        authProvider = provider;
    }

    public void LoginWithProvider(IAuthProvider provider)
    {
        SetAuthProvider(provider);
        StartCoroutine(HandleLogin(provider));
    }

    private IEnumerator HandleLogin(IAuthProvider provider)
    {
        yield return provider.SignIn(
            user =>
            {
                CurrentUser = user;
                tokenCache.SaveUser(user);
                Debug.Log("✅ Logged in: " + user.localId);
                OnLoginSuccess?.Invoke(user);
            },
            error =>
            {
                Debug.LogError("❌ Login error: " + error);
                OnLoginError?.Invoke(error);
            });
    }

    private IEnumerator InitializeSession()
    {
        if (!tokenCache.HasCachedUser())
        {
            Debug.Log("🔁 No cached user found. Awaiting login.");
            yield break;
        }

        CurrentUser = tokenCache.LoadCachedUser();
        Debug.Log("🧠 Cached user loaded: " + CurrentUser.localId);

        yield return FirebaseTokenRefresher.RefreshToken(
            CurrentUser.refreshToken,
            refreshed =>
            {
                CurrentUser = refreshed;
                tokenCache.SaveUser(refreshed);
                Debug.Log("♻️ Token refreshed");
                OnLoginSuccess?.Invoke(refreshed);
                ToastManager.Instance.ShowToast("Signed In Successfully!", ToastData.ToastType.Success);
            },
            error =>
            {
                Debug.LogError("Token refresh failed: " + error);
                tokenCache.ClearCache();
                OnLoginError?.Invoke("Token expired. Please log in again.");
            });
    }

    public void Logout()
    {
        tokenCache.ClearCache();
        CurrentUser = null;
        Debug.Log("🚪 Logged out and cache cleared");
    }
}

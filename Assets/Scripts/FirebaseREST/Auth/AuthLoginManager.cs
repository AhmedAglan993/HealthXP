using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class AuthLoginManager : MonoBehaviour
{
    public static AuthLoginManager Instance { get; private set; }
    public FirebaseUserData CurrentUser;
    public event Action<FirebaseUserData> OnLoginSuccess;
    public event Action<string> OnLoginError;
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
    public void AutoLoginWithCashedUser()
    {
        StartCoroutine(InitializeSession());
    }


    public void LoginWithProvider(IAuthProvider provider)
    {
        StartCoroutine(HandleLogin(provider));
    }

    private IEnumerator HandleLogin(IAuthProvider provider)
    {
        LoadingManager.Service.Show("Logining In");
        yield return provider.SignIn(
            user =>
            {
                CurrentUser = user;
                tokenCache.SaveUser(user);
                Debug.Log("✅ Logged in: " + user.localId);
                OnLoginSuccess?.Invoke(user);
                ToastManager.Instance.ShowToast("Signed In Successfully!", ToastData.ToastType.Success);
                LoadingManager.Service.Hide();
                UserSessionManager.Instance.OnLoginSuccess(user);
            },
            error =>
            {
                Debug.LogError("❌ Login error: " + error);
                ToastManager.Instance.ShowToast("❌ Login error!", ToastData.ToastType.Error);
                OnLoginError?.Invoke(error);
                LoadingManager.Service.Hide();
            });
    }

    private IEnumerator InitializeSession()
    {
        if (!tokenCache.HasCachedUser())
        {
            Debug.Log("🔁 No cached user found. Awaiting login.");
            ScreenNavigator.Instance.NavigateTo(ScreenId.RoleSelection);
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
                UserSessionManager.Instance.OnLoginSuccess(CurrentUser);
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

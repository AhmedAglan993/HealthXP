using System;
using System.Collections;
using UnityEngine;

public class FirebaseAuthManager : MonoBehaviour
{
    public static FirebaseAuthManager Instance { get; private set; }

    public FirebaseUserData CurrentUser { get; private set; }
    public event Action<FirebaseUserData> OnUserSignedIn;

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

    public void SetAuthProvider(IAuthProvider provider)
    {
        authProvider = provider;
    }

    public void StartAuthentication()
    {
        if (authProvider == null)
        {
            Debug.LogError("❌ AuthProvider not set! Use SetAuthProvider() before authentication.");
            return;
        }
        StartCoroutine(InitializeSession());
    }

    private IEnumerator InitializeSession()
    {
        if (tokenCache.HasCachedUser())
        {
            Debug.Log("🧠 Loaded cached user");
            CurrentUser = tokenCache.LoadCachedUser();
            Debug.Log(CurrentUser.localId);

            yield return FirebaseTokenRefresher.RefreshToken(
                CurrentUser.refreshToken,
                refreshedUser =>
                {
                    CurrentUser = refreshedUser;
                    tokenCache.SaveUser(refreshedUser);
                    Debug.Log("🔁 Token refreshed");
                    OnUserSignedIn?.Invoke(CurrentUser);

                },
                error =>
                {
                    Debug.LogError("Token refresh failed: " + error);
                    ToastManager.Instance.ShowToast("Error Signing In!", ToastData.ToastType.Error);
                });

        }
        else
        {
            Debug.Log("📡 Signing in...");
            yield return authProvider.SignIn(
                user =>
                {
                    CurrentUser = user;
                    tokenCache.SaveUser(user);
                    Debug.Log("✅ User signed in: " + user.localId);
                    OnUserSignedIn?.Invoke(user);
                },
                error =>
                {
                    Debug.LogError("❌ Auth failed: " + error);
                });
        }
    }

    public void Logout()
    {
        tokenCache.ClearCache();
        CurrentUser = null;
        Debug.Log("👋 Logged out and cleared cache");
    }
}

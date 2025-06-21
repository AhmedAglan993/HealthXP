using UnityEngine;

public class PlayerPrefsTokenCacheService : ITokenCacheService
{
    private const string KEY_ID = "auth_localId";
    private const string KEY_TOKEN = "auth_idToken";
    private const string KEY_REFRESH = "auth_refreshToken";

    public bool HasCachedUser() => PlayerPrefs.HasKey(KEY_ID);

    public FirebaseUserData LoadCachedUser()
    {
        return new FirebaseUserData
        {
            localId = PlayerPrefs.GetString(KEY_ID),
            idToken = PlayerPrefs.GetString(KEY_TOKEN),
            refreshToken = PlayerPrefs.GetString(KEY_REFRESH)
        };
    }

    public void SaveUser(FirebaseUserData user)
    {
        PlayerPrefs.SetString(KEY_ID, user.localId);
        PlayerPrefs.SetString(KEY_TOKEN, user.idToken);
        PlayerPrefs.SetString(KEY_REFRESH, user.refreshToken);
        PlayerPrefs.Save();
    }

    public void ClearCache()
    {
        PlayerPrefs.DeleteKey(KEY_ID);
        PlayerPrefs.DeleteKey(KEY_TOKEN);
        PlayerPrefs.DeleteKey(KEY_REFRESH);
    }
}

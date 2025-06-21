using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public static class FirebaseTokenRefresher
{
    private const string RefreshUrl = "https://securetoken.googleapis.com/v1/token?key=AIzaSyDsrPpzKKUQyAc2p_sKxcaX2Z0r4YiBOAg";

    public static IEnumerator RefreshToken(string refreshToken, Action<FirebaseUserData> onSuccess, Action<string> onError)
    {
        string payload = $"grant_type=refresh_token&refresh_token={refreshToken}";
        byte[] body = Encoding.UTF8.GetBytes(payload);

        UnityWebRequest request = UnityWebRequest.PostWwwForm(RefreshUrl, "");
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var refreshed = JsonUtility.FromJson<FirebaseRefreshResponse>(request.downloadHandler.text);
            var user = new FirebaseUserData
            {
                localId = refreshed.user_id,
                idToken = refreshed.id_token,
                refreshToken = refreshed.refresh_token
            };
            onSuccess?.Invoke(user);
        }
        else
        {
            onError?.Invoke(request.downloadHandler.text);
        }
    }

    [Serializable]
    private class FirebaseRefreshResponse
    {
        public string id_token;
        public string refresh_token;
        public string user_id;
    }
}

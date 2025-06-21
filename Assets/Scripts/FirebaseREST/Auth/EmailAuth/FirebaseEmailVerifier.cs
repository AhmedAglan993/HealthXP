using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public static class FirebaseEmailVerifier
{
    private const string ApiKey = "AIzaSyDsrPpzKKUQyAc2p_sKxcaX2Z0r4YiBOAg";

    public static IEnumerator SendVerificationEmail(string idToken, Action onSuccess, Action<string> onError)
    {
        string url = $"https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key={ApiKey}";
        string json = JsonUtility.ToJson(new Payload { requestType = "VERIFY_EMAIL", idToken = idToken });

        UnityWebRequest req = WebRequestUtils.CreateJsonPostRequest(url, json);
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
            onSuccess?.Invoke();
        else
            onError?.Invoke(req.downloadHandler.text);
    }

    public static IEnumerator CheckEmailVerified(string idToken, Action<bool> onResult, Action<string> onError)
    {
        string url = $"https://identitytoolkit.googleapis.com/v1/accounts:lookup?key={ApiKey}";
        string json = JsonUtility.ToJson(new TokenPayload { idToken = idToken });

        UnityWebRequest req = WebRequestUtils.CreateJsonPostRequest(url, json);
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            if (req.downloadHandler.text.Contains("\"emailVerified\":true"))
                onResult?.Invoke(true);
            else
                onResult?.Invoke(false);
        }
        else
            onError?.Invoke(req.downloadHandler.text);
    }

    [Serializable] private class Payload { public string requestType; public string idToken; }
    [Serializable] private class TokenPayload { public string idToken; }
}

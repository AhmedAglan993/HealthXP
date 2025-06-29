using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public static class FirebaseCustomAuth
{
    private const string ApiKey = "AIzaSyDsrPpzKKUQyAc2p_sKxcaX2Z0r4YiBOAg";

    public static void ExchangeGoogleToken(string idToken, Action<FirebaseUserData> onSuccess, Action<string> onError)
    {
        AuthLoginManager.Instance.StartCoroutine(ExchangeRoutine(idToken, onSuccess, onError));
    }

    private static IEnumerator ExchangeRoutine(string idToken, Action<FirebaseUserData> onSuccess, Action<string> onError)
    {
        string url = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithIdp?key={ApiKey}";

        string json = JsonUtility.ToJson(new
        {
            postBody = $"id_token={idToken}&providerId=google.com",
            requestUri = "http://localhost",
            returnSecureToken = true
        });

        UnityWebRequest req = WebRequestUtils.CreateJsonPostRequest(url, json);
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            FirebaseUserData user = JsonUtility.FromJson<FirebaseUserData>(req.downloadHandler.text);
            onSuccess?.Invoke(user);
        }
        else
        {
            onError?.Invoke(req.downloadHandler.text);
        }
    }
}

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public static class FirebasePasswordReset
{
    private const string ApiKey = "YOUR_FIREBASE_API_KEY";

    public static IEnumerator SendResetEmail(string email, Action onSuccess, Action<string> onError)
    {
        string url = $"https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key={ApiKey}";
        string json = JsonUtility.ToJson(new Payload { requestType = "PASSWORD_RESET", email = email });

        UnityWebRequest req = WebRequestUtils.CreateJsonPostRequest(url, json);
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
            onSuccess?.Invoke();
        else
            onError?.Invoke(req.downloadHandler.text);
    }

    [Serializable]
    private class Payload
    {
        public string requestType;
        public string email;
    }
}

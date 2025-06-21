using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AnonymousAuthProvider : IAuthProvider
{
    private const string ApiKey = "AIzaSyDsrPpzKKUQyAc2p_sKxcaX2Z0r4YiBOAg";
    private const string Url = "https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=" + ApiKey;

    public IEnumerator SignIn(System.Action<FirebaseUserData> onSuccess, System.Action<string> onError)
    {
        var json = "{\"returnSecureToken\":true}";
        var request = WebRequestUtils.CreateJsonPostRequest(Url, json);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
            onError?.Invoke(request.error);
        else
        {
            var result = JsonUtility.FromJson<FirebaseUserData>(request.downloadHandler.text);
            onSuccess?.Invoke(result);
        }
    }
    
}

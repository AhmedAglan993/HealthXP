using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public static class FirestoreRESTManager
{
    private const string ProjectId = "lifestyleloop";

    public static IEnumerator GetDocument(string collection, string docId, string idToken, System.Action<string> onSuccess, System.Action<string> onError)
    {
        string url = $"https://firestore.googleapis.com/v1/projects/{ProjectId}/databases/(default)/documents/{collection}/{docId}?access_token={idToken}";

        UnityWebRequest req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
            onSuccess?.Invoke(req.downloadHandler.text);
        else
            onError?.Invoke(req.error);
    }

    public static IEnumerator SaveDocument(string docPath, string jsonBody, string idToken, Action onSuccess, Action<string> onError)
    {
        string url = $"https://firestore.googleapis.com/v1/projects/{ProjectId}/databases/(default)/documents/{docPath}";

        UnityWebRequest request = new UnityWebRequest(url, "PATCH");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Authorization", $"Bearer {idToken}");
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            onSuccess?.Invoke();
        }
        else
        {
            onError?.Invoke($"{request.responseCode} - {request.downloadHandler.text}");
        }
    }

}

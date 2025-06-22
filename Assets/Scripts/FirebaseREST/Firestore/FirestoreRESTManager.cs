using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public static class FirestoreRESTManager
{
    private const string BaseUrl = "https://firestore.googleapis.com/v1/";
    private const string ProjectId = "lifestyleloop"; // Replace with your Firebase Project ID

    public static IEnumerator GetDocument(
        string documentPath,
        string idToken,
        Action<string> onSuccess,
        Action<string> onError)
    {
        string url = $"{BaseUrl}projects/{ProjectId}/databases/(default)/documents/{documentPath}";

        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Authorization", $"Bearer {idToken}");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("❌ Firestore get failed: " + request.downloadHandler.text);
            onError?.Invoke(request.downloadHandler.text);
        }
        else
        {
            string resultJson = request.downloadHandler.text;
            Debug.Log("📄 Firestore document fetched successfully.");
            onSuccess?.Invoke(resultJson);
        }
    }

    public static IEnumerator SaveDocument(
        string documentPath,
        string jsonBody,
        string idToken,
        Action onSuccess,
        Action<string> onError)
    {
        string projectId = "lifestyleloop"; // Replace with your actual Firebase project ID
        string url = $"{BaseUrl}projects/{projectId}/databases/(default)/documents/{documentPath}?currentDocument.exists=false";

        var request = new UnityWebRequest(url, "PATCH");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Authorization", $"Bearer {idToken}");
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("❌ Firestore save failed: " + request.downloadHandler.text);
            onError?.Invoke(request.downloadHandler.text);
        }
        else
        {
            Debug.Log("✅ Firestore save successful");
            onSuccess?.Invoke();
        }
    }

}

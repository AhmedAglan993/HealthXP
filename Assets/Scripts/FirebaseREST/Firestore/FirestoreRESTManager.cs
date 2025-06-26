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
    string documentPath,      // e.g., "users/userId/plan"
    string idToken,
    Action<string> onSuccess,
    Action<string> onError)
    {
        // Firebase Realtime Database base URL (adjust to your actual DB URL)
        string baseUrl = "https://lifestyleloop-default-rtdb.firebaseio.com/"; // Ensure trailing slash

        // Build the full URL
        string url = $"{baseUrl}{documentPath}.json?auth={idToken}";

        UnityWebRequest request = UnityWebRequest.Get(url);
        LoadingManager.Service.Show();
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("❌ Realtime DB get failed: " + request.downloadHandler.text);
            onError?.Invoke(request.downloadHandler.text);
        }
        else
        {
            string resultJson = request.downloadHandler.text;
            Debug.Log("📄 Realtime DB document fetched successfully.");
            onSuccess?.Invoke(resultJson);
            LoadingManager.Service.Hide();
        }
    }


    public static IEnumerator SaveDocument(
     string documentPath,        // e.g., "users/user123/plans/plan_456"
     string jsonBody,            // should be valid raw JSON string (already formatted)
     string idToken,             // Firebase user ID token
     Action onSuccess,
     Action<string> onError)
    {
        string baseUrl = "https://lifestyleloop-default-rtdb.firebaseio.com/";

        // Realtime Database requires ".json" at the end
        string url = $"{baseUrl}{documentPath}.json?auth={idToken}";

        var request = new UnityWebRequest(url, "PUT"); // PUT = Replace/Save
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        LoadingManager.Service.Show();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("❌ Realtime DB save failed: " + request.downloadHandler.text);
            onError?.Invoke(request.downloadHandler.text);
        }
        else
        {
            Debug.Log("✅ Realtime DB save successful");
            onSuccess?.Invoke();
            LoadingManager.Service.Hide();

        }
    }


}

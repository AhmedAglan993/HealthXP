using UnityEngine;
using System;
using System.Collections;

public class FirestoreUserProfileService : IUserProfileService
{
    public IEnumerator SaveUserProfile(string uid, string token, string role, Action onSuccess, Action<string> onError)
    {
        string docPath = $"users/{uid}";
        string json = $"{{\"fields\":{{\"role\":{{\"stringValue\":\"{role}\"}}}}}}";

        yield return FirestoreRESTManager.SaveDocument(
            docPath,
            json,
            token,
            onSuccess,
            onError
        );
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirestoreManager : MonoBehaviour
{
    private void Start()
    {
        AuthLoginManager.Instance.OnLoginSuccess += HandleUserSignedIn;
    }

    private void OnDisable()
    {
        AuthLoginManager.Instance.OnLoginSuccess -= HandleUserSignedIn;
    }

    private void HandleUserSignedIn(FirebaseUserData user)
    {
        Debug.Log("🔥 Auth ready, saving progress...");
        
        StartCoroutine(SaveTestProgress(user));
    }

    private IEnumerator SaveTestProgress(FirebaseUserData user)
    {
        string docPath = FirestorePaths.UserProgress(user.localId, DateTime.Now);
        Debug.Log(user.localId);
        print(docPath);
        string json = FirestoreJsonBuilder.BuildMealProgress(new Dictionary<string, bool>
        {
            { "breakfast", true },
            { "lunch", false },
            { "dinner", true }
        });

        yield return FirestoreRESTManager.SaveDocument(
            docPath,
            json,
            user.idToken,
            () => Debug.Log("✅ Firestore progress saved to: " + docPath),
            error => Debug.LogError("❌ Firestore save failed: " + error)
        );
    }
}

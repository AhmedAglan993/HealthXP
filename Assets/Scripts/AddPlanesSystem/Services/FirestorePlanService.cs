using System;
using System.Collections;
using UnityEngine;

public class FirestorePlanService : IPlanService
{
    public IEnumerator SavePlan(MealPlan plan, string token, Action onSuccess, Action<string> onError)
    {
        string docPath = $"users/{plan.userId}/plans/{plan.planId}";
        string json = JsonUtility.ToJson(plan);

        yield return FirestoreRESTManager.SaveDocument(
            docPath,
            json,
            token,
            onSuccess,
            onError
        );
    }
}

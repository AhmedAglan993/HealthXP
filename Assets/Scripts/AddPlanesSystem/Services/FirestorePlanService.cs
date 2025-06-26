using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Networking;

public class FirestorePlanService : MonoBehaviour
{
    public IEnumerator SavePlan(string uid, string token, MealPlan meal, System.Action onSuccess, System.Action<string> onError)
    {
        string docPath = $"users/{uid}/plans/{meal.planId}";
        for (int i = 0; i < meal.days.Count; i++)
        {
            print(meal.days[i].meals.Count);
        }
        string json = BuildPlanJson(meal.days);
        print(json);
        yield return FirestoreRESTManager.SaveDocument(docPath, json, token, onSuccess, onError);
    }
    public IEnumerator GetAllPlans(string uid, string token, Action<List<MealPlan>> onSuccess, Action<string> onError)
    {
        string path = $"users/{uid}/plans.json?auth={token}";
        string url = $"https://lifestyleloop-default-rtdb.firebaseio.com/{path}";

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("❌ Failed to fetch plans: " + request.downloadHandler.text);
            onError?.Invoke(request.downloadHandler.text);
            yield break;
        }

        var rawJson = request.downloadHandler.text;
        Debug.Log("📦 Plans fetched: " + rawJson);

        var parsedPlans = new List<MealPlan>();

        try
        {
            var dict = JsonUtility.FromJson<MealPlanWrapper>($"{{\"plans\":{rawJson}}}");
            foreach (var planEntry in dict.plans)
            {
                parsedPlans.Add(planEntry.Value);
            }

            onSuccess?.Invoke(parsedPlans);
        }
        catch (Exception e)
        {
            Debug.LogError("❌ Parsing error: " + e.Message);
            onError?.Invoke("Failed to parse plans");
        }
    }

    [Serializable]
    public class MealPlanWrapper
    {
        public Dictionary<string, MealPlan> plans;
    }

    private string BuildPlanJson(List<PlanDay> plan)
    {
        // You can expand this to serialize the plan better
        return JsonUtility.ToJson(new Wrapper { plan = plan });
    }

    [System.Serializable]
    private class Wrapper
    {
        public List<PlanDay> plan;
    }
}

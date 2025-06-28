using Newtonsoft.Json;
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
        string json = JsonConvert.SerializeObject(meal);
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
            var planDict = JsonConvert.DeserializeObject<Dictionary<string, MealPlan>>(rawJson); onSuccess?.Invoke(parsedPlans);
            foreach (var pair in planDict)
            {
                pair.Value.planId = pair.Key; // set planId manually
                parsedPlans.Add(pair.Value);
            }
            onSuccess.Invoke(parsedPlans);
            print(parsedPlans.Count);
        }
        catch (Exception e)
        {
            Debug.LogError("❌ Parsing error: " + e.Message);
            onError?.Invoke("Failed to parse plans");
        }
    }


}

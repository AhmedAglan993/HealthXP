using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Networking;

public class FirestorePlanService : MonoBehaviour
{
    public IEnumerator UpdateMealInPlan(
      string planId,
      string dayLabel,
      MealEntry updatedMeal,
      string idToken,
      Action onSuccess,
      Action<string> onError)
    {
        string userId = AuthLoginManager.Instance.CurrentUser.localId;
        string getUrl = $"https://lifestyleloop-default-rtdb.firebaseio.com/users/{userId}/plans.json?auth={idToken}";

        UnityWebRequest getRequest = UnityWebRequest.Get(getUrl);
        yield return getRequest.SendWebRequest();

        if (getRequest.result != UnityWebRequest.Result.Success)
        {
            onError?.Invoke("❌ Failed to fetch plan: " + getRequest.downloadHandler.text);
            yield break;
        }

        var json = getRequest.downloadHandler.text;
        var dict = JsonConvert.DeserializeObject<Dictionary<string, MealPlan>>(json);

        if (dict == null || !dict.TryGetValue(planId, out MealPlan plan))
        {
            onError?.Invoke("❌ Plan not found.");
            yield break;
        }
        // Modify the meal
        bool mealFound = false;
        foreach (var day in plan.days)
        {
        print(dayLabel);

            if (day.label == dayLabel)
            {
                int index = day.meals.FindIndex(m => m.mealid == updatedMeal.mealid);
                if (index != -1)
                {
                    day.meals[index] = updatedMeal;
                    mealFound = true;
                    break;
                }
            }
        }

        if (!mealFound)
        {
            onError?.Invoke("❌ Meal not found in specified day.");
            yield break;
        }

        // Save to only this plan path
        string putUrl = $"https://lifestyleloop-default-rtdb.firebaseio.com/users/{userId}/plans/{planId}.json?auth={idToken}";
        string updatedJson = JsonConvert.SerializeObject(plan);

        var putRequest = new UnityWebRequest(putUrl, "PUT");
        putRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(updatedJson));
        putRequest.downloadHandler = new DownloadHandlerBuffer();
        putRequest.SetRequestHeader("Content-Type", "application/json");

        yield return putRequest.SendWebRequest();

        if (putRequest.result == UnityWebRequest.Result.Success)
        {
            onSuccess?.Invoke();
        }
        else
        {
            onError?.Invoke("❌ Failed to update meal: " + putRequest.downloadHandler.text);
        }
    }

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

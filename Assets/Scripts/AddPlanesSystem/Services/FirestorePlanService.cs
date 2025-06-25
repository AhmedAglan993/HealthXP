using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirestorePlanService : MonoBehaviour
{
    public IEnumerator SavePlan(string uid, string token, List<PlanDay> plan, System.Action onSuccess, System.Action<string> onError)
    {
        string docPath = $"users/{uid}/plans/current";
        string json = BuildPlanJson(plan);
        yield return FirestoreRESTManager.SaveDocument(docPath, json, token, onSuccess, onError);
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

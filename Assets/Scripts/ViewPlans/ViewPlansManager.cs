using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ViewPlansManager : MonoBehaviour
{
    public Transform planContainer;
    public GameObject planCardPrefab;

    private void OnEnable()
    {
        StartCoroutine(LoadPlans());
    }

    private IEnumerator LoadPlans()
    {
        string uid = AuthLoginManager.Instance.CurrentUser.localId;
        string token = AuthLoginManager.Instance.CurrentUser.idToken;

        var service = new FirestorePlanService();

        yield return service.GetAllPlans(uid, token,
            plans =>
            {
                foreach (Transform child in planContainer) Destroy(child.gameObject);

                foreach (var plan in plans)
                {
                    var card = Instantiate(planCardPrefab, planContainer);
                    card.GetComponent<PlanCardUI>().Setup(plan);
                }
            },
            error =>
            {
                Debug.LogError("❌ Error fetching plans: " + error);
            });
    }
}

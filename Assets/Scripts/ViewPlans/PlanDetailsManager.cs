using Ricimi;
using System.Numerics;
using UnityEngine;

public class PlanDetailsManager : MonoBehaviour
{
    public Transform dayContainer;
    public GameObject dayCardPrefab;
    public CleanButton editButton;
    public CleanButton saveButton;

    private MealPlan currentPlan;
    private bool isEditable = false;

    public void Setup(MealPlan plan)
    {
        currentPlan = plan;
        RenderPlan();

        editButton.onClick.AddListener(() =>
        {
            isEditable = true;
            saveButton.gameObject.SetActive(true);
            RenderPlan();
        });

        saveButton.onClick.AddListener(SaveEditedPlan);
    }

    private void RenderPlan()
    {
        foreach (Transform child in dayContainer)
            Destroy(child.gameObject);

        foreach (var day in currentPlan.days)
        {
            var dayCard = Instantiate(dayCardPrefab, dayContainer);
           // dayCard.GetComponent<DayCardUI>().Setup(day, isEditable);
        }
    }

    private void SaveEditedPlan()
    {
        var token = FirebaseAuthManager.Instance.CurrentUser.idToken;
        var uid = FirebaseAuthManager.Instance.CurrentUser.localId;

        string planId = currentPlan.planId;

        var service = new FirestorePlanService();
        StartCoroutine(service.SavePlan(
            uid,                       // ✅ userId
            token,                       // ✅ token
            currentPlan,                        // ✅ the actual plan
            () => Debug.Log("✅ Plan saved!"),  // ✅ onSuccess
            err => Debug.LogError("❌ Failed to save: " + err) // ✅ onError
        ));

        isEditable = false;
        saveButton.gameObject.SetActive(false);
        RenderPlan();
    }
}

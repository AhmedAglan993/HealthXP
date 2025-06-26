using Ricimi;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlanPreviewManager : MonoBehaviour
{
    public CleanButton confirmButton;
    public AddPlanManager AddPlanManager;
    private void Awake()
    {
        confirmButton.onClick.AddListener(() => ConfirmPlan());
    }
    private void OnEnable()
    {
        AddPlanManager.OnDayCardsChanged += UpdateButton;
        UpdateButton();
    }

    private void OnDisable()
    {
        AddPlanManager.OnDayCardsChanged -= UpdateButton;
    }

    private void UpdateButton()
    {
        confirmButton.gameObject.SetActive(AddPlanManager.dayCards.Count > 0);
    }
    public void ConfirmPlan()
    {
        var user = AuthLoginManager.Instance.CurrentUser;
        var service = new FirestorePlanService();
        AddPlanManager.ResetPlans();
        MealPlan plan = new MealPlan
        {
            userId = user.localId,
            title = "My Plan",
            days = new List<PlanDay>()
        };

        foreach (var dayCard in AddPlanManager.dayCards)
        {
            plan.days.Add(dayCard.ToDayPlan());
        }



        StartCoroutine(service.SavePlan(
            user.localId,                       // ✅ userId
            user.idToken,                       // ✅ token
            plan,                        // ✅ the actual plan
            () => Debug.Log("✅ Plan saved!"),  // ✅ onSuccess
            err => Debug.LogError("❌ Failed to save: " + err) // ✅ onError
        ));
    }

}

using System.Collections.Generic;
using UnityEngine;

public class AddPlanManager : MonoBehaviour
{
    public Transform dayCardsParent;
    public GameObject dayCardPrefab;

    private List<DayCardUI> dayCards = new();

    public void AddDay()
    {
        var go = Instantiate(dayCardPrefab, dayCardsParent);
        var card = go.GetComponent<DayCardUI>();
        card.SetupDay();
        dayCards.Add(card);
    }

    public void PreviewPlan()
    {
        List<PlanDay> days = new();
        foreach (var day in dayCards)
        {
            days.Add(day.GetDayPlan());
        }
        PlanPreviewManager.Instance.ShowPreview(days);
    }
}

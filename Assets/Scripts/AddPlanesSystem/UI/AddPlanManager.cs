using System;
using System.Collections.Generic;
using UnityEngine;

public class AddPlanManager : MonoBehaviour
{
    public Transform dayCardsParent;
    public GameObject dayCardPrefab;
    public static event Action OnDayCardsChanged;
    public List<DayCardUI> dayCards = new();

    public void AddDay()
    {
        var go = Instantiate(dayCardPrefab, dayCardsParent);
        var card = go.GetComponent<DayCardUI>();
        dayCards.Add(card);
        card.title.text = "Day " + dayCards.Count;
        OnDayCardsChanged.Invoke();
    }
    public void ResetPlans()
    {
        foreach (Transform child in dayCardsParent.transform)
            Destroy(child.gameObject);

    }
}

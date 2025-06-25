using System.Collections.Generic;
using UnityEngine;

public class DayCardUI : MonoBehaviour
{
    public Transform mealCardContainer;
    public GameObject mealCardPrefab;

    private List<MealCardUI> mealCards = new();

    public void SetupDay() => AddMeal();

    public void AddMeal()
    {
        var go = Instantiate(mealCardPrefab, mealCardContainer);
        var card = go.GetComponent<MealCardUI>();
        card.SetupMeal();
        mealCards.Add(card);
    }

    public PlanDay GetDayPlan()
    {
        var plan = new PlanDay { meals = new List<MealEntry>() };
        foreach (var card in mealCards)
        {
            plan.meals.Add(card.GetMeal());
        }
        return plan;
    }
}

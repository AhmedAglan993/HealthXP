using System.Collections.Generic;
using UnityEngine;

public static class PlanBuilder
{
    public static MealPlan BuildMealPlanFromUI(AddPlanScreenUI ui)
    {
        MealPlan plan = new MealPlan
        {
            userId = FirebaseAuthManager.Instance.CurrentUser.localId,
            planId = System.Guid.NewGuid().ToString(),
            title = ui.planTitleInput.text,
            days = new List<PlanDay>()
        };

        foreach (Transform dayObj in ui.daysContainer)
        {
            var dayUI = dayObj.GetComponent<DayCardUI>();
            var day = new PlanDay
            {
                label = dayUI.dayLabel.text,
                meals = new List<MealEntry>(dayUI.GetMeals())
            };
            plan.days.Add(day);
        }

        return plan;
    }
}

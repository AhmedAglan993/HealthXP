using System;
using System.Collections.Generic;
using UnityEngine.XR;

[Serializable]
public class MealPlan
{
    public string planId;
    public string userId;
    public string title;
    public List<PlanDay> days = new();
    public MealPlan()
    {
        planId = System.Guid.NewGuid().ToString(); // Generates when created
    }
}

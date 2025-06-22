using System;
using System.Collections.Generic;

[Serializable]
public class MealPlan
{
    public string planId;
    public string userId;
    public string title;
    public DateTime? startDate;
    public DateTime? endDate;
    public List<PlanDay> days = new();
}

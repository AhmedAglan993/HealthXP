using System;
using System.Collections.Generic;

[Serializable]
public class PlanDay
{
    public string label; // e.g., "Day 1", "Monday"
    public List<MealEntry> meals = new();
}

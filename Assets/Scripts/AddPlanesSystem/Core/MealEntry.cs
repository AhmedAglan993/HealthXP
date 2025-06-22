using System;

[Serializable]
public class MealEntry
{
    public string mealType; // Breakfast, Lunch, etc.
    public string mealName;
    public string recipe;
    public string macros; // could be calories, proteins, etc.
}

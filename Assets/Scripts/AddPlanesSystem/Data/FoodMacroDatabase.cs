using System.Collections.Generic;
using UnityEngine;

public class FoodMacroDatabase : MonoBehaviour
{
    public static FoodMacroDatabase Instance;

    [SerializeField] private List<MealEntry> foodItems;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public MealEntry FindByName(string name)
    {
        return foodItems.Find(item => item.mealName.ToLower() == name.ToLower());
    }
}

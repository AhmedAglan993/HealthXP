using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MealDataService : MonoBehaviour
{
    public static MealDataService Instance { get; private set; }
    public MealDatabase database;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        LoadLocalDatabase();
    }

    private void LoadLocalDatabase()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("food_database");
        database = JsonUtility.FromJson<MealDatabase>(jsonFile.text);
    }

    public List<MealEntry> GetMeals(string search = "", string category = "All")
    {
        List<MealEntry> results = new();

        foreach (var meal in database.meals)
        {
            if (!string.IsNullOrEmpty(search) && !meal.mealName.ToLower().Contains(search.ToLower()))
                continue;

            if (category != "All" && meal.category != category)
                continue;

            results.Add(meal);
        }

        return results;
    }

    public List<string> GetUniqueCategories()
    {
        HashSet<string> categories = new();
        foreach (var meal in database.meals)
            categories.Add(meal.category);
        return new List<string>(categories);
    }
}

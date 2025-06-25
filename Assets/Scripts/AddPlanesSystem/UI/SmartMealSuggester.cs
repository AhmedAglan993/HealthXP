using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SmartMealSuggester : MonoBehaviour
{
    [SerializeField] private TextAsset localFoodDatabaseJSON;
    private List<MealEntry> allSuggestions;

    public event Action<List<MealEntry>> OnSuggestionsUpdated;

    private void Awake()
    {
        LoadDatabase();
    }

    private void LoadDatabase()
    {
        var wrapper = JsonUtility.FromJson<MealSuggestionListWrapper>(localFoodDatabaseJSON.text);
        allSuggestions = wrapper?.meals ?? new List<MealEntry>();
    }

    public void FilterSuggestions(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            OnSuggestionsUpdated?.Invoke(new List<MealEntry>());
            return;
        }

        var filtered = allSuggestions
            .Where(s => s.mealName.IndexOf(input, StringComparison.OrdinalIgnoreCase) >= 0)
            .Take(10)
            .ToList();

        OnSuggestionsUpdated?.Invoke(filtered);
    }

    [Serializable]
    public class MealSuggestionListWrapper
    {
        public List<MealEntry> meals;
    }
}

using TMPro;
using UnityEngine;

public class MealCardUI : MonoBehaviour
{
    [Header("Meal Input Fields")]
    public TMP_Dropdown mealTypeDropdown;
    public TMP_InputField mealNameInput;
    public TMP_InputField macrosInput;
    public TMP_InputField recipeInput;

    // Setup the UI with an existing MealEntry (for editing or template preview)
    public void SetupMeal(MealEntry meal)
    {
        // Set meal type dropdown index based on value
        int typeIndex = mealTypeDropdown.options.FindIndex(option => option.text == meal.mealType);
        mealTypeDropdown.value = typeIndex >= 0 ? typeIndex : 0;

        // Fill other input fields
        mealNameInput.text = meal.mealName;
        macrosInput.text = meal.macros;
        recipeInput.text = meal.recipe;
    }

    // Extract meal from UI
    public MealEntry GetMeal()
    {
        return new MealEntry
        {
            mealType = mealTypeDropdown.options[mealTypeDropdown.value].text,
            mealName = mealNameInput.text.Trim(),
            macros = macrosInput.text.Trim(),
            recipe = recipeInput.text.Trim()
        };
    }

    // Optional: validate inputs
    public bool IsValidMeal(out string error)
    {
        error = "";
        if (string.IsNullOrWhiteSpace(mealNameInput.text))
            error = "Meal name is required.";
        else if (string.IsNullOrWhiteSpace(macrosInput.text))
            error = "Macros are required.";
        else if (string.IsNullOrWhiteSpace(recipeInput.text))
            error = "Recipe is required.";

        return string.IsNullOrEmpty(error);
    }
}

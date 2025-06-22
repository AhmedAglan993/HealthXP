using TMPro;
using UnityEngine;

public class MealCardUI : MonoBehaviour
{
    public TMP_Dropdown mealTypeDropdown;
    public TMP_InputField mealNameInput;
    public TMP_InputField macrosInput;
    public TMP_InputField recipeInput;

    public void SetupMeal() { /* Optional pre-fill logic */ }

    public MealEntry GetMeal()
    {
        return new MealEntry
        {
            mealType = mealTypeDropdown.options[mealTypeDropdown.value].text,
            mealName = mealNameInput.text,
            macros = macrosInput.text,
            recipe = recipeInput.text
        };
    }
}

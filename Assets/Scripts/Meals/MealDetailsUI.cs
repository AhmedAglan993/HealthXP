using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MealDetailsUI : MonoBehaviour
{
    public TMP_Text mealNameText;
    public TMP_Text macrosText;
    public TMP_Text recipeText;
    public Button addToPlanButton;

    public static MealDetailsUI Instance;

    private MealEntry currentMeal;

    private void Awake()
    {
        Instance = this;
        addToPlanButton.onClick.AddListener(AddToPlan);
    }

    public void Setup(MealEntry meal)
    {
        currentMeal = meal;
        mealNameText.text = meal.category + " " + meal.mealName;
        macrosText.text = meal.macros;
        recipeText.text = meal.recipe;
    }

    void AddToPlan()
    {
        // Optionally open Add Plan UI and pass `currentMeal`
        Debug.Log("📦 Add to plan: " + currentMeal.mealName);
    }
}

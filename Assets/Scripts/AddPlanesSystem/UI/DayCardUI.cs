using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayCardUI : MonoBehaviour
{
    public TMP_Text dayLabel;
    public Button addMealButton;
    public Transform mealContainer;
    public MealCardUI mealCardPrefab;

    public void SetupDay(string label)
    {
        dayLabel.text = label;
        addMealButton.onClick.AddListener(() =>
        {
            var meal = Instantiate(mealCardPrefab, mealContainer);
            meal.SetupMeal();
        });
    }

    public MealEntry[] GetMeals()
    {
        var entries = new MealEntry[mealContainer.childCount];
        for (int i = 0; i < mealContainer.childCount; i++)
        {
            entries[i] = mealContainer.GetChild(i).GetComponent<MealCardUI>().GetMeal();
        }
        return entries;
    }
}

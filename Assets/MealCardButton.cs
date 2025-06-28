using TMPro;
using UnityEngine;

public class MealCardButton : MonoBehaviour
{
    public TextMeshProUGUI mealText;
    

    public MealEntry mealEntry; // 🟡 Store the actual meal data

    public void UpdateMeal(MealEntry meal)
    {
        this.mealEntry = meal;
        mealText.text = meal.mealName;
    }
}

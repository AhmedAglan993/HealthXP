using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MealCardButton : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI mealText;
    // Start is called before the first frame update
    public void UpdateMeal(MealEntry newMeal)
    {
        mealText.text = $"{newMeal.mealType}: {newMeal.mealName} ({newMeal.macros})";

    }
}

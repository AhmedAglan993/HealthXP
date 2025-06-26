using TMPro;
using UnityEngine;
using Ricimi;

public class MealNutshellUI : MonoBehaviour
{
    public TMP_Text nameText, macrosText;
    public CleanButton editButton;
    private MealEntry meal;

    public void Setup(MealEntry entry, bool showEdit)
    {
        meal = entry;
        nameText.text = entry.mealName;
        macrosText.text = entry.macros;

        editButton.gameObject.SetActive(showEdit);
        editButton.onClick.RemoveAllListeners();
        editButton.onClick.AddListener(() => OpenMealPopup());
    }

    private void OpenMealPopup()
    {
        ScreenNavigator.Instance.ShowPopup(popupId.AddMealPopUp);
    }

    private void OnMealEdited(MealEntry updated)
    {
        // Update the local meal
        meal.mealName = updated.mealName;
        meal.macros = updated.macros;
        meal.recipe = updated.recipe;

        nameText.text = updated.mealName;
        macrosText.text = updated.macros;
    }
}

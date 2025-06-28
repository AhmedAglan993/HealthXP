using Ricimi;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class DayCardUI : MonoBehaviour
{
    public Transform mealButtonParent;
    public MealCardButton mealButtonPrefab;
    public CleanButton addMealButton;
    public TextMeshProUGUI title;

    private List<MealEntry> dayMeals = new();

    public void AddMeal(MealEntry meal)
    {
        dayMeals.Add(meal);
    }
    private void Start()
    {
        addMealButton?.onClick.AddListener(OpenAddMealPopup);

    }
    public PlanDay ToDayPlan()
    {
        return new PlanDay
        {
            label = title.text,
            meals = new List<MealEntry>(dayMeals)
        };
    }
    MealCardButton currentMealCardButton;
    private void CreateMealButton(MealEntry meal)
    {
        currentMealCardButton = Instantiate(mealButtonPrefab, mealButtonParent);
        currentMealCardButton.GetComponent<CleanButton>().onClick.AddListener(() =>
        {
            MealCardUI.Instance.SetupMeal(currentMealCardButton, this, meal);
            ScreenNavigator.Instance.ShowPopup(popupId.AddMealPopUp);
        });
    }
    // From DayCardUI.cs
    public void OpenAddMealPopup()
    {
        ScreenNavigator.Instance.ShowPopup(popupId.AddMealPopUp);
        CreateMealButton(new MealEntry());
        MealCardUI.Instance.SetupMeal(currentMealCardButton, this);
    }
}

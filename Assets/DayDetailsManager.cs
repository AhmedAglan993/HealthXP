using Ricimi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayDetailsManager : MonoBehaviour
{
    public Transform mealsContainer;
    public MealCardButton mealCardPrefab;
    public static DayDetailsManager Instance;
    MealCardButton currentMealCardButton;
    public List<MealEntry> allDayMealsData;
    string currentPlanID;
    public string dayLable;
    private void Awake()
    {
        Instance = this;
    }
    public void Setup(string planID)
    {
        currentPlanID = planID;
        RenderMeals();
    }
    public void RenderMeals()
    {
        foreach (Transform child in mealsContainer)
            Destroy(child.gameObject);

        for (int i = 0; i < allDayMealsData.Count; i++)
        {
            MealEntry mealData = allDayMealsData[i];
            currentMealCardButton = Instantiate(mealCardPrefab, mealsContainer);
            int index = i;
            currentMealCardButton.GetComponent<CleanButton>().onClick.AddListener(() => OpenMealDetails(index));
            currentMealCardButton.UpdateMeal(mealData);
        }
    }
    public void OpenMealDetails(int id)
    {
        ScreenNavigator.Instance.ShowPopup(popupId.AddMealPopUp);
        MealCardUI.Instance.SetupMeal(currentMealCardButton, null, allDayMealsData[id], true, currentPlanID,dayLable);
    }

}

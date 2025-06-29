using Ricimi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayDetailsManager : MonoBehaviour
{
    public Transform mealsContainer;
    public MealCardButton mealCardPrefab;
    public static DayDetailsManager Instance;
    List<MealCardButton> MealCardButtons = new List<MealCardButton>();
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
        MealCardButtons.Clear();
        for (int i = 0; i < allDayMealsData.Count; i++)
        {
            MealEntry mealData = allDayMealsData[i];
            MealCardButtons.Add(Instantiate(mealCardPrefab, mealsContainer));
            int index = i;
            MealCardButtons[index].GetComponent<CleanButton>().onClick.AddListener(() => OpenMealDetails(index));
            MealCardButtons[index].UpdateMeal(mealData);
        }
    }
    public void OpenMealDetails(int id)
    {
        ScreenNavigator.Instance.ShowPopup(popupId.AddMealPopUp);
        MealCardUI.Instance.SetupMeal(MealCardButtons[id], null, allDayMealsData[id], false, currentPlanID, dayLable, true);
    }

}

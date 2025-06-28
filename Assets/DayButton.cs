using Ricimi;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayButton : MonoBehaviour
{
    public TextMeshProUGUI title;
    private List<MealEntry> dayMeals = new();
    public string PlanID;
    public void Setup(PlanDay dayData, string planId)
    {
        title.text = dayData.label;
        PlanID = planId;
        dayMeals = new List<MealEntry>(dayData.meals);
        GetComponent<CleanButton>().onClick.AddListener(() => OpenDayDetails());
    }
    void OpenDayDetails()
    {
        ScreenNavigator.Instance.ShowPopup(popupId.DayDetails);
        DayDetailsManager.Instance.allDayMealsData = dayMeals;
        DayDetailsManager.Instance.dayLable = title.text;
        DayDetailsManager.Instance.Setup(PlanID);
    }
}

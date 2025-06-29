using Ricimi;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MealItemUI : MonoBehaviour
{
    public TMP_Text mealNameText;
    public TMP_Text macrosText;
    public Image mealIcon;
    private MealEntry entry;

    public void Setup(MealEntry meal)
    {
        entry = meal;
        mealNameText.text = meal.mealName;
        macrosText.text = meal.macros;
        GetComponent<CleanButton>().onClick.AddListener(OpenDetails);
    }

    void OpenDetails()
    {
        ScreenNavigator.Instance.ShowPopup(popupId.MealDetails);
        MealDetailsUI.Instance.Setup(entry);
    }
}

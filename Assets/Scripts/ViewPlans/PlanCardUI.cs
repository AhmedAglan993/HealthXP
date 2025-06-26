using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlanCardUI : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text dateText;
    public Button openButton;

    private MealPlan currentPlan;

    public void Setup(MealPlan plan)
    {
        currentPlan = plan;
        titleText.text = plan.title;

        openButton.onClick.AddListener(() => OpenPlanDetail());
    }

    private void OpenPlanDetail()
    {
        // Option: pass to a plan viewer screen
        Debug.Log("📋 Open plan: " + currentPlan.title);
        // Store in session, or use navigation with params
        ScreenNavigator.Instance.NavigateTo(ScreenId.AddNewPlan);
    }
}

using Ricimi;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlanCardUI : MonoBehaviour
{
    public TMP_Text titleText;

    private MealPlan currentPlan;

    public void Setup(MealPlan plan)
    {
        currentPlan = plan;
        titleText.text = plan.title;

        GetComponent<CleanButton>().onClick.AddListener(() => OpenPlanDetail());
    }

    private void OpenPlanDetail()
    {
        // Option: pass to a plan viewer screen
        Debug.Log("📋 Open plan: " + currentPlan.title);
        // Store in session, or use navigation with params
        PlanDetailsManager.Instance.Setup(currentPlan);
        ScreenNavigator.Instance.NavigateTo(ScreenId.PlanDetails);
    }
}

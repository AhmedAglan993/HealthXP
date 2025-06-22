using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlanPreviewUI : MonoBehaviour
{
    public TMP_Text titleText;
    public Transform previewListContainer;
    public GameObject dayPreviewPrefab;
    public Button confirmButton;

    private MealPlan currentPlan;

    public void ShowPreview(MealPlan plan)
    {
        currentPlan = plan;
        titleText.text = plan.title;
        // populate previewListContainer...

        gameObject.SetActive(true);
    }

    public void ConfirmPlan()
    {
        var token = FirebaseAuthManager.Instance.CurrentUser.idToken;
        var service = new FirestorePlanService();
        StartCoroutine(service.SavePlan(currentPlan, token,
            () => Debug.Log("✅ Plan saved!"),
            err => Debug.LogError("❌ Failed to save: " + err)
        ));
    }
}

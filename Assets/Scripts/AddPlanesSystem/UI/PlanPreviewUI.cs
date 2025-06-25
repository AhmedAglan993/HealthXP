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
        var user = FirebaseAuthManager.Instance.CurrentUser;
        var service = new FirestorePlanService();

        StartCoroutine(service.SavePlan(
            user.localId,                       // ✅ userId
            user.idToken,                       // ✅ token
            currentPlan.days,                        // ✅ the actual plan
            () => Debug.Log("✅ Plan saved!"),  // ✅ onSuccess
            err => Debug.LogError("❌ Failed to save: " + err) // ✅ onError
        ));
    }

}

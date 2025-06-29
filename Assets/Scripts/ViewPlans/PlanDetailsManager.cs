using Ricimi;
using System.Numerics;
using TMPro;
using UnityEngine;

public class PlanDetailsManager : MonoBehaviour
{
    public Transform dayContainer;
    public GameObject dayCardPrefab;
  //  public CleanButton editButton;
   // public CleanButton saveButton;
    public TextMeshProUGUI planTitle;
    public static PlanDetailsManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    private MealPlan currentPlan;

    public void Setup(MealPlan plan)
    {
        currentPlan = plan;
        RenderPlan();
        planTitle.text = currentPlan.title;
      /*  editButton.onClick.AddListener(() =>
        {
            saveButton.gameObject.SetActive(true);
            RenderPlan();
        });

        saveButton.onClick.AddListener(SaveEditedPlan);*/
    }

    private void RenderPlan()
    {
        foreach (Transform child in dayContainer)
            Destroy(child.gameObject);

        foreach (var day in currentPlan.days)
        {
            var dayCard = Instantiate(dayCardPrefab, dayContainer);
            dayCard.GetComponent<DayButton>().Setup(day, currentPlan.planId);
        }
    }

    private void SaveEditedPlan()
    {
        var token = AuthLoginManager.Instance.CurrentUser.idToken;
        var uid = AuthLoginManager.Instance.CurrentUser.localId;

        string planId = currentPlan.planId;

        var service = new FirestorePlanService();
        StartCoroutine(service.SavePlan(
            uid,                       // ✅ userId
            token,                       // ✅ token
            currentPlan,                        // ✅ the actual plan
            () => Debug.Log("✅ Plan saved!"),  // ✅ onSuccess
            err => Debug.LogError("❌ Failed to save: " + err) // ✅ onError
        ));

       // saveButton.gameObject.SetActive(false);
        RenderPlan();
    }
}

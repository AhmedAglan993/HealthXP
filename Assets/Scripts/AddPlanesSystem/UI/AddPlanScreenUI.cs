using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddPlanScreenUI : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField planTitleInput;
    public TMP_InputField startDateInput;
    public Button addDayButton;
    public Transform daysContainer;
    public DayCardUI dayCardPrefab;
    public Button previewButton;
    public GameObject previewScreen;

    private void Start()
    {
        addDayButton.onClick.AddListener(AddNewDay);
        previewButton.onClick.AddListener(OpenPreview);
    }

    void AddNewDay()
    {
        var newDay = Instantiate(dayCardPrefab, daysContainer);
        newDay.SetupDay("Day " + (daysContainer.childCount));
    }

    void OpenPreview()
    {
        var plan = PlanBuilder.BuildMealPlanFromUI(this);
        // to add open preview popup here
        previewScreen.GetComponent<PlanPreviewUI>().ShowPreview(plan);
    }
}

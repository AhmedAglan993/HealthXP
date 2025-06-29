using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Ricimi;

public class MealCardUI : MonoBehaviour
{
    public TMP_Dropdown mealTypeDropdown;
    public TMP_InputField mealNameInput;
    public TMP_InputField macrosInput;
    public TMP_InputField recipeInput;
    public GameObject suggestionContainer, suggestionParent;
    public GameObject suggestionItemPrefab;
    public SmartMealSuggester suggester;
    public CleanButton saveButton;
    public CleanButton editButton;
    public MealCardButton mealCardButton;
    public DayCardUI currentDayCard;
    public string currentPlanId; // Set this when editing an existing plan
    public string currentDayLabel; // Set this when editing an existing plan
    public bool isEditingExistingPlan = false;

    public static MealCardUI Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        suggester.OnSuggestionsUpdated += RenderSuggestions;
        mealNameInput.onValueChanged.AddListener(suggester.FilterSuggestions);
        mealNameInput.onSelect.AddListener(_ => suggester.FilterSuggestions(mealNameInput.text));
        saveButton.onClick.AddListener(SaveMealToDayMeals);
        editButton.onClick.AddListener(() => EnableEditMode(true));

    }
    void EnableEditMode(bool editable)
    {

        mealNameInput.text = "";
        macrosInput.text = "";
        recipeInput.text = "";
        mealTypeDropdown.value = 0; mealNameInput.interactable = editable;
        macrosInput.interactable = editable;
        recipeInput.interactable = editable;
        mealTypeDropdown.interactable = editable;
        saveButton.gameObject.SetActive(editable);

    }
    public void SetupMeal(MealCardButton mealCard, DayCardUI currentDayCard, MealEntry existing = null,
                          bool editable = true, string planId = null, string dayLabel = null, bool isEditButtonEnabled = false)
    {
        this.currentDayCard = currentDayCard;
        this.mealCardButton = mealCard;
        EnableEditMode(editable);
        this.currentPlanId = planId;
        this.currentDayLabel = dayLabel;
        this.isEditingExistingPlan = !string.IsNullOrEmpty(planId);
        editButton.gameObject.SetActive(isEditButtonEnabled);
        // Set interactability based on mode


        if (existing != null)
        {
            mealNameInput.text = existing.mealName;
            macrosInput.text = existing.macros;
            recipeInput.text = existing.recipe;

            int index = mealTypeDropdown.options.FindIndex(o => o.text == existing.mealType);
            if (index >= 0) mealTypeDropdown.value = index;
        }

    }


    private void RenderSuggestions(List<MealEntry> suggestions)
    {
        foreach (Transform child in suggestionParent.transform)
            Destroy(child.gameObject);

        foreach (var suggestion in suggestions)
        {
            var item = Instantiate(suggestionItemPrefab, suggestionParent.transform);
            item.GetComponentInChildren<TMP_Text>().text = suggestion.mealName;
            item.GetComponent<Button>().onClick.AddListener(() => OnSuggestionClicked(suggestion));
        }

        suggestionContainer.SetActive(suggestions.Count > 0);
    }

    private void OnSuggestionClicked(MealEntry suggestion)
    {
        mealNameInput.text = suggestion.mealName;
        macrosInput.text = suggestion.macros;
        recipeInput.text = suggestion.recipe;
        suggestionContainer.SetActive(false);
    }

    public void SaveMealToDayMeals()
    {
        MealEntry entry = new MealEntry
        {
            mealType = mealTypeDropdown.options[mealTypeDropdown.value].text,
            mealName = mealNameInput.text,
            macros = macrosInput.text,
            recipe = recipeInput.text,
            mealid = string.IsNullOrEmpty(mealCardButton.mealEntry.mealid)
                ? System.Guid.NewGuid().ToString()
                : mealCardButton.mealEntry.mealid
        };
        print(mealCardButton.mealEntry.mealid);
        mealCardButton.UpdateMeal(entry);
        currentDayCard?.AddMeal(entry);

        // ✅ If this is editing an existing saved plan, update it in Firebase
        if (isEditingExistingPlan)
        {
            string token = AuthLoginManager.Instance.CurrentUser.idToken;
            var service = new FirestorePlanService(); // Should be renamed to RealtimePlanService
            Instance.StartCoroutine(service.UpdateMealInPlan(
                currentPlanId,
                currentDayLabel,
                entry,
                token,
                () =>
                {
                    ScreenNavigator.Instance.ClosePopup(popupId.AddMealPopUp);
                    Debug.Log("✅ Meal updated in Firebase: " + entry.mealName);
                },
                err => Debug.LogError("❌ Meal update failed: " + err)
            ));
        }
        else
        {
            ScreenNavigator.Instance.ClosePopup(popupId.AddMealPopUp);
        }
    }

}

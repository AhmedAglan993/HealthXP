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

    public MealCardButton mealCardButton;
    public DayCardUI currentDayCard;
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
    }

    public void SetupMeal(MealCardButton dayCard, DayCardUI currentDayCard, MealEntry existing = null)
    {
        this.currentDayCard = currentDayCard;
        mealCardButton = dayCard;
        if (existing != null)
        {
            // Edit mode
            mealNameInput.text = existing.mealName;
            macrosInput.text = existing.macros;
            recipeInput.text = existing.recipe;

            int index = mealTypeDropdown.options.FindIndex(o => o.text == existing.mealType);
            if (index >= 0) mealTypeDropdown.value = index;
        }
        else
        {
            // New meal
            mealNameInput.text = "";
            macrosInput.text = "";
            recipeInput.text = "";
            mealTypeDropdown.value = 0;
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
            recipe = recipeInput.text
        };
        mealCardButton.UpdateMeal(entry);
        currentDayCard.AddMeal(entry);
        ScreenNavigator.Instance.ClosePopup(popupId.AddMealPopUp);
    }
}

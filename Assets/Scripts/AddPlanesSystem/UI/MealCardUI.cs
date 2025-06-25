using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MealCardUI : MonoBehaviour
{
    public TMP_Dropdown mealTypeDropdown;
    public TMP_InputField mealNameInput;
    public TMP_InputField macrosInput;
    public TMP_InputField recipeInput;

    public GameObject suggestionContainer,suggestionParent;
    public GameObject suggestionItemPrefab;
    public SmartMealSuggester suggester;

    private void Start()
    {
        suggester.OnSuggestionsUpdated += RenderSuggestions;

        mealNameInput.onValueChanged.AddListener(suggester.FilterSuggestions);
        mealNameInput.onSelect.AddListener(_ => suggester.FilterSuggestions(mealNameInput.text));
    }
    public void SetupMeal()
    {

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

    }
    public MealEntry GetMeal()
    {
        return new MealEntry
        {
            mealType = mealTypeDropdown.options[mealTypeDropdown.value].text,
            mealName = mealNameInput.text,
            macros = macrosInput.text,
            recipe = recipeInput.text
        };
    }
}

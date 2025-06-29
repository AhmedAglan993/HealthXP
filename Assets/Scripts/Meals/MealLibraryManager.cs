using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MealLibraryManager : MonoBehaviour
{
    public TMP_InputField searchInput;
    public TMP_Dropdown categoryDropdown;
    public Transform mealListParent;
    public GameObject mealItemPrefab;

    private void Start()
    {
        searchInput.onValueChanged.AddListener(_ => RefreshMeals());
        categoryDropdown.onValueChanged.AddListener(_ => RefreshMeals());

        LoadCategories();
        RefreshMeals();
    }

    void LoadCategories()
    {
        categoryDropdown.ClearOptions();
        var categories = MealDataService.Instance.GetUniqueCategories();
        categories.Insert(0, "All");
        categoryDropdown.AddOptions(categories);
    }

    void RefreshMeals()
    {
        foreach (Transform child in mealListParent)
            Destroy(child.gameObject);

        string search = searchInput.text;
        string category = categoryDropdown.options[categoryDropdown.value].text;

        List<MealEntry> filtered = MealDataService.Instance.GetMeals(search, category);
        foreach (var meal in filtered)
        {
            var item = Instantiate(mealItemPrefab, mealListParent);
            item.GetComponent<MealItemUI>().Setup(meal);
        }
    }
}

using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlanPreviewManager : MonoBehaviour
{
    public static PlanPreviewManager Instance;

    public TMP_Text previewText;
    public GameObject previewPanel;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void ShowPreview(List<PlanDay> days)
    {
        previewPanel.SetActive(true);
        previewText.text = "";

        for (int i = 0; i < days.Count; i++)
        {
            previewText.text += $"Day {i + 1}:\n";
            foreach (var meal in days[i].meals)
            {
                previewText.text += $"- {meal.mealType}: {meal.mealName}\n";
            }
            previewText.text += "\n";
        }
    }
}

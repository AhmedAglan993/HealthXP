using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MealDetailsManager : MonoBehaviour
{
   
    public List<MealEntry> mealsData;
    public static MealDetailsManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void OnEnable()
    {
    }
   
}

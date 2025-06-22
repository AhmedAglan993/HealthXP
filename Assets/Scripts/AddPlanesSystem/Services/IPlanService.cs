using System;
using System.Collections;

public interface IPlanService
{
    IEnumerator SavePlan(MealPlan plan, string token, Action onSuccess, Action<string> onError);
}

using System;
using System.Collections.Generic;
using UnityEngine;

public static class FirestoreJsonBuilder
{
    public static string BuildMealProgress(Dictionary<string, bool> values)
    {
        var wrapper = new FirestoreFieldsWrapper();

        foreach (var pair in values)
        {
            wrapper.fields[pair.Key] = new FirestoreBoolean { booleanValue = pair.Value };
        }

        return JsonUtility.ToJson(wrapper);
    }

    [Serializable]
    public class FirestoreFieldsWrapper
    {
        public Dictionary<string, FirestoreBoolean> fields = new();
    }

    [Serializable]
    public class FirestoreBoolean
    {
        public bool booleanValue;
    }
}

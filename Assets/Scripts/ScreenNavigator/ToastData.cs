using UnityEngine;

[System.Serializable]
public class ToastData
{
    public string message;
    public ToastType type;
    public float duration = 2.5f;

    public enum ToastType
    {
        Success,
        Error,
        Info
    }
}

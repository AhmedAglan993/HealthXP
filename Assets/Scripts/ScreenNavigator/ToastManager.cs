using UnityEngine;

public class ToastManager : MonoBehaviour
{
    public static ToastManager Instance { get; private set; }

    [SerializeField] private UIToastMessage toastPrefab;
    [SerializeField] private Transform toastParent;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowToast(string message, ToastData.ToastType type = ToastData.ToastType.Info, float duration = 2f)
    {
        var toast = Instantiate(toastPrefab, toastParent);
        toast.transform.SetAsLastSibling();

        var data = new ToastData
        {
            message = message,
            type = type,
            duration = duration
        };

        toast.ShowToast(data);
    }
}

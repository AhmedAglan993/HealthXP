using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIScreen : MonoBehaviour, IScreen
{
    [SerializeField] protected CanvasGroup canvasGroup;
    [SerializeField] public bool isHomeScreen;

    public bool IsVisible => canvasGroup.alpha > 0;

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}

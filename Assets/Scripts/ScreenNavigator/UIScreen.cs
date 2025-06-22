using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIScreen : MonoBehaviour, IScreen
{
    [SerializeField] protected CanvasGroup canvasGroup;
    [SerializeField] public bool isHomeScreen;
    [SerializeField] public bool isSplash;
    public ScreenId screenId;


    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

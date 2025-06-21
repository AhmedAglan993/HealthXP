using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIToastMessage : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private Image background;

    [Header("Style Colors")]
    public Color successColor = new Color(0.13f, 0.83f, 0.68f);
    public Color errorColor = new Color(1f, 0.42f, 0.42f);
    public Color infoColor = new Color(1f, 0.8f, 0.5f);

    public void ShowToast(ToastData data)
    {
        messageText.text = data.message;
        background.color = GetColor(data.type);

        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, 0.4f).OnComplete(() =>
        {
            DOVirtual.DelayedCall(data.duration, () =>
            {
                canvasGroup.DOFade(0, 0.4f);
                DestroyImmediate(gameObject);
            });
        });
    }

    private Color GetColor(ToastData.ToastType type)
    {
        return type switch
        {
            ToastData.ToastType.Success => successColor,
            ToastData.ToastType.Error => errorColor,
            ToastData.ToastType.Info => infoColor,
            _ => infoColor
        };
    }
}

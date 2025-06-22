using UnityEngine;
using TMPro;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class LoadingView : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private float fadeDuration = 0.3f;

    public bool IsVisible => canvasGroup.alpha > 0;

    public void SetMessage(string message)
    {
        if (messageText != null)
            messageText.text = message;
    }

    public void FadeIn()
    {
        gameObject.SetActive(true);
        canvasGroup.DOFade(0.75f, fadeDuration).SetEase(Ease.OutQuad);
        canvasGroup.blocksRaycasts = true;
    }

    public void FadeOut()
    {
        canvasGroup.DOFade(0f, fadeDuration)
            .SetEase(Ease.InQuad)
            .OnComplete(() => gameObject.SetActive(false));
        canvasGroup.blocksRaycasts = false;
    }
}

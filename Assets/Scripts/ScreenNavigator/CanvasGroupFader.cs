using UnityEngine;
using System.Collections;

public class CanvasGroupFader : MonoBehaviour
{
    public IEnumerator FadeIn(CanvasGroup group, float duration = 0.3f)
    {
        group.gameObject.SetActive(true);
        group.blocksRaycasts = true;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            group.alpha = Mathf.Lerp(0, 1, t / duration);
            yield return null;
        }
        group.alpha = 1f;
    }

    public IEnumerator FadeOut(CanvasGroup group, float duration = 0.3f)
    {
        group.blocksRaycasts = false;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            group.alpha = Mathf.Lerp(1, 0, t / duration);
            yield return null;
        }
        group.alpha = 0f;
        group.gameObject.SetActive(false);
    }
}

using UnityEngine;
using System.Collections;

public class OptionPanelController : MonoBehaviour
{
    public CanvasGroup optionPanel;

    public void ShowOption()
    {
        optionPanel.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(FadeCanvas(optionPanel, 0f, 1f, 0.5f));
    }
    public void HideOption()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutAndDisable(optionPanel, 1f, 0f, 0.5f));
    }

    private IEnumerator FadeCanvas (CanvasGroup cg, float start, float end, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, elapsed / duration);
            yield return null;
        }
        cg.alpha = end;
    }
    private IEnumerator FadeOutAndDisable(CanvasGroup cg, float start, float end, float duration)
    {
        yield return FadeCanvas(cg, start, end, duration);
        cg.gameObject.SetActive(false);
    }
}

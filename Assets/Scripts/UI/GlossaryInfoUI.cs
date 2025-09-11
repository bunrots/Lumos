using UnityEngine;
using TMPro;
using System.Collections;

public class GlossaryInfoUI : MonoBehaviour
{
    public TMP_Text termText;
    public TMP_Text definitionText;

    private RectTransform rectTransform;
    private Vector2 targetAnchoredPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        targetAnchoredPosition = rectTransform.anchoredPosition;
        // Start off-screen to the right
        rectTransform.anchoredPosition = new Vector2(rectTransform.rect.width, targetAnchoredPosition.y);
    }

    public void DisplayEntry(GlossaryEntry entry)
    {
        if (termText != null) termText.text = $"<color=#FFA500>{entry.Term}</color>";
        if (definitionText != null) definitionText.text = entry.Definition;
        StopAllCoroutines();
        StartCoroutine(SlideIn());
    }

    private IEnumerator SlideIn()
    {
        float duration = 0.3f;
        float elapsed = 0f;
        Vector2 startPos = rectTransform.anchoredPosition;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetAnchoredPosition, Mathf.SmoothStep(0f, 1f, elapsed / duration));
            yield return null;
        }
        rectTransform.anchoredPosition = targetAnchoredPosition;
    }
}
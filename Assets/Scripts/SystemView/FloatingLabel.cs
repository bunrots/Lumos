using UnityEngine;
using TMPro;

public class FloatingLabel : MonoBehaviour
{
    public Transform target; // planet transform
    public RectTransform canvasRect;
    public TextMeshPro labelText;
    public Vector2 screenOffset = new Vector2(0, 20); // pixels above planet
    public bool showLabels = true;
    public float minVisibleDistance = 5f;

void Awake()
{
    // Auto-assign the TMP component if not manually set
    if (labelText == null)
    {
        labelText = GetComponent<TextMeshPro>();
        if (labelText == null)
        {
            Debug.LogError("FloatingLabel: TextMeshPro component not found on the same GameObject!");
        }
    }
}

    void LateUpdate()
    {
        if (!target) return;
        if (canvasRect == null) return;

        Camera cam = Camera.main;
        Vector3 screenPos = cam.WorldToScreenPoint(target.position);

        if (screenPos.z < 0) // behind camera
        {
            labelText.enabled = false;
            return;
        }

        float dist = Vector3.Distance(cam.transform.position, target.position);

        if (!showLabels || dist < minVisibleDistance)
        {
            labelText.enabled = false;
            return;
        }

        labelText.enabled = true;

        // Convert to canvas local position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, cam, out Vector2 localPoint);
        Vector2 targetPos = localPoint + screenOffset;

        // Optional: scale by distance
        float targetScaleFactor = Mathf.Clamp(1f / (dist * 0.02f), 0.6f, 1.4f);
        Vector3 targetScale = Vector3.one * targetScaleFactor;

        // Smoothly interpolate position and scale
        float smoothing = 10f;
        labelText.rectTransform.anchoredPosition = Vector2.Lerp(labelText.rectTransform.anchoredPosition, targetPos, Time.deltaTime * smoothing);
        labelText.rectTransform.localScale = Vector3.Lerp(labelText.rectTransform.localScale, targetScale, Time.deltaTime * smoothing);
    }

    public void SetCanvasRect(RectTransform rect)
    {
        canvasRect = rect;
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlanetSizeComparison : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform earthCircle;
    public RectTransform planetCircle;
    public Image earthImage;
    public Image planetImage;
    public TextMeshProUGUI planetNameText;

    [Header("Planet Data")]
    public float earthRadius = 1f;        // in Earth units
    public float planetRadius = 1f;       // in Earth units

    [Header("Colors")]
    // Earth: subtle off-white; Planet: soft pastel muted gold
    public Color earthColor = new Color(0.9f, 0.9f, 0.9f);
    public Color planetColor = new Color(0.8f, 0.6f, 0.2f);

    [Header("Max Scale")]
    public float maxCircleSize = 100f;    // max size in pixels for the largest circle

    public void UpdateComparison(float planetR, string planetName = null)
    {
        planetRadius = planetR;

        // Scale both circles using Earth-relative scaling
        float earthScale = earthRadius * maxCircleSize;
        float planetScale = planetRadius * maxCircleSize;

        earthCircle.sizeDelta = new Vector2(earthScale, earthScale);
        planetCircle.sizeDelta = new Vector2(planetScale, planetScale);

        // Set colors
        earthImage.color = earthColor;
        planetImage.color = planetColor;

        // Decide sibling order
        if (planetRadius > earthRadius)
        {
            planetCircle.SetAsFirstSibling(); // behind
            earthCircle.SetAsLastSibling();   // in front
        }
        else
        {
            earthCircle.SetAsFirstSibling();  // behind
            planetCircle.SetAsLastSibling();  // in front
        }

        if (planetNameText != null && planetName != null)
        {
            planetNameText.text = planetName;
        }
    }
}
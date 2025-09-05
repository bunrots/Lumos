using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class PlanetInfoUI : MonoBehaviour {
    [Header("Texts")]
    public TMP_Text nameText;
    public TMP_Text labelsText;
    public TMP_Text probsText;
    public TMP_Text interestText;
    public TMP_Text descriptionText;
    public TMP_Text orbitalText;
    public TMP_Text semiMajorText;
    public TMP_Text radiusText;
    public TMP_Text massText;
    public TMP_Text equilibriumTempText;

    [Header("3D Preview")]
    public MeshRenderer planetRenderer;

    [Header("Spectrum UI")]
    public RawImage spectrumImage;

    public void DisplayPlanet(PlanetData data) {
        if (data == null) {
            Debug.LogError("PlanetData passed is null!");
            return;
        }

        // Text fields (safety checks)
        if (nameText) nameText.text = data.Planet_ID;
        if (orbitalText) orbitalText.text = $"Orbital Period (days): {data.Orbital_Period_days}";
        if (semiMajorText) semiMajorText.text = $"Semi-Major Axis (AU): {data.Semi_Major_Axis_AU}";
        if (radiusText) radiusText.text = $"Radius (Earth): {data.Radius_Earth}";
        if (massText) massText.text = $"Mass (Earth): {data.Mass_Earth}";
        if (equilibriumTempText) equilibriumTempText.text = $"Equilibrium Temp (K): {data.Equilibrium_Temp_K}";

        // Labels convenience
        string GetLabel(string gas) {
            if (data.Labels != null) {
                for (int i = 0; i < data.Labels.Length; i++) {
                    var kv = data.Labels[i];
                    if (kv != null && kv.key == gas) return kv.value == 1 ? "Yes" : "No";
                }
            }
            return "No";
        }

        float GetProb(string gas) {
            if (data.Probabilities != null) {
                for (int i = 0; i < data.Probabilities.Length; i++) {
                    var kv = data.Probabilities[i];
                    if (kv != null && kv.key == gas) return kv.value;
                }
            }
            return -1f;
        }

        if (labelsText)
            labelsText.text = "Detected Gases:\n" +
                              $"- CH₄: {GetLabel("CH4")}\n" +
                              $"- O₃: {GetLabel("O3")}\n" +
                              $"- H₂O: {GetLabel("H2O")}";

        if (probsText) {
            float pCh4 = GetProb("CH4");
            float pO3  = GetProb("O3");
            float pH2o = GetProb("H2O");
            probsText.text = "Probabilities:\n" +
                             $"- CH₄: {(pCh4 >= 0 ? pCh4.ToString("F2") : "N/A")}\n" +
                             $"- O₃: {(pO3  >= 0 ? pO3.ToString("F2")  : "N/A")}\n" +
                             $"- H₂O: {(pH2o >= 0 ? pH2o.ToString("F2") : "N/A")}";
        }

        if (interestText) interestText.text = data.Overall_Interest ? "Overall Interest: Yes" : "Overall Interest: No";
        if (descriptionText) descriptionText.text = data.Description;

        // --- Apply planet texture to the 3D preview renderer ---
        if (planetRenderer != null && !string.IsNullOrEmpty(data.Texture)) {
            string baseName = Path.GetFileNameWithoutExtension(data.Texture);
            Texture2D planetTex = Resources.Load<Texture2D>($"Images/Planets/{baseName}");
            if (planetTex != null) {
                Material mat = planetRenderer.material;
                if (mat.HasProperty("_BaseMap"))      mat.SetTexture("_BaseMap", planetTex);
                else if (mat.HasProperty("_MainTex")) mat.SetTexture("_MainTex", planetTex);
                else                                     mat.mainTexture = planetTex;
            } else {
                Debug.LogWarning($"Planet texture not found at Resources/Images/Planets/{baseName}");
            }
        }

        // --- Load spectrum image into RawImage ---
        if (spectrumImage != null) {
            if (!string.IsNullOrEmpty(data.SpectrumImage)) {
                string baseName = Path.GetFileNameWithoutExtension(data.SpectrumImage);
                Texture2D specTex = Resources.Load<Texture2D>($"Images/Spectra/{baseName}");
                if (specTex != null) {
                    spectrumImage.texture = specTex;
                    var c = spectrumImage.color; c.a = 1f; spectrumImage.color = c;
                } else {
                    Debug.LogWarning($"Spectrum image not found at Resources/Images/Spectra/{baseName}");
                    spectrumImage.texture = null;
                }
            } else {
                spectrumImage.texture = null;
            }
        }

        // --- Planet scaling based on Radius_Earth ---
        // Use static largest planet radius from PlanetManager for scaling reference
        float largestPlanetRadius = PlanetManager.LargestPlanetRadius;
        // Maximum scale for the largest planet (Unity units)
        float maxPlanetScale = 1.0f;
        if (planetRenderer != null) {
            // Safety check for null/zero largestPlanetRadius
            if (largestPlanetRadius > 0f) {
                // Only scale down smaller planets
                if (data.Radius_Earth < largestPlanetRadius) {
                    float scaleFactor = (data.Radius_Earth / largestPlanetRadius) * maxPlanetScale;
                    planetRenderer.transform.localScale = Vector3.one * scaleFactor;
                } else {
                    planetRenderer.transform.localScale = Vector3.one * maxPlanetScale; // largest stays at max
                }
            } else {
                // Fallback: set to max scale if largest radius is not set
                planetRenderer.transform.localScale = Vector3.one * maxPlanetScale;
            }
        }
    }
}
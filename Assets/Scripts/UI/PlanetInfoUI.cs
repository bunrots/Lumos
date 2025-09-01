using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class PlanetInfoUI : MonoBehaviour {
    [Header("Texts")]
    public TMP_Text nameText;
    public TMP_Text snrText;
    public TMP_Text labelsText;
    public TMP_Text probsText;
    public TMP_Text interestText;
    public TMP_Text descriptionText;

    [Header("3D Preview")]
    // Assign the MeshRenderer of your spinning planet prefab (the MeshRenderer component, not the GameObject)
    public MeshRenderer planetRenderer;

    [Header("Spectrum UI")]
    // Assign the RawImage that should show the spectrum image
    public RawImage spectrumImage;

    public void DisplayPlanet(PlanetData data) {
        if (data == null) {
            Debug.LogError("PlanetData passed is null!");
            return;
        }

        // Text fields (safety checks)
        if (nameText) nameText.text = data.Planet_ID;
        if (snrText) snrText.text = $"SNR: {data.Sim_SNR}";

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
                Material mat = planetRenderer.material; // creates an instance at runtime
                if (mat.HasProperty("_BaseMap"))      mat.SetTexture("_BaseMap", planetTex);   // URP/HDRP
                else if (mat.HasProperty("_MainTex")) mat.SetTexture("_MainTex", planetTex);   // Built-in
                else                                     mat.mainTexture = planetTex;             // fallback
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
                    // Optional: clear texture to avoid showing stale image
                    spectrumImage.texture = null;
                }
            } else {
                // No spectrum specified - clear image
                spectrumImage.texture = null;
            }
        }
    }
}
using UnityEngine;
using TMPro;
using System.Linq;
public class PlanetInfoUI : MonoBehaviour {
    public TMP_Text nameText;
    public TMP_Text snrText;
    public TMP_Text labelsText;
    public TMP_Text probsText;
    public TMP_Text interestText;
    public TMP_Text descriptionText;
    public UnityEngine.UI.RawImage planetImage;
    public UnityEngine.UI.RawImage spectrumImage;

    public void DisplayPlanet(PlanetData data) {
        if (data == null)
        {
            Debug.LogError("PlanetData passed is null!");
            return;
        }

        if (nameText == null)
            Debug.LogError("nameText is not assigned!");
        if (snrText == null)
            Debug.LogError("snrText is not assigned!");
        if (labelsText == null)
            Debug.LogError("labelsText is not assigned!");
        if (probsText == null)
            Debug.LogError("probsText is not assigned!");
        if (interestText == null)
            Debug.LogError("interestText is not assigned!");

        Debug.Log($"Planet ID: {data.Planet_ID}");
        Debug.Log($"SNR: {data.Sim_SNR}");
        Debug.Log($"Labels is null? {data.Labels == null}");
        Debug.Log($"Probabilities is null? {data.Probabilities == null}");
        Debug.Log($"Overall Interest: {data.Overall_Interest}");
        
        nameText.text = data.Planet_ID;
        snrText.text = $"SNR: {data.Sim_SNR}";

        bool HasLabel(string gas) =>
            data.Labels != null && data.Labels.Any(kv => kv.key == gas && kv.value == 1);
        
        float GetProbability(string gas) =>
            data.Probabilities != null && data.Probabilities.Any(kv => kv.key == gas)
                ? data.Probabilities.First(kv => kv.key == gas).value
                : -1f;

        labelsText.text =
            $"Detected Gases:\n" +
            $"- CH₄: {(HasLabel("CH4") ? "Yes" : "No")}\n" +
            $"- O₃: {(HasLabel("O3") ? "Yes" : "No")}\n" +
            $"- H₂O: {(HasLabel("H2O") ? "Yes" : "No")}";

        probsText.text =
            $"Probabilities:\n" +
            $"- CH₄: {(GetProbability("CH4") >= 0 ? GetProbability("CH4").ToString("F2") : "N/A")}\n" +
            $"- O₃: {(GetProbability("O3") >= 0 ? GetProbability("O3").ToString("F2") : "N/A")}\n" +
            $"- H₂O: {(GetProbability("H2O") >= 0 ? GetProbability("H2O").ToString("F2") : "N/A")}";

        interestText.text = data.Overall_Interest ? "Overall Interest: Yes" : "Overall Interest: No";

        if (descriptionText != null)
            descriptionText.text = data.Description;

        if (planetImage != null && !string.IsNullOrEmpty(data.TextureName)) {
            string texPath = $"Images/Planets/{data.TextureName}";
            Texture2D tex = Resources.Load<Texture2D>(texPath);
            if (tex != null) planetImage.texture = tex;
            else Debug.LogWarning($"Texture not found at {texPath}");
        }

        if (spectrumImage != null && !string.IsNullOrEmpty(data.SpectrumName)) {
            string specPath = $"Images/Spectra/{data.SpectrumName}";
            Texture2D specTex = Resources.Load<Texture2D>(specPath);
            if (specTex != null) spectrumImage.texture = specTex;
            else Debug.LogWarning($"Spectrum image not found at {specPath}");
        }
    }
}
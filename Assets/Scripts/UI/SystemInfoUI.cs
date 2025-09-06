using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class SystemData
{
    public string Name;
    public string Summary;
    public SystemDetails Details;
}

[System.Serializable]
public class SystemDetails
{
    public string Spectral_Type;
    public float Mass_Solar;
    public float Radius_Solar;
    public float Effective_Temperature_K;
    public float Luminosity_Solar;
    public float Age_Gyr;
    public float Distance_LY;
    public string Discovery_Description;
    public string[] Notable_Characteristics;
}

public class SystemInfoUI : MonoBehaviour
{
    [Header("Texts")]
    public TMP_Text nameText;
    public TMP_Text summaryText;
    public TMP_Text spectralTypeText;
    public TMP_Text massText;
    public TMP_Text radiusText;
    public TMP_Text tempText;
    public TMP_Text luminosityText;
    public TMP_Text ageText;
    public TMP_Text distanceText;
    public TMP_Text discoveryText;
    public TMP_Text notableText;

    [Header("List Texts")]
    public TMP_Text listNameText;
    public TMP_Text listSummaryText;

    [Header("Scroll (Optional)")]
    public ScrollRect scrollRect; // assign the Scroll View component here (optional)

    public void DisplaySystem(SystemData data)
    {
        if (data == null)
        {
            Debug.LogError("SystemData is null!");
            return;
        }

        if (nameText) nameText.text = data.Name;
        if (summaryText) summaryText.text = data.Summary;
        // Update list fields as well
        if (listNameText) listNameText.text = data.Name;
        if (listSummaryText) listSummaryText.text = data.Summary;

        if (data.Details != null)
        {
            if (spectralTypeText) spectralTypeText.text = $"Spectral Type: {data.Details.Spectral_Type}";
            if (massText) massText.text = $"Mass: {data.Details.Mass_Solar} M☉";
            if (radiusText) radiusText.text = $"Radius: {data.Details.Radius_Solar} R☉";
            if (tempText) tempText.text = $"Temperature: {data.Details.Effective_Temperature_K} K";
            if (luminosityText) luminosityText.text = $"Luminosity: {data.Details.Luminosity_Solar} L☉";
            if (ageText) ageText.text = $"Age: {data.Details.Age_Gyr} Gyr";
            if (distanceText) distanceText.text = $"Distance: {data.Details.Distance_LY} LY";
            if (discoveryText) discoveryText.text = data.Details.Discovery_Description;

            if (notableText)
            {
                notableText.text = "Notable Characteristics:\n";
                if (data.Details.Notable_Characteristics != null)
                {
                    foreach (string characteristic in data.Details.Notable_Characteristics)
                    {
                        notableText.text += $"- {characteristic}\n";
                    }
                }
            }
        }
    }
    void OnEnable()
    {
        if (scrollRect != null)
        {
            StartCoroutine(SetScrollToTopNextFrame());
        }
    }

    private System.Collections.IEnumerator SetScrollToTopNextFrame()
    {
        yield return null; // wait one frame
        Canvas.ForceUpdateCanvases();
        if (scrollRect != null)
            scrollRect.verticalNormalizedPosition = 1f; // 1 = top, 0 = bottom
    }
}
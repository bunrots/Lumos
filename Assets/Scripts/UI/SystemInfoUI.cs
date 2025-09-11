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
            if (spectralTypeText) spectralTypeText.text = $"<color=#FFA500><b>Spectral Type:</b></color> <color=#FFFFFF>{data.Details.Spectral_Type}</color>";
            if (massText) massText.text = $"<color=#FFA500><b>Mass:</b></color> <color=#FFFFFF>{data.Details.Mass_Solar} M☉</color>";
            if (radiusText) radiusText.text = $"<color=#FFA500><b>Radius:</b></color> <color=#FFFFFF>{data.Details.Radius_Solar} R☉</color>";
            if (tempText) tempText.text = $"<color=#FFA500><b>Temperature:</b></color> <color=#FFFFFF>{data.Details.Effective_Temperature_K} K</color>";
            if (luminosityText) luminosityText.text = $"<color=#FFA500><b>Luminosity:</b></color> <color=#FFFFFF>{data.Details.Luminosity_Solar} L☉</color>";
            if (ageText) ageText.text = $"<color=#FFA500><b>Age:</b></color> <color=#FFFFFF>{data.Details.Age_Gyr} Gyr</color>";
            if (distanceText) distanceText.text = $"<color=#FFA500><b>Distance:</b></color> <color=#FFFFFF>{data.Details.Distance_LY} LY</color>";

            if (discoveryText)
            {
                discoveryText.text = $"<color=#FFA500><b>Discovery & Transit Method:</b></color>\n<color=#FFFFFF>{data.Details.Discovery_Description}</color>";
            }

            if (notableText)
            {
                notableText.text = "<color=#FFA500><b>Notable Characteristics:</b></color>\n";
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
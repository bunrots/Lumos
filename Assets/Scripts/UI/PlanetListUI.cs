using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlanetListUI : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform contentParent;
    public PlanetInfoUI infoUI;
    public ScreenManager screenManager;

    public PlanetData[] planets;

    // Folder inside Resources that holds planet textures (no extension in names)
    private const string PlanetTextureFolder = "Images/Planets";

    public void PopulateList(PlanetData[] loadedPlanets)
    {
        planets = loadedPlanets;

        foreach (PlanetData planet in planets)
        {
            GameObject buttonObj = Instantiate(buttonPrefab, contentParent);

            // Set button label
            TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = planet.Planet_ID;
            }

            // Use PlanetButtonPreview component to get previewRenderer
            PlanetButtonPreview preview = buttonObj.GetComponent<PlanetButtonPreview>();
            if (preview != null && preview.previewRenderer != null)
            {
                Debug.Log($"Found PlanetButtonPreview for {planet.Planet_ID}");
                if (!string.IsNullOrEmpty(planet.Texture))
                {
                    string textureName = Path.GetFileNameWithoutExtension(planet.Texture);
                    Texture2D tex = Resources.Load<Texture2D>($"{PlanetTextureFolder}/{textureName}");
                    if (tex != null)
                    {
                        Debug.Log($"Loaded texture {textureName} for {planet.Planet_ID}");
                        Material mat = preview.previewRenderer.material;
                        if (mat.HasProperty("_BaseMap")) mat.SetTexture("_BaseMap", tex);
                        else if (mat.HasProperty("_MainTex")) mat.SetTexture("_MainTex", tex);
                        else mat.mainTexture = tex;

                        // Scale preview sphere based on planet radius relative to largest planet
                        float maxScale = 1.0f;
                        float scale = maxScale;
                        if (planet.Radius_Earth < PlanetManager.LargestPlanetRadius)
                        {
                            scale = (planet.Radius_Earth / PlanetManager.LargestPlanetRadius) * maxScale;
                        }
                        preview.previewRenderer.transform.localScale = new Vector3(scale, scale, scale);
                    }
                    else
                    {
                        Debug.LogWarning($"PlanetListUI: Texture '{textureName}' not found in Resources/{PlanetTextureFolder} for planet {planet.Planet_ID}.");
                    }
                }
                else
                {
                    Debug.LogWarning($"PlanetListUI: Empty Texture for planet {planet.Planet_ID}.");
                }
            }
            else
            {
                Debug.LogWarning("PlanetListUI: No PlanetButtonPreview or previewRenderer found in button prefab for planet preview.");
            }

            // Wire up button click -> show detail panel
            Button button = buttonObj.GetComponent<Button>();
            if (button != null)
            {
                PlanetData captured = planet;
                button.onClick.AddListener(() =>
                {
                    infoUI.DisplayPlanet(captured);
                    screenManager.ShowPlanetInfo();
                });
            }
        }
    }
}
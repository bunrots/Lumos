using UnityEngine;
using TMPro;

public class SystemPlanetDropdown : MonoBehaviour
{
    public PlanetSystemManager systemManager;
    public OrbitCamera orbitCamera;
    public TMP_Dropdown dropdown;
    public TMP_Text cornerText;

    void Start()
    {
        if (systemManager == null || dropdown == null)
        {
            Debug.LogError("SystemPlanetDropdown not set up properly!");
            return;
        }
    }

    public void PopulateDropdown()
    {
        dropdown.ClearOptions();

        var options = new System.Collections.Generic.List<string>();

        for (int i = 0; i < systemManager.spawnedPlanets.Count; i++)
        {
            GameObject planet = systemManager.spawnedPlanets[i];
            if (planet != null)
            {
                options.Add(planet.name);
            }
        }

        dropdown.AddOptions(options);

        if (cornerText != null)
        {
            cornerText.text = "TRAPPIST-1";
        }

        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    void OnDropdownValueChanged(int index)
    {
        if (index >= 0 && index < systemManager.spawnedPlanets.Count)
        {
            GameObject targetPlanet = systemManager.spawnedPlanets[index];
            if (targetPlanet != null && orbitCamera != null)
            {
                float zoomDistance = 30f; // You can adjust this default zoom distance as needed
                orbitCamera.SetTargetSmooth(targetPlanet.transform, zoomDistance);
                if (cornerText != null)
                {
                    cornerText.text = targetPlanet.name;
                }
            }
        }
    }
}
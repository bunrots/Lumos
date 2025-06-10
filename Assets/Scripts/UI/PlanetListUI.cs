using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlanetListUI : MonoBehaviour
{
    public GameObject buttonPrefab;              // The PlanetButton prefab
    public Transform contentParent;              // Content object in Scroll View
    public PlanetInfoUI infoUI;                  // Reference to detail UI
    public PlanetSystem[] planetSystems;         // Loaded in from manager

    public void PopulateList(PlanetSystem[] systems)
    {
        planetSystems = systems;

        // Clear old buttons if any
        // foreach (Transform child in contentParent)
        // {
        //     Destroy(child.gameObject);
        // }

        // Create a button for each planet in each system
        foreach (PlanetSystem system in systems)
        {
            foreach (PlanetData planet in system.planets)
            {
                GameObject buttonObj = Instantiate(buttonPrefab, contentParent);
                TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();
                buttonText.text = $"{planet.name}";

                Button button = buttonObj.GetComponent<Button>();
                // Debug.Log($"Button created for {planet.name} at position: {buttonObj.GetComponent<RectTransform>().anchoredPosition}");
                PlanetData captured = planet; // Capture in closure
                button.onClick.AddListener(() => {
                    infoUI.DisplayPlanet(captured);
                });
            }
        }
    }
}

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

    public void PopulateList(PlanetData[] loadedPlanets)
    {
        planets = loadedPlanets;

        // Clear previous entries
        // foreach (Transform child in contentParent)
        // {
        //     Destroy(child.gameObject);
        // }

        foreach (PlanetData planet in planets)
        {
            GameObject buttonObj = Instantiate(buttonPrefab, contentParent);
            TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();
            buttonText.text = planet.Planet_ID;

            Button button = buttonObj.GetComponent<Button>();
            PlanetData captured = planet;
            button.onClick.AddListener(() =>
            {
                infoUI.DisplayPlanet(captured);
                screenManager.ShowPlanetInfo();
            });
        }
    }
}
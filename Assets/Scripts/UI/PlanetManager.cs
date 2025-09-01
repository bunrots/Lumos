using UnityEngine;

public class PlanetManager : MonoBehaviour {
    public PlanetInfoUI ui;
    public PlanetListUI listUI;

    private PlanetData[] planets;

    void Start() {
        LoadPlanets();
    }

    void LoadPlanets() {
        TextAsset json = Resources.Load<TextAsset>("PlanetData/planetdata");
        if (json == null) {
            Debug.LogError("Could not find planetdata.json in Resources/PlanetData/");
            return;
        }

        string wrappedJson = "{\"planets\":" + json.text + "}";
        PlanetList planetList = JsonUtility.FromJson<PlanetList>(wrappedJson);
        planets = planetList.planets;

        if (planets != null && planets.Length > 0) {
            Debug.Log($"Loaded planet {planets[0].Planet_ID} with description: {planets[0].Description}");
            if (ui != null)
                ui.DisplayPlanet(planets[0]);
            else
                Debug.LogError("PlanetInfoUI reference is not assigned in PlanetManager!");
            if (listUI != null)
                listUI.PopulateList(planets);
            else
                Debug.LogError("PlanetListUI reference is not assigned in PlanetManager!");
        } else {
            Debug.LogWarning("No planets found!");
        }
    }
}
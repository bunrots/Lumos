using UnityEngine;

public class PlanetManager : MonoBehaviour {
    public PlanetInfoUI ui;

    private PlanetData[] planets;

    void Start() {
        LoadPlanetList();
        
        if (planets != null && planets.Length > 0) {
            ui.DisplayPlanet(planets[0]); // Display first planet by default
        } else {
            Debug.LogWarning("No planet data found!");
        }
    }

    void LoadPlanetList() {
        TextAsset json = Resources.Load<TextAsset>("PlanetData/planetdata");
        if (json == null) {
            Debug.LogError("Could not find planetdata.json in Resources/PlanetData/");
            return;
        }

        PlanetList planetList = JsonUtility.FromJson<PlanetList>(json.text);
        planets = planetList.planets;
    }
}

using UnityEngine;

public class PlanetManager : MonoBehaviour {
    public PlanetInfoUI ui;
    public PlanetListUI listUI;

    private PlanetSystem[] systems;

    void Start() {
        LoadPlanetSystems();
    }

    void LoadPlanetSystems() {
        TextAsset json = Resources.Load<TextAsset>("PlanetData/planetdata");
        if (json == null) {
            Debug.LogError("Could not find planetdata.json in Resources/PlanetData/");
            return;
        }

        PlanetSystemList systemList = JsonUtility.FromJson<PlanetSystemList>(json.text);
        systems = systemList.systems;

        if (systems != null && systems.Length > 0 && systems[0].planets.Length > 0) {
            ui.DisplayPlanet(systems[0].planets[0]);
            listUI.PopulateList(systems);
        } else {
            Debug.LogWarning("No systems or planets found!");
        }
    }
}


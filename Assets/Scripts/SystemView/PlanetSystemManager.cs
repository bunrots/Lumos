using UnityEngine;
using System.IO;

public class PlanetSystemManager : MonoBehaviour
{
    [Header("References")]
    public Transform star;                // central star
    public GameObject[] planetPrefabs;    // assign 7 prefabs (with OrbitController)
    public TextAsset planetJson;          // drag your planetdata.json here

    private PlanetData[] planets;

    void Start()
    {
        if (planetJson == null)
        {
            Debug.LogError("Planet JSON not assigned!");
            return;
        }

        // Parse JSON
        string wrappedJson = "{\"planets\":" + planetJson.text + "}";
        PlanetList planetList = JsonUtility.FromJson<PlanetList>(wrappedJson);
        planets = planetList.planets;

        if (planets == null || planets.Length == 0)
        {
            Debug.LogError("No planets found in JSON!");
            return;
        }

        // Spawn planets using matching prefabs
        for (int i = 0; i < planets.Length; i++)
        {
            if (i < planetPrefabs.Length && planetPrefabs[i] != null)
            {
                SpawnPlanet(planets[i], planetPrefabs[i]);
            }
            else
            {
                Debug.LogWarning($"No prefab assigned for planet index {i} ({planets[i].Planet_ID})");
            }
        }
    }

    private void SpawnPlanet(PlanetData data, GameObject prefab)
    {
        // Instantiate planet without parenting to the star to allow independent orbit in world space
        GameObject planet = Instantiate(prefab, Vector3.zero, Quaternion.identity);

        planet.name = data.Planet_ID;

        // Assign orbit controller
        OrbitController orbit = planet.GetComponent<OrbitController>();
        if (orbit != null)
        {
            orbit.star = star;
            orbit.semiMajorAxisAU = data.Semi_Major_Axis_AU;
            orbit.orbitalPeriodDays = data.Orbital_Period_days;
        }
        else
        {
            Debug.LogWarning($"No OrbitController found on prefab {planet.name}");
        }
    }
}
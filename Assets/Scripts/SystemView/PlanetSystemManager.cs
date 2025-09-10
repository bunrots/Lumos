using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class PlanetSystemManager : MonoBehaviour
{
    [Header("Orbit Speed Slider Steps")]
    public float[] orbitSpeedSteps = new float[] { 1f, 10f, 100f, 1000f, 10000f, 100000f };
    [Header("References")]
    public Transform star;                // central star
    public GameObject[] planetPrefabs;    // assign 7 prefabs (with OrbitController)
    public TextAsset planetJson;          // drag your planetdata.json here
    public GameObject labelPrefab;        // prefab for floating label
    public RectTransform canvasTransform; // canvas to hold labels
    public Transform labelsParent;
    public Transform planetsParent;
    public SystemPlanetDropdown systemDropdown;
    public Slider orbitSpeedSlider;
    public TextMeshProUGUI speedLabel;

    [Header("Label Settings")]
    public bool labelsVisible = true;

    public List<GameObject> spawnedPlanets = new List<GameObject>();

    private PlanetData[] planets;
    private bool lastLabelsVisible;

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

        if (systemDropdown != null)
        {
            systemDropdown.PopulateDropdown();
        }

        if (orbitSpeedSlider != null)
        {
            orbitSpeedSlider.wholeNumbers = true;
            orbitSpeedSlider.minValue = 0;
            orbitSpeedSlider.maxValue = orbitSpeedSteps.Length - 1;
            orbitSpeedSlider.value = 4;
            SetOrbitSpeed(orbitSpeedSlider.value);
            orbitSpeedSlider.onValueChanged.AddListener(SetOrbitSpeed);
        }

        lastLabelsVisible = labelsVisible;
        if (labelsParent != null)
        {
            labelsParent.gameObject.SetActive(labelsVisible);
        }

    }

    private void SpawnPlanet(PlanetData data, GameObject prefab)
    {
        // Instantiate planet without parenting to the star to allow independent orbit in world space
        GameObject planet = Instantiate(prefab, Vector3.zero, Quaternion.identity);

        if (planetsParent != null)
        {
            planet.transform.SetParent(planetsParent, worldPositionStays: true);
        }

        spawnedPlanets.Add(planet);

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

        if (labelPrefab != null && labelsParent != null)
        {
            GameObject labelObj = Instantiate(labelPrefab, labelsParent);
            labelObj.SetActive(labelsVisible);
            FloatingLabel label = labelObj.GetComponent<FloatingLabel>();
            if (label != null)
            {
                label.target = planet.transform;
                label.SetCanvasRect(canvasTransform);
                label.labelText.text = planet.name;
            }
            else
            {
                Debug.LogWarning("FloatingLabel component not found on labelPrefab");
            }
        }
    }

    void Update()
    {
        if (labelsVisible != lastLabelsVisible)
        {
            if (labelsParent != null)
            {
                labelsParent.gameObject.SetActive(labelsVisible);
            }
            lastLabelsVisible = labelsVisible;
        }
    }
    // Public method to toggle label visibility, assignable to UI button OnClick
    public void ToggleLabelsButton()
    {
        labelsVisible = !labelsVisible;
        if (labelsParent != null)
        {
            labelsParent.gameObject.SetActive(labelsVisible);
        }
    }
    // Public method to move camera to focus on the star
    public void FocusStar()
    {
        if (star != null && systemDropdown != null && systemDropdown.orbitCamera != null)
        {
            systemDropdown.orbitCamera.SetTargetSmooth(star, 60);
            if (systemDropdown.cornerText != null)
            {
                systemDropdown.cornerText.text = "TRAPPIST-1";
            }
        }
    }
    public void FocusSystem()
    {
        if (star != null && systemDropdown != null && systemDropdown.orbitCamera != null)
        {
            systemDropdown.orbitCamera.SetTargetSmooth(star, 900);
            if (systemDropdown.cornerText != null)
            {
                systemDropdown.cornerText.text = "TRAPPIST-1";
            }
        }
    }
    // Public method to set orbit speed multiplier for all spawned planets
    public void SetOrbitSpeed(float speedIndex)
    {
        int idx = Mathf.Clamp((int)speedIndex, 0, orbitSpeedSteps.Length - 1);
        OrbitController.speedMultiplier = orbitSpeedSteps[idx];
        if (speedLabel != null)
            speedLabel.text = orbitSpeedSteps[idx].ToString("N0") + "x Speed";
    }
}

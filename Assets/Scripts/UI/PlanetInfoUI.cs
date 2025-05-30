using TMPro;
using UnityEngine;

public class PlanetInfoUI : MonoBehaviour {
    public TMP_Text nameText;
    public TMP_Text radiusText;
    public TMP_Text orbitText;
    public TMP_Text scoreText;
    public TMP_Text featuresText;

    public void DisplayPlanet(PlanetData data) {
        nameText.text = data.name;
        radiusText.text = $"Radius: {data.radius} R⊕";
        orbitText.text = $"Orbital Period: {data.orbital_period} days";
        scoreText.text = $"Score: {data.biosignature_score:F2}";
        featuresText.text = "Top Features:\n" + string.Join(", ", data.top_features);
    }
}

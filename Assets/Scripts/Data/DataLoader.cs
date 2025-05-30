using UnityEngine;

public static class DataLoader {
    public static PlanetData LoadPlanet(string filename) {
        TextAsset json = Resources.Load<TextAsset>($"PlanetData/{filename}");
        return JsonUtility.FromJson<PlanetData>(json.text);
    }
}

[System.Serializable]
public class PlanetSystem {
    public string name;
    public PlanetData[] planets;
}

[System.Serializable]
public class PlanetSystemList {
    public PlanetSystem[] systems;
}

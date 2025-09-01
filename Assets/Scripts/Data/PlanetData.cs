using System;
using System.Collections.Generic;

[Serializable]
public class KeyValueInt {
    public string key;
    public int value;
}

[Serializable]
public class KeyValueFloat {
    public string key;
    public float value;
}

[Serializable]
public class PlanetData {
    public string Planet_ID;
    public int Sim_SNR;
    public KeyValueInt[] Labels;
    public KeyValueFloat[] Probabilities;
    public bool Overall_Interest;

    // New fields for description and file names for textures/spectra
    public string Description;
    public string TextureName;
    public string SpectrumName;
}
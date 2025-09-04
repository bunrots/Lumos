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
    public string Texture;        // e.g. "trappist1e_texture" (no extension necessary)
    public string SpectrumImage;  // e.g. "trappist1e_spectrum" (no extension necessary)

    public float Orbital_Period_days;
    public float Semi_Major_Axis_AU;
    public float Radius_Earth;
    public float Mass_Earth;
    public float Equilibrium_Temp_K;
}
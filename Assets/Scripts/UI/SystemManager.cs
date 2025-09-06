using UnityEngine;

public class SystemManager : MonoBehaviour
{
    public SystemInfoUI systemInfoUI;

    void Start()
    {
        LoadSystemData();
    }

    void LoadSystemData()
    {
        TextAsset json = Resources.Load<TextAsset>("PlanetData/systemdata");
        if (json == null)
        {
            Debug.LogError("Could not find systemdata.json in Resources/PlanetData/");
            return;
        }

        SystemData systemData = JsonUtility.FromJson<SystemData>(json.text);
        if (systemData != null && systemInfoUI != null)
        {
            systemInfoUI.DisplaySystem(systemData);
        }
    }
}
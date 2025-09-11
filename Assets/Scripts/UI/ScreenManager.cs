using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public GameObject mainScreenPanel;
    public GameObject planetListPanel;
    public GameObject planetInfoPanel;
    public GameObject systemInfoPanel;
    public GameObject glossaryPanel;

    void Start()
    {
        ShowMainScreen(); 
    }

    public void ShowMainScreen()
    {
        mainScreenPanel.SetActive(true);
        planetListPanel.SetActive(false);
        planetInfoPanel.SetActive(false);
        if (systemInfoPanel != null) systemInfoPanel.SetActive(false);
        if (glossaryPanel != null) glossaryPanel.SetActive(false);
    }

    public void ShowPlanetList()
    {
        mainScreenPanel.SetActive(false);
        planetListPanel.SetActive(true);
        planetInfoPanel.SetActive(false);
        if (systemInfoPanel != null) systemInfoPanel.SetActive(false);
        if (glossaryPanel != null) glossaryPanel.SetActive(false);
    }

    public void ShowPlanetInfo()
    {
        mainScreenPanel.SetActive(false);
        planetListPanel.SetActive(false);
        planetInfoPanel.SetActive(true);
        if (systemInfoPanel != null) systemInfoPanel.SetActive(false);
        if (glossaryPanel != null) glossaryPanel.SetActive(false);
    }

    public void ShowSystemInfo()
    {
        mainScreenPanel.SetActive(false);
        planetListPanel.SetActive(false);
        planetInfoPanel.SetActive(false);
        if (systemInfoPanel != null) systemInfoPanel.SetActive(true);
        if (glossaryPanel != null) glossaryPanel.SetActive(false);
    }

    public void ShowGlossary()
    {
        mainScreenPanel.SetActive(false);
        planetListPanel.SetActive(false);
        planetInfoPanel.SetActive(false);
        if (systemInfoPanel != null) systemInfoPanel.SetActive(false);
        if (glossaryPanel != null) glossaryPanel.SetActive(true);
    }
}

using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public GameObject mainScreenPanel;
    public GameObject planetListPanel;
    public GameObject planetInfoPanel;


    void Start()
    {
        ShowMainScreen(); 
    }

    public void ShowMainScreen()
    {
        mainScreenPanel.SetActive(true);
        planetListPanel.SetActive(false);
        planetInfoPanel.SetActive(false);
    }

    public void ShowPlanetList()
    {
        mainScreenPanel.SetActive(false);
        planetListPanel.SetActive(true);
        planetInfoPanel.SetActive(false);
    }

    public void ShowPlanetInfo()
    {
        mainScreenPanel.SetActive(false);
        planetListPanel.SetActive(false);
        planetInfoPanel.SetActive(true);
    }
}

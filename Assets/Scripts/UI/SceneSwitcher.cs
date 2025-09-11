using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    // Call this function from any button
    public void SwitchToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void GoToMainUI()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void GoToSystemView()
    {
        SceneManager.LoadScene("SpaceScene");
    }

    public void GoToWalkaround()
    {
        SceneManager.LoadScene("SimulationScene");
    }
}
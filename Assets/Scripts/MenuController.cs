using UnityEngine;
using UnityEngine.SceneManagement;   // for loading scenes

public class MenuController : MonoBehaviour
{
    [SerializeField] private string simulationSceneName = "Simulation";  // must match your sim scene's exact name

    // Wire to the Start button.
    public void StartSimulation()
    {
        SceneManager.LoadScene(simulationSceneName);
    }

    // Wire to the Quit button.
    public void QuitGame()
    {
        Application.Quit();
        // Note: Application.Quit does nothing in the editor or in a WebGL browser build;
        // it only really quits on a standalone or Android build.
    }
}
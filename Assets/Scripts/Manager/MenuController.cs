using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public string menuSceneName = "MainMenu";
    public string gameSceneName = "Persistent";

    public string optionSceneName = "OptionMenu";
 
    public void StartGame()
    {
        Debug.Log("Start Game: " + gameSceneName);
        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenSettings()
    {
        Debug.Log("Open Settings");
        SceneManager.LoadScene("OptionMenu");
    }

    public void BackToMain()
    {
        Debug.Log("MainMenu");
        SceneManager.LoadScene(menuSceneName);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
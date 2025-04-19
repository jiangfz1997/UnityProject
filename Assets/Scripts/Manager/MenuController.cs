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
        // check difficulty
        if (PlayerPrefs.GetFloat("Difficulty") == 0 || PlayerPrefs.GetFloat("Difficulty") < 1)
        {
            PlayerPrefs.SetFloat("Difficulty", 1);
        }
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
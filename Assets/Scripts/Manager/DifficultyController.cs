using UnityEngine;
using UnityEngine.UI;

public class DifficultyController: MonoBehaviour
{
    public GameObject AlertText;
    private string notes = "Now you will receive damage and rewards at a ratio of ";
    public void SetDifficulty(float difficulty)
    {
        PlayerPrefs.SetFloat("Difficulty", difficulty);
        PlayerPrefs.Save();
        ShowAlertText(difficulty);
    }

    public void ShowAlertText(float difficulty)
    {
        Debug.Log("Difficulty set to: " + PlayerPrefs.GetFloat("Difficulty"));
        AlertText.GetComponent<Text>().text = notes + difficulty.ToString();
        AlertText.SetActive(true);
    }
}

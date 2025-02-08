using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneResetTrigger : MonoBehaviour
{

    
    // Temp use for testing, reset the whole scene when player enters the trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player entered the reset trigger");
            ReloadScene();
        }

        
    }
    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

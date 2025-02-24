using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;
    private GameObject player;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }

        if (scene.name == "Context")
        {
            FreezePlayer();  
        }
        else if (scene.name == "Level_1")
        {
            UnfreezePlayer();  
        }
    }

    private void FreezePlayer()
    {
        if (player)
        {
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;  // **��ֹ����**
            rb.linearVelocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
        }
    }

    private void UnfreezePlayer()
    {
        if (player)
        {
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;  // **�ָ�����**
            rb.constraints = RigidbodyConstraints2D.None;
        }
    }
}

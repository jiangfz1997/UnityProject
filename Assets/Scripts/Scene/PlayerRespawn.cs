using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerRespawn : MonoBehaviour
{
    private PlayerStats stats;
    [SerializeField] private Animator animator;

    public void Start()
    {
        stats = GetComponent<PlayerStats>();
    }

    public void OnDeathAnimationEnd()
    {
        Debug.Log("Dead Animation finished");
        Invoke(nameof(ReloadScene), 1f);
    }

    void ReloadScene()
    {
        // LevelManager.Instance.RestartLevel();
        // animator.SetBool("isDead", false);
        // animator.SetTrigger("Respawn");
        // stats.Restart();
    }
}

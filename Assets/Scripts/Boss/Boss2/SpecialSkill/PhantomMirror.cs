using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhantomMirrors : MonoBehaviour
{
    public GameObject prefab;
    public Transform playerTransform;
    [SerializeField] private Transform[] spawnPositions; 
    [SerializeField] private List<GameObject> attackers = new List<GameObject>();
    [SerializeField] private int numberOfMirrors = 3;

    public void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }



    // Update is called once per frame
    public IEnumerator SummonMirrors()
    {
        Debug.Log("Phantom Mirror Cast");

        ClearMirrors();

        SpawnMirrors();

        // wait for attacking
        yield return new WaitForSeconds(2.5f);

        // attack
        foreach (var attacker in attackers)
        {
            Debug.Log("Everyone Attack");
            attacker.GetComponent<MirrorAnim>().Cast();
        }

    }

    private void ClearMirrors()
    {
        foreach (var attacker in attackers)
        {
            Destroy(attacker);
        }
        attackers.Clear();
    }

    private void SpawnMirrors()
    {
        var selectedPositions = GetRandomPositions(spawnPositions, numberOfMirrors);

        foreach (var position in selectedPositions)
        {
            GameObject clone = Instantiate(prefab, position.position, Quaternion.identity);
            attackers.Add(clone);
        }

    }

    private Transform[] GetRandomPositions(Transform[] positions, int count)
    {
        // LINQ
        return positions.OrderBy(x => Random.value).Take(count).ToArray();
        // return new Transform[] { positions[0] };
    }

}

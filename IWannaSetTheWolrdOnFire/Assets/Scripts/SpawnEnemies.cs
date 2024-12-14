using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class SpawnEnemies : MonoBehaviour
{
    private GameManager gameManagerScript;

    [SerializeField] private List<GameObject> enemies = new List<GameObject>();

    private void Start()
    {
        gameManagerScript = GetComponent<GameManager>();
    }

    public void StartSpawning()
    {
        foreach (GameObject go in gameManagerScript.grownShrooms)
        {
            GrowShroomScript GSS = go.GetComponent<GrowShroomScript>();
                NavMeshHit hit;
            if (NavMesh.SamplePosition(go.transform.position, out hit, 3, NavMesh.AllAreas))
            {
                // Instantiate the enemy at the valid NavMesh position
                Instantiate(enemies.ElementAt(GSS.shroomType-1), hit.position, Quaternion.identity);
                Destroy(go);
            }
        }
    }
}

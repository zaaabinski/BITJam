using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class SpawnShrooms : MonoBehaviour
{
       
    [SerializeField] private List<GameObject> shrooms =  new List<GameObject>();
    [SerializeField] private List<GameObject> spawnedShrooms = new List<GameObject>();

    [SerializeField] private float spawnRadius = 20f;
    [SerializeField] private int numberOfObjects = 400;
    [SerializeField] private float navMeshSampleDistance = 5f;
    [Header("Spawn Area")]
    public Transform centerPoint; 

    void Start()
    {
        SpawnObjects();
    }

    void SpawnObjects()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            Vector3 randomPosition = GetRandomPointOnNavMesh();
            if (randomPosition != Vector3.zero) 
            {
                int r = Random.Range(0, 3);
                Debug.Log(r);
               GameObject sh = Instantiate(shrooms.ElementAt(r), randomPosition, Quaternion.identity);
                spawnedShrooms.Add(sh);
            }
        }
    }

    public void DeleteShrooms()
    {
        foreach(GameObject sh in spawnedShrooms)
            Destroy(sh);
        spawnedShrooms.Clear();
    }

    Vector3 GetRandomPointOnNavMesh()
    {
        Vector3 randomPoint = centerPoint.position + Random.insideUnitSphere * spawnRadius;
        randomPoint.y = centerPoint.position.y; 

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, navMeshSampleDistance, NavMesh.AllAreas))
        {
            return hit.position; 
        }

        return Vector3.zero; 
    }

}

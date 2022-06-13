using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoidManager : MonoBehaviour
{
    public static BoidManager instance;

    [SerializeField] private GameObject boid;
    [SerializeField] public int amountBoids;
    [SerializeField] private float spawnOffset;

    private List<GameObject> _allBoids;
    private BoxCollider _boxCollider;
    
    private void Awake()
    {
        instance = this;
        _allBoids = new List<GameObject>();
        
        SpawnBoids();
    }

    [ContextMenu("Spawn Boids")]
    private void SpawnBoids()
    {
        for (var i = 0; i < amountBoids; i++)
        {
            var randomPos = new Vector3(Random.Range(-spawnOffset, spawnOffset),
                Random.Range(-spawnOffset, spawnOffset), Random.Range(-spawnOffset, spawnOffset));
            
            _allBoids.Add(Instantiate(boid, randomPos, Quaternion.identity));
        }
    }
    
    [ContextMenu("Destroy Boids")]
    private void DestroyBoids()
    {
        foreach (var boid in _allBoids)
        {
            Destroy(boid);
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boid : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float alignmentMatchingFactor = 0.05f;
    [SerializeField] private float separationMatchingFactor = 0.05f;
    [SerializeField] private float cohesionMatchingFactor = 0.05f;
    [SerializeField] private float maxRandomVelocity = 5f;
    [SerializeField] private float localRadius;
    [SerializeField] private LayerMask layerMask;
    public Rigidbody rb;

    private BoundingBox _boundingBox;
    
    private void OnEnable()
    {
        _boundingBox = FindObjectOfType<BoundingBox>();
        
        rb.velocity = new Vector3(Random.Range(-maxRandomVelocity, maxRandomVelocity), Random.Range(-maxRandomVelocity, maxRandomVelocity),
            Random.Range(-maxRandomVelocity, maxRandomVelocity));
    }

    private void FixedUpdate()
    {
        DetectLocalNeighbours(out var rigidbodies);
        Align(rigidbodies);
        Cohere(rigidbodies);
        Separate(rigidbodies);
        BoundingBox();
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(rb.velocity);
    }

    private void DetectLocalNeighbours(out List<Rigidbody> rigidbodies)
    {
        var neighbours = Physics.OverlapSphere(transform.position, localRadius, layerMask);
        var neighborRbs = new List<Rigidbody>();

        foreach (var neighbour in neighbours)
        {
            if (neighbour == this.GetComponent<Collider>())
                continue;
            if (neighbour.TryGetComponent(out Rigidbody rigidBody))
                neighborRbs.Add(rigidBody);
            else
                Debug.LogWarningFormat("No Rigidbody found on boid: {0}!", neighbour.name);
        }
        rigidbodies = neighborRbs;
    }
    
    private void Align(IReadOnlyCollection<Rigidbody> boids)
    {
        if (boids.Count == 0) return;

        var averageVelocity = new Vector3();
        averageVelocity = boids.Aggregate(averageVelocity, (current, boid) => current + boid.velocity);
        averageVelocity /= boids.Count;

        var velocity = rb.velocity;
        velocity += (averageVelocity - velocity) * alignmentMatchingFactor;
        rb.velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        rb.AddForce(velocity, ForceMode.Acceleration);
    }

    private void Cohere(IReadOnlyCollection<Rigidbody> boids)
    {
        if (boids.Count == 0) return;

        var averagePosition = new Vector3();
        averagePosition = boids.Aggregate(averagePosition, (current, boid) => current + boid.transform.position);
        averagePosition /= boids.Count;

        var velocity = rb.velocity;
        velocity += (averagePosition - transform.position) * cohesionMatchingFactor;
        rb.velocity = Vector3.ClampMagnitude(velocity,maxSpeed);
        rb.AddForce(velocity, ForceMode.Acceleration);
    }

    private void Separate(IReadOnlyCollection<Rigidbody> boids)
    {
        if (boids.Count == 0) return;

        var direction = new Vector3();
        foreach (var boid in boids)
        {
            var position = transform.position;
            var boidPosition = boid.transform.position;
            var distance = Vector3.Distance(position, boidPosition);
            var difference = (position - boidPosition) / distance;
            
            direction += difference;
            
            if (distance == 0)
            {
                Debug.LogError("Distance is 0");
                return;
            }
            
        }
        
        direction /= boids.Count;

        var velocity = rb.velocity;
        velocity += direction * separationMatchingFactor;
        rb.velocity = Vector3.ClampMagnitude(velocity,maxSpeed);
        rb.AddForce(velocity, ForceMode.Acceleration);
    }

    
    private void BoundingBox()
    {
        var scale = _boundingBox.scale;
        var position = transform.position;
        if (Mathf.Abs(transform.position.x) > scale)
            transform.position = new Vector3(position.x * -1, position.y, position.z);
        if (Mathf.Abs(transform.position.y) > scale)
            transform.position = new Vector3(position.x, position.y * -1, position.z);
        if (Mathf.Abs(transform.position.z) > scale)
            transform.position = new Vector3(position.x, position.y, position.z * -1);
    }
}

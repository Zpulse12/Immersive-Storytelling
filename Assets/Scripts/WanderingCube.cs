using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderingCube : MonoBehaviour
{
    public Transform player;
    public float safeDistance = 5f;
    public float stopDistance = 7f;       // Afstand waarop de kubus stopt met wegrennen
    public float moveSpeed = 3.5f;
    public float fleeSpeed = 6f;
    public float fleeAcceleration = 20f;
    public float checkRadius = 10f;

    private NavMeshAgent agent;
    private bool isMovingAway = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            return;
        }

        if (player == null)
        {
        }

        agent.speed = moveSpeed;
        agent.acceleration = 8f;
    }

    void Update()
    {
        if (agent == null || player == null)
        {
            return;
        }

        Vector3 playerPosition = player.position;
        Vector3 cubePosition = transform.position;
        float distanceToPlayer = Vector3.Distance(cubePosition, playerPosition);

        Debug.Log($"{gameObject.name}: Spelerpositie: {playerPosition}, Kubuspositie: {cubePosition}");
        Debug.Log($"{gameObject.name}: Afstand tot speler: {distanceToPlayer}");

        if (distanceToPlayer < safeDistance && !isMovingAway)
        {
            MoveAwayFromPlayer();
        }
        else if (isMovingAway && distanceToPlayer > stopDistance)
        {
            Debug.Log($"{gameObject.name}: Veilige afstand bereikt. Stoppen.");
            StopMoving();
        }
    }



    private void MoveAwayFromPlayer()
    {
        isMovingAway = true;
        agent.ResetPath();
        agent.speed = fleeSpeed;
        agent.acceleration = fleeAcceleration;

        Vector3 fleeDirection = (transform.position - player.position).normalized;
        Vector3 fleePosition = transform.position + fleeDirection * safeDistance;

        Debug.Log($"{gameObject.name}: MoveAwayFromPlayer gestart, probeer weg te bewegen!");

        Debug.Log($"FleeDirection: {fleeDirection}, FleePosition: {fleePosition}");

        if (NavMesh.SamplePosition(fleePosition, out NavMeshHit hit, checkRadius * 2, NavMesh.AllAreas))
        {
            Debug.Log($"NavMesh Hit Position: {hit.position}");
            agent.SetDestination(hit.position);
            Debug.Log($"{gameObject.name}: Nieuwe bestemming ingesteld naar {hit.position}");
        }
        else
        {
            Debug.LogWarning("Geen valide NavMesh-positie gevonden!");
        }
    }




    private void StopMoving()
    {
        agent.ResetPath();
        agent.speed = moveSpeed;
        isMovingAway = false;
        Debug.Log($"{gameObject.name}: Gestopt met bewegen.");
    }
}

using UnityEngine;

public class ChasePlayer : MonoBehaviour
{
    private Transform player;
    private float moveSpeed;
    private float despawnDistance;

    public void Initialize(Transform playerTransform, float speed, float destroyDistance, float radius)
    {
        player = playerTransform;
        moveSpeed = speed;
        despawnDistance = destroyDistance;
    }

    void Update()
    {
        if (player == null) return;

        // Bereken richting naar speler
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        directionToPlayer.y = 0; // Behoud huidige hoogte

        // Beweeg naar speler
        transform.position += directionToPlayer * moveSpeed * Time.deltaTime;
        
        // Kijk naar speler
        transform.LookAt(player);

        // Check voor despawn
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= despawnDistance)
        {
            Destroy(gameObject);
        }
    }
} 
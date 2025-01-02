using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [Header("NPC Settings")]
    public GameObject[] npcPrefabs;
    public Transform player;

    [Header("Spawn Settings")]
    public int numberOfNPCs = 8;
    public float spawnRadius = 15f;
    public float moveSpeed = 3f;
    public float stopWalkingDistance = 5f;
    public int requiredSpotlights = 4;

    [Header("Animation")]
    public RuntimeAnimatorController walkController;
    public RuntimeAnimatorController idleController;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player reference is missing!");
            return;
        }
    }

    void Update()
    {
        var gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            Debug.LogError("GameManager instance is missing!");
            return;
        }

        if (gameManager.SpotlightsVisited >= requiredSpotlights)
        {
            SpawnNPCs();
            enabled = false; // Disable script after spawning NPCs
        }
    }

    private void SpawnNPCs()
    {
        if (npcPrefabs == null || npcPrefabs.Length == 0)
        {
            Debug.LogError("NPC prefabs are not assigned!");
            return;
        }

        float angleStep = 360f / numberOfNPCs;
        for (int i = 0; i < numberOfNPCs; i++)
        {
            Vector3 spawnPosition = CalculateSpawnPosition(i * angleStep);
            GameObject npc = InstantiateRandomNPC(spawnPosition);
            ConfigureNPC(npc);
        }
    }

    private Vector3 CalculateSpawnPosition(float angle)
    {
        Vector3 offset = Quaternion.Euler(0, angle, 0) * Vector3.forward * spawnRadius;
        Vector3 spawnPosition = player.position + offset;

        if (Physics.Raycast(spawnPosition + Vector3.up * 10f, Vector3.down, out RaycastHit hit))
        {
            spawnPosition.y = hit.point.y;
        }

        return spawnPosition;
    }

    private GameObject InstantiateRandomNPC(Vector3 position)
    {
        int randomIndex = Random.Range(0, npcPrefabs.Length);
        GameObject npc = Instantiate(npcPrefabs[randomIndex], position, Quaternion.identity);
        
        npc.transform.localScale *= 1.5f;

        return npc;
    }

    private void ConfigureNPC(GameObject npc)
    {
        // Add ChasePlayer behavior
        var chaseComponent = npc.AddComponent<ChasePlayer>();
        chaseComponent.Initialize(player, moveSpeed, idleController);

        // Assign walk animation
        if (npc.TryGetComponent(out Animator animator) && walkController != null)
        {
            animator.runtimeAnimatorController = walkController;
        }

        // Add trigger collider
        var triggerCollider = npc.AddComponent<SphereCollider>();
        triggerCollider.isTrigger = true;
        triggerCollider.radius = stopWalkingDistance;
        
        // Add Rigidbody for collision detection
        var npcRigidBody = npc.AddComponent<Rigidbody>();
        npcRigidBody.isKinematic = true;
    }
}

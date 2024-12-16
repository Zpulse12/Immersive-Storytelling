using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject[] npcPrefabs;
    public Transform player;
    
    [Header("Spawn Settings")]
    public int numberOfNPCs = 8;
    public float spawnRadius = 15f;    
    public float moveSpeed = 3f;       
    public float despawnDistance = 2f;  
    public int requiredSpotlights = 4;

    [Header("Animation")]
    public RuntimeAnimatorController walkController;  // Alleen walk animatie controller

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Wijs de Player toe in de inspector!");
            return;
        }
        Debug.Log("NPCSpawner gestart, wacht op " + requiredSpotlights + " spotlights");
    }

    void Update()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("Geen GameManager gevonden!");
            return;
        }

        Debug.Log($"Huidige spotlights bezocht: {GameManager.Instance.SpotlightsVisited} / {requiredSpotlights}");
        
        if (GameManager.Instance.SpotlightsVisited >= requiredSpotlights)
        {
            Debug.Log("Genoeg spotlights bezocht, spawning NPCs...");
            SpawnNPCsInCircle();
            enabled = false; 
        }
    }

    void SpawnNPCsInCircle()
    {
        if (npcPrefabs == null || npcPrefabs.Length == 0)
        {
            Debug.LogError("Geen NPC prefabs toegewezen!");
            return;
        }

        Debug.Log($"Spawning {numberOfNPCs} NPCs in cirkel...");
        float angleStep = 360f / numberOfNPCs;
        
        for (int i = 0; i < numberOfNPCs; i++)
        {
            float angle = i * angleStep;
            Vector3 spawnPosition = player.position + Quaternion.Euler(0, angle, 0) * Vector3.forward * spawnRadius;
            
            RaycastHit hit;
            if (Physics.Raycast(spawnPosition + Vector3.up * 10f, Vector3.down, out hit))
            {
                spawnPosition.y = hit.point.y;
            }
            else
            {
                spawnPosition.y = 0f;
            }

            GameObject npc = Instantiate(npcPrefabs[Random.Range(0, npcPrefabs.Length)], spawnPosition, Quaternion.identity);
            
            ChasePlayer chaseComponent = npc.AddComponent<ChasePlayer>();
            chaseComponent.Initialize(player, moveSpeed, despawnDistance, spawnRadius);

            Animator animator = npc.GetComponent<Animator>();
            if (animator != null && walkController != null)
            {
                animator.runtimeAnimatorController = walkController;
            }
        }
    }
} 
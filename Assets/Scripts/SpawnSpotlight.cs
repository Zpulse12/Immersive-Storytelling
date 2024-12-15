using UnityEngine;
using System.Collections;

public class SpawnSpotlight : MonoBehaviour
{
    public GameObject[] npcPrefabs; // Array van verschillende NPC prefabs
    private int currentPrefabIndex = 0; // Bijhouden welke prefab als volgende komt

    public Light spotlightPrefab;

    [Header("Spawn Settings")]
    public bool spawnAroundPlayer = true;
    public float spawnAreaWidth = 10f;
    public float spawnAreaLength = 10f;
    public float spotlightHeight = 5f;
    public bool autoRespawn = true;
    public float respawnDelay = 2f;
    public float characterHeight = 0f;
    public Vector3 characterScale = Vector3.one;

    [Header("Spotlight Settings")]
    public float spotlightIntensity = 8f;
    public float spotlightRange = 20f;
    public float spotAngle = 30f;
    public Color spotlightColor = Color.white;

    [Header("Cylinder Settings")]
    public float cylinderHeight = 2f;
    public float cylinderRadius = 0.5f;
    public Material cylinderMaterial; // Use this to assign a material

    [Header("Despawn Settings")]
    public float despawnDistance = 2f;
    public float despawnDelay = 5f;
    public Transform player;

    void Start()
    {
        Debug.Log("Script is gestart");

        if (player == null)
        {
            Debug.LogError("Player reference is niet ingesteld! Sleep je Player object naar het Player veld in de Inspector.");
            return;
        }

        SpawnSpotlightWithCharacter();
    }

    void SpawnSpotlightWithCharacter()
    {
        Debug.Log("SpawnSpotlightWithCharacter aangeroepen");

        float randomX = Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2);
        float randomZ = Random.Range(-spawnAreaLength / 2, spawnAreaLength / 2);

        Vector3 basePosition = spawnAroundPlayer ? player.position : transform.position;
        Vector3 spawnPosition = basePosition + new Vector3(randomX, characterHeight, randomZ);

        GameObject character = Instantiate(npcPrefabs[currentPrefabIndex], spawnPosition, Quaternion.identity);
        character.transform.localScale = characterScale;

        Debug.Log($"Spawned prefab {currentPrefabIndex}");
        
        currentPrefabIndex = (currentPrefabIndex + 1) % npcPrefabs.Length;

        // Spawn spotlight
        Light spotlight = Instantiate(spotlightPrefab);
        spotlight.intensity = spotlightIntensity;
        spotlight.range = spotlightRange;
        spotlight.spotAngle = spotAngle;
        spotlight.color = spotlightColor;
        spotlight.shadows = LightShadows.Soft;
        spotlight.transform.position = spawnPosition + new Vector3(0, spotlightHeight, 0);
        spotlight.transform.rotation = Quaternion.Euler(90, 0, 0);

        // Create container
        GameObject container = new GameObject("SpotlightGroup");
        character.transform.parent = container.transform;
        spotlight.transform.parent = container.transform;

        // Spawn cylinder
        GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        cylinder.transform.position = spawnPosition + new Vector3(0, cylinderHeight / 2, 0);
        cylinder.transform.localScale = new Vector3(cylinderRadius * 2, cylinderHeight / 2, cylinderRadius * 2);
        Destroy(cylinder.GetComponent<Collider>());

        // Apply material to cylinder
        if (cylinderMaterial != null)
        {
            Renderer cylinderRenderer = cylinder.GetComponent<Renderer>();
            cylinderRenderer.material = cylinderMaterial;
            cylinderRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            cylinderRenderer.receiveShadows = false;
        }

        cylinder.transform.parent = container.transform;

        container.transform.position = spawnPosition;

        // Start despawn logic
        StartCoroutine(CheckForDespawn(container));
    }

    IEnumerator CheckForDespawn(GameObject container)
    {
        if (player == null)
        {
            Debug.LogError("Player reference is niet ingesteld!");
            yield break;
        }

        Transform characterTransform = container.transform.GetChild(0);
        bool startedDespawnTimer = false;
        float despawnTimer = 0;

        while (container != null)
        {
            float distance = Vector3.Distance(player.position, characterTransform.position);

            if (distance <= despawnDistance)
            {
                if (!startedDespawnTimer)
                {
                    startedDespawnTimer = true;
                    Debug.Log("Speler in de buurt van character, start despawn timer");
                }

                despawnTimer += Time.deltaTime;
                Debug.Log($"Despawn timer: {despawnTimer} / {despawnDelay}");

                if (despawnTimer >= despawnDelay)
                {
                    Debug.Log("Despawning spotlight, character en cylinder");
                    
                    GameManager.Instance.IncrementSpotlightsVisited();
                    
                    NPCBehavior npcBehavior = characterTransform.GetComponent<NPCBehavior>();
                    if (npcBehavior != null)
                    {
                        Debug.Log("NPCBehavior gevonden, EnableWalking aanroepen");
                        characterTransform.parent = null;
                        npcBehavior.EnableWalking();
                    }
                    
                    Destroy(container);
                    if (autoRespawn)
                    {
                        StartCoroutine(RespawnCoroutine());
                    }
                    break;
                }
            }
            yield return null;
        }
    }

    IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnSpotlightWithCharacter();
    }

    public void SpawnNPC(Vector3 position)
    {
        if (npcPrefabs == null || npcPrefabs.Length == 0)
        {
            Debug.LogError("Geen NPC prefabs toegewezen aan de SpawnSpotlight!");
            return;
        }

        // Spawn de huidige prefab
        GameObject spawnedNPC = Instantiate(npcPrefabs[currentPrefabIndex], position, Quaternion.identity);

        // Update de index voor de volgende spawn
        currentPrefabIndex = (currentPrefabIndex + 1) % npcPrefabs.Length;
    }
}

using UnityEngine;
using System.Collections;

public class SpawnSpotlight : MonoBehaviour
{
    public GameObject characterPrefab;
    public Light spotlightPrefab;

    [Header("Spawn Settings")]
    public float spawnAreaWidth = 10f;
    public float spawnAreaLength = 10f;
    public float spotlightHeight = 5f;
    public bool autoRespawn = true;
    public float respawnDelay = 2f;
    public float characterHeight = 0f;

    [Header("Spotlight Settings")]
    public float spotlightIntensity = 8f;
    public float spotlightRange = 20f;
    public float spotAngle = 30f;
    public Color spotlightColor = Color.white;

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
        Vector3 spawnPosition = new Vector3(randomX, characterHeight, randomZ);

        GameObject character = Instantiate(characterPrefab, spawnPosition, Quaternion.identity);
        Light spotlight = Instantiate(spotlightPrefab);
        
        spotlight.intensity = spotlightIntensity;
        spotlight.range = spotlightRange;
        spotlight.spotAngle = spotAngle;
        spotlight.color = spotlightColor;
        spotlight.shadows = LightShadows.Soft;
        
        spotlight.transform.position = spawnPosition + new Vector3(0, spotlightHeight, 0);
        spotlight.transform.rotation = Quaternion.Euler(90, 0, 0);

        GameObject container = new GameObject("SpotlightGroup");
        character.transform.parent = container.transform;
        spotlight.transform.parent = container.transform;
        container.transform.position = spawnPosition; 

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
            
            Debug.Log($"Afstand tot character: {distance}, Despawn afstand: {despawnDistance}");

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
                    Debug.Log("Despawning spotlight en character");
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Spatie ingedrukt");
            SpawnSpotlightWithCharacter();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("T ingedrukt");
            SpawnSpotlightWithCharacter();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Muis geklikt");
            SpawnSpotlightWithCharacter();
        }
    }
}

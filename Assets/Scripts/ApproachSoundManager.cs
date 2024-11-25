using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class ApproachSoundManager : MonoBehaviour
{
    public AudioClip[] soundClips;
    public float maxVolume = 1.0f; 
    public float minDistance = 5f;
    public float maxDistance = 10f;
    public float timeMultiplier = 0.5f;
    public float minTimeBetweenSounds = 1f; 
    public float soundRadius = 10f;
    public int poolSize = 10;

    private Transform player;
    private GameObject[] personObjects;
    private float nextSoundTime = 0f;
    private Queue<GameObject> soundPool;

    void Start()
    {
        if (soundClips.Length == 0)
        {
            Debug.LogError("Please assign at least one sound clip in the Inspector.");
            return;
        }

        player = Camera.main.transform;
        personObjects = GameObject.FindGameObjectsWithTag("Human");

        if (personObjects.Length == 0)
        {
            Debug.LogWarning("No GameObjects with the 'Human' tag found in the scene.");
        }

        InitializeSoundPool();
    }

    void Update()
    {
        if (personObjects.Length == 0) return;

        float closestPersonDistance = float.MaxValue;
        
        foreach (var person in personObjects)
        {
            float distanceToPerson = Vector3.Distance(player.position, person.transform.position);
            closestPersonDistance = Mathf.Min(closestPersonDistance, distanceToPerson);
        }

        float adjustedTimeBetweenSounds = Mathf.Lerp(minTimeBetweenSounds, timeMultiplier, Mathf.InverseLerp(minDistance, maxDistance, closestPersonDistance));
        AdjustVolume(closestPersonDistance);

        if (Time.time >= nextSoundTime)
        {
            PlayRandomSounds();
            nextSoundTime = Time.time + adjustedTimeBetweenSounds;
        }
    }

    void AdjustVolume(float distanceToPerson)
    {
        float volume = Mathf.Lerp(maxVolume, 0, Mathf.InverseLerp(minDistance, maxDistance, distanceToPerson));
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (var audioSource in audioSources)
        {
            audioSource.volume = volume;
        }
    }

    void InitializeSoundPool()
    {
        soundPool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            // Create a sound object and deactivate it
            GameObject soundObject = new GameObject("PooledSound");
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1.0f; // Full 3D sound
            audioSource.dopplerLevel = 0.0f;
            audioSource.minDistance = minDistance;
            audioSource.maxDistance = maxDistance;
            soundObject.SetActive(false);
            
            soundPool.Enqueue(soundObject);
        }
    }

    void PlayRandomSounds()
    {
        for (int i = 0; i < 3; i++)
        {
            if (soundPool.Count > 0)
            {
                GameObject soundObject = soundPool.Dequeue();
                AudioSource audioSource = soundObject.GetComponent<AudioSource>();

                int randomIndex = Random.Range(0, soundClips.Length);
                audioSource.clip = soundClips[randomIndex];
                Vector3 randomPosition = player.position + Random.insideUnitSphere * soundRadius;
                soundObject.transform.position = randomPosition;
                soundObject.SetActive(true);

                audioSource.Play();

                StartCoroutine(ReturnToPoolAfterPlay(soundObject, audioSource.clip.length));
            }
            else
            {
                Debug.LogWarning("Sound pool is empty!");
            }
        }
    }

    IEnumerator ReturnToPoolAfterPlay(GameObject soundObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        soundObject.SetActive(false);
        soundPool.Enqueue(soundObject);
    }
}

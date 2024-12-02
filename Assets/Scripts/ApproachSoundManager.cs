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
    public int initialPoolSize = 10;
    public int maxPoolSize = 50; // Limit to avoid excessive expansion

    private Transform player;
    private GameObject[] personObjects;
    private float nextSoundTime = 0f;
    private Queue<GameObject> soundPool;
    private int currentPoolSize;
    private AudioSource permanentAudioSource;  // Reference to PermanentAudioManager's AudioSource

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

        InitializeSoundPool(initialPoolSize);
        permanentAudioSource = FindObjectOfType<PermanentAudioManager>().GetComponent<AudioSource>(); // Get the PermanentAudioManager's AudioSource
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

        AdjustPoolSize(closestPersonDistance);

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
            // Ensure we're only adjusting audio sources that belong to the ApproachSoundManager
            if (audioSource != permanentAudioSource)  // Exclude PermanentAudioManager's AudioSource
            {
                audioSource.volume = volume;
            }
        }
    }

    void InitializeSoundPool(int size)
    {
        soundPool = new Queue<GameObject>();
        currentPoolSize = size;

        for (int i = 0; i < size; i++)
        {
            CreateSoundObject();
        }
    }

    void AdjustPoolSize(float closestDistance)
    {
        float normalizedDistance = 1 - Mathf.InverseLerp(minDistance, maxDistance, closestDistance);
        float interpolatedPoolSize = Mathf.Lerp(initialPoolSize, maxPoolSize, normalizedDistance);
        int targetPoolSize = Mathf.RoundToInt(interpolatedPoolSize);

        if (targetPoolSize > currentPoolSize)
        {
            for (int i = currentPoolSize; i < targetPoolSize; i++)
            {
                CreateSoundObject();
            }
        }

        currentPoolSize = targetPoolSize;
    }

    void CreateSoundObject()
    {
        GameObject soundObject = new GameObject("PooledSound");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1.0f;
        audioSource.dopplerLevel = 0.0f;
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;
        soundObject.SetActive(false);
        soundPool.Enqueue(soundObject);
    }

    void PlayRandomSounds()
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

            StartCoroutine(ReturnToPoolAfterPlay(soundObject, audioSource));
        }
        else
        {
            Debug.LogWarning("Sound pool is empty!");
        }
    }

    IEnumerator ReturnToPoolAfterPlay(GameObject soundObject, AudioSource audioSource)
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        soundObject.SetActive(false);
        soundPool.Enqueue(soundObject);
    }
}

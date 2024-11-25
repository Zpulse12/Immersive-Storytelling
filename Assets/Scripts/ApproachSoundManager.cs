using UnityEngine;

public class ApproachSoundManager : MonoBehaviour
{
    public AudioClip[] soundClips;    // Array to store the sound clips (any number)
    public float maxVolume = 1.0f;    // Maximum volume of the sound
    public float minDistance = 5f;    // Minimum distance where the sound is full volume
    public float maxDistance = 10f;   // Maximum distance for the sound to fade
    public float timeMultiplier = 0.5f; // Multiplier to adjust time between sounds
    public float minTimeBetweenSounds = 1f; // Minimum time interval between sounds
    public float soundRadius = 10f;   // Radius around the player to spawn sounds

    private Transform player;
    private GameObject[] personObjects; // Array to hold all the cubes tagged as "Person"
    private float nextSoundTime = 0f; // Time when the next sound can be played

    void Start()
    {
        // Ensure there are at least one sound clip
        if (soundClips.Length == 0)
        {
            Debug.LogError("Please assign at least one sound clip in the Inspector.");
            return;
        }

        if (soundClips.Length == 1)
        {
            Debug.LogWarning("Only one sound clip is assigned. Consider adding more sounds.");
        }

        // Get player (camera in most VR setups)
        player = Camera.main.transform; 

        // Find all objects with the "Person" tag
        personObjects = GameObject.FindGameObjectsWithTag("Human");

        if (personObjects.Length == 0)
        {
            Debug.LogWarning("No GameObjects with the 'Human' tag found in the scene.");
        }
    }

    void Update()
    {
        if (personObjects.Length == 0) return;

        // Calculate the distance to all "Person" objects (cubes) and adjust the sound parameters accordingly
        float closestPersonDistance = float.MaxValue;

        // Calculate the influence of each "Person" object on the sound based on distance
        foreach (var person in personObjects)
        {
            float distanceToPerson = Vector3.Distance(player.position, person.transform.position);
            closestPersonDistance = Mathf.Min(closestPersonDistance, distanceToPerson);
        }

        // Adjust the time interval between sounds based on distance, but don't allow it to go below the minimum
        float adjustedTimeBetweenSounds = Mathf.Lerp(minTimeBetweenSounds, timeMultiplier, Mathf.InverseLerp(minDistance, maxDistance, closestPersonDistance));

        // Adjust volume based on proximity to the closest "Person" object
        AdjustVolume(closestPersonDistance);

        // Play sounds if it's time to play
        if (Time.time >= nextSoundTime)
        {
            PlayRandomSounds();
            nextSoundTime = Time.time + adjustedTimeBetweenSounds; // Increase frequency as player gets closer
        }
    }

    void AdjustVolume(float distanceToPerson)
    {
        // Adjust volume based on distance to the closest "Person" object (closer = louder)
        float volume = Mathf.Lerp(maxVolume, 0, Mathf.InverseLerp(minDistance, maxDistance, distanceToPerson));

        // Adjust the global volume based on proximity to the "Person" objects
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>(); // Finding all audio sources in the scene
        foreach (var audioSource in audioSources)
        {
            audioSource.volume = volume;
        }
    }

    void PlayRandomSounds()
    {
        // Create 3 AudioSource objects
        for (int i = 0; i < 3; i++)
        {
            // Randomly pick a sound from the soundClips array
            int randomIndex = Random.Range(0, soundClips.Length);

            // Generate a random position around the player within the specified radius
            Vector3 randomPosition = player.position + Random.insideUnitSphere * soundRadius;

            // Create the AudioSource object and assign it the random sound
            CreateAndPlaySoundAtPosition(randomPosition, soundClips[randomIndex]);
        }
    }

    void CreateAndPlaySoundAtPosition(Vector3 position, AudioClip sound)
    {
        // Create a new GameObject to attach the AudioSource to
        GameObject soundObject = new GameObject("ApproachSound");
        soundObject.transform.position = position;

        // Add AudioSource to the new GameObject and set up the sound
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = sound;
        audioSource.loop = false;

        // Set AudioSource properties for 3D sound:
        audioSource.spatialBlend = 1.0f;  // 1 for full 3D sound (2D would be 0)
        audioSource.dopplerLevel = 0.0f;  // Optional: set to 0 to disable Doppler effect

        // Set min and max distance for volume attenuation
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;

        // Calculate the distance between the player and the sound origin
        float distance = Vector3.Distance(player.position, position);

        // Adjust volume based on distance to the player
        float volume = Mathf.Lerp(maxVolume, 0, (distance - minDistance) / (maxDistance - minDistance));
        audioSource.volume = volume;

        // Play the selected sound
        audioSource.Play();

        // Destroy the GameObject after the sound finishes
        Destroy(soundObject, audioSource.clip.length);
    }
}

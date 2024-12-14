using UnityEngine;
using System.Collections.Generic;

public class ProximityAudio : MonoBehaviour
{
    [Header("Player Reference")]
    public Transform player;

    [Header("Audio Settings")]
    public SharedAudioClips sharedAudioClips;
    private AudioClip[] audioClips;
    public float maxDistance = 20f; // Maximum distance at which sounds can be heard
    public float minDistance = 2f; // Distance at which sounds are at maximum volume
    public float maxVolume = 1.0f; // Maximum volume for the audio clips
    public int maxAudioSources = 4; // Maximum number of audio sources active

    private List<AudioSource> audioSourcePool;

    void Start()
    {
        // Initialize the object pool
        audioClips = sharedAudioClips.audioClips;
        audioSourcePool = new List<AudioSource>();
        for (int i = 0; i < maxAudioSources; i++)
        {
            GameObject obj = new GameObject($"AudioSource_{i}");
            obj.transform.parent = transform;
            AudioSource source = obj.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.loop = false;
            source.volume = maxVolume;
            obj.SetActive(false);
            audioSourcePool.Add(source);
        }
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // Calculate the volume multiplier and active audio sources based on distance
        float normalizedDistance = Mathf.Clamp01((maxDistance - distanceToPlayer) / (maxDistance - minDistance));
        int activeSources = Mathf.RoundToInt(normalizedDistance * maxAudioSources);

        // Enable and adjust audio sources
        for (int i = 0; i < audioSourcePool.Count; i++)
        {
            if (i < activeSources)
            {
                if (!audioSourcePool[i].isPlaying)
                {
                    audioSourcePool[i].clip = GetRandomClip();
                    audioSourcePool[i].Play();
                }
                audioSourcePool[i].volume = normalizedDistance;
                audioSourcePool[i].transform.position = transform.position; // Ensure sources stay on the object
                audioSourcePool[i].gameObject.SetActive(true);
            }
            else
            {
                if (audioSourcePool[i].isPlaying)
                {
                    audioSourcePool[i].Stop();
                }
                audioSourcePool[i].gameObject.SetActive(false);
            }
        }
    }

    private AudioClip GetRandomClip()
    {
        if (audioClips.Length > 0)
        {
            int index = Random.Range(0, audioClips.Length);
            return audioClips[index];
        }
        return null;
    }
}

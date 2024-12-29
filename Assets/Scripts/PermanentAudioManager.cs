using UnityEngine;

public class PermanentAudioManager : MonoBehaviour
{
    [Header("Shared Audio Settings")]
    public SharedAudioClips sharedAudioClipList;
    public float maxVolume = 1.0f;

    private AudioSource audioSource;

    void Start()
    {
        // Create and configure an AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.volume = maxVolume;

        // Start playing random clips continuously
        PlayRandomClip();
    }

    void PlayRandomClip()
    {
        if (sharedAudioClipList.audioClips.Length > 0)
        {
            audioSource.clip = GetRandomClip();
            audioSource.Play();

            // Schedule the next clip to play after the current one finishes
            Invoke(nameof(PlayRandomClip), audioSource.clip.length);
        }
    }

    private AudioClip GetRandomClip()
    {
        int index = Random.Range(0, sharedAudioClipList.audioClips.Length);
        return sharedAudioClipList.audioClips[index];
    }
}
using UnityEngine;
using System.Collections;

public class PermanentAudioManager : MonoBehaviour
{
    public ApproachSoundManager approachSoundManager;
    public float permanentSoundVolume = 0.75f;
    public float delayBetweenClips = 0.5f;  // Adjustable delay between clips

    private AudioSource permanentAudioSource;

    void Start()
    {
        if (approachSoundManager.soundClips.Length == 0)
        {
            Debug.LogError("No sound clips found in ApproachSoundManager!");
            return; // Exit if no clips are available.
        }

        // Create an AudioSource for the permanent sound
        permanentAudioSource = gameObject.AddComponent<AudioSource>();
        permanentAudioSource.volume = permanentSoundVolume;

        // Set to 2D sound (not affected by distance)
        permanentAudioSource.spatialBlend = 0;

        // Start the loop of playing sounds
        StartCoroutine(PlayRandomClipLoop());
    }

    IEnumerator PlayRandomClipLoop()
    {
        while (true) // Infinite loop to continuously play new sounds
        {
            permanentAudioSource.clip = GetRandomClip();
            permanentAudioSource.Play();
            Debug.Log($"Permanent sound {permanentAudioSource.clip.name} is now playing.");

            // Wait until the clip finishes playing, plus the additional delay
            yield return new WaitForSeconds(permanentAudioSource.clip.length + delayBetweenClips);
        }
    }

    AudioClip GetRandomClip()
    {
        int randomIndex = Random.Range(0, approachSoundManager.soundClips.Length);
        return approachSoundManager.soundClips[randomIndex];
    }
}
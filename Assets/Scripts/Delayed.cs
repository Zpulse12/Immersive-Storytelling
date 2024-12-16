using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delayed : MonoBehaviour
{
    public Light lightSource1;
    public Light lightSource2;
    public AudioSource audioSource;

    void Start()
    {
        StartCoroutine(ActivateLightsAndMusic());
    }

    private IEnumerator ActivateLightsAndMusic()
    {
        yield return new WaitForSeconds(5f);

        if (lightSource1 != null)
        {
            lightSource1.enabled = true;
        }

        if (lightSource2 != null)
        {
            lightSource2.enabled = true;
        }

        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}

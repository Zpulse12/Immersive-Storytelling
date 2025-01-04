using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delayed : MonoBehaviour
{
    public Light lightSource1;
    public Light lightSource2;
    public AudioSource audioSource;
    public GameObject mirrorCanvas;

    void Start()
    {
        StartCoroutine(ActivateLightsAndMusic());
    }

    private IEnumerator ActivateLightsAndMusic()
    {
        yield return new WaitForSeconds(5f);

        if (lightSource1)lightSource1.enabled = true;
        if (lightSource2)lightSource2.enabled = true;
        if (audioSource)audioSource.Play();
        if(mirrorCanvas) mirrorCanvas.SetActive(true);
    }
}

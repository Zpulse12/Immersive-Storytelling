using UnityEngine;

public class LightController : MonoBehaviour
{
    public Light pointLight;
    public AudioSource audioSource;
    public float delay = 5f;

    private void Start()
    {
        if (pointLight != null)
        {
            pointLight.enabled = false;
        }

        if (audioSource != null)
        {
            audioSource.Stop();
        }

        StartCoroutine(ActivateLightAndMusic());
    }

    private System.Collections.IEnumerator ActivateLightAndMusic()
    {
        yield return new WaitForSeconds(delay);

        if (pointLight != null)
        {
            pointLight.enabled = true;
        }

        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}

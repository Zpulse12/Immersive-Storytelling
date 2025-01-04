using System.Collections;
using UnityEngine;

public class ActivateAfterDelay : MonoBehaviour
{
    [Tooltip("The GameObject to activate after the delay.")]
    public GameObject targetObject;

    [Tooltip("The delay in seconds before the GameObject is activated.")]
    public float delay = 5f;
    private void Start()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target object not assigned. Please assign a GameObject in the Inspector.");
            return;
        }

        StartCoroutine(ActivateObjectAfterDelay());
    }

    private IEnumerator ActivateObjectAfterDelay()
    {
        yield return new WaitForSeconds(delay);

        targetObject.SetActive(true);
                
        AudioSource audioSource = targetObject.GetComponent<AudioSource>();
        if (audioSource) audioSource.Play();

        Debug.Log($"{targetObject.name} activated after {delay} seconds.");
    }
}
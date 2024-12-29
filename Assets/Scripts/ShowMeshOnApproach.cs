using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShowMeshOnApproach : MonoBehaviour
{
    private List<MeshRenderer> meshRenderers;
    public string fenceTag = "Fence";
    // Start is called before the first frame update
    void Start()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>()
            .Where(r => r.gameObject.CompareTag(fenceTag))
            .ToList();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.enabled = true;
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.enabled = false;
            }
        }
    }
}

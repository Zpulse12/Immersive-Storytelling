using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    void Start()
    {
        GameObject spawnPoint = GameObject.FindWithTag("SpawnPoint");
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.transform.position;
            transform.rotation = spawnPoint.transform.rotation;
        }
        else
        {
            Debug.LogError("No SpawnPoint found in the scene!");
        }
    }
}
using UnityEngine;
using System.Collections;

public class PlayerSpotlight : MonoBehaviour
{
    public Light spotlightPrefab;
    public float spotlightDuration = 3f;
    public float spotlightHeight = 5f;
    public float spotlightIntensity = 8f;
    public float spotAngle = 30f;
    public Color spotlightColor = Color.white;
    private SceneChanger sceneChanger;

    private Light currentSpotlight;
    
    void Start()
    {
        sceneChanger = FindObjectOfType<SceneChanger>();
        if (sceneChanger == null)
        {
            Debug.LogError("SceneChanger object is missing!");
        }
    }

    public void FreezePlayerWithSpotlight()
    {
        StartCoroutine(SpotlightSequence());
    }

    private IEnumerator SpotlightSequence()
    {
        SpawnSpotlight();

        yield return new WaitForSeconds(spotlightDuration);

        if (currentSpotlight != null)
        {
            Destroy(currentSpotlight.gameObject);
        }
    }

    private void SpawnSpotlight()
    {
        if (spotlightPrefab != null)
        {
            Vector3 spotPosition = transform.position + Vector3.up * spotlightHeight;
            currentSpotlight = Instantiate(spotlightPrefab, spotPosition, Quaternion.Euler(90, 0, 0));
            
          currentSpotlight.intensity = spotlightIntensity;
            currentSpotlight.spotAngle = spotAngle;
            currentSpotlight.color = spotlightColor;
        }
    }
    
    private void ToNextScene()
    {
        sceneChanger.LoadNextScene();
    }
} 
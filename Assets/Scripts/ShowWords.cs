using System.Collections;
using TMPro;
using UnityEngine;

public class ShowWords : MonoBehaviour
{
    public Transform vrCamera;
    public float radius = 2f;
    public float speed = 1f;
    public float displayDuration = 5f;
    public float fadeDuration = 2f;

    private TextMeshPro textMesh;
    private float angle = 0f;
    private float timer = 0f;
    private bool isFading = false;

    void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
        if (textMesh == null)
        {
            Debug.LogError("TextMeshPro component niet gevonden!");
            return;
        }
        textMesh.alpha = 1f;
    }

    void Update()
    {
        if (textMesh == null || vrCamera == null) return;

        // Verhoog de hoek voor cirkelvormige beweging
        angle += speed * Time.deltaTime;
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;

        // Positioneer de tekst in een cirkel rondom de speler
        Vector3 newPosition = new Vector3(vrCamera.position.x + x, vrCamera.position.y, vrCamera.position.z + z);
        transform.position = newPosition;

        // Bereken de richting naar de camera
        Vector3 direction = vrCamera.position - transform.position;
        direction.y = 0; // Zorg ervoor dat de tekst horizontaal blijft

        // Pas rotatie toe zodat de tekst correct naar de camera gericht is
        transform.rotation = Quaternion.LookRotation(-direction);

        timer += Time.deltaTime;
        if (timer > displayDuration && !isFading)
        {
            isFading = true;
            StartCoroutine(FadeOutText());
        }
    }

    private IEnumerator FadeOutText()
    {
        float fadeSpeed = 1f / fadeDuration;
        while (textMesh.alpha > 0f)
        {
            textMesh.alpha -= fadeSpeed * Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}

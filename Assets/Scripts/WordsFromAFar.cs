using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class WordsFromAFar : MonoBehaviour
{
    public Transform vrCamera; // Zorg ervoor dat deze naar de hoofd (HMD) camera van de XR Rig verwijst
    public float displayDistance = 5f;
    public float displayDuration = 3f;
    public float fadeDuration = 1f;
    public float delayBetweenMessages = 2f;
    public float minHeight = 1.5f; // Minimale hoogte boven de grond
    public float maxHeight = 3.5f; // Maximale hoogte boven de grond

    private TextMeshPro textMesh;
    private int currentMessageIndex = 0;

    [TextArea]
    public List<string> messages = new List<string>
    {
        "Je bent niet goed genoeg",
        "Niemand vindt je leuk",
        "Je zult falen",
        "Je bent dom",
        "Waarom probeer je het nog?",
        "Je zult nooit slagen",
        "Je bent lelijk",
        "Niemand gelooft in jou"
    };

    void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
        if (textMesh == null)
        {
            Debug.LogError("TextMeshPro component niet gevonden!");
            return;
        }

        textMesh.alpha = 0f;
        StartCoroutine(ShowRotatingMessages());
    }

    private IEnumerator ShowRotatingMessages()
    {
        while (true)
        {
            string currentMessage = messages[currentMessageIndex];
            textMesh.text = currentMessage;

            PositionTextInFrontOfPlayer();
            yield return StartCoroutine(FadeInText());

            yield return new WaitForSeconds(displayDuration);

            yield return StartCoroutine(FadeOutText());

            yield return new WaitForSeconds(delayBetweenMessages);

            currentMessageIndex = (currentMessageIndex + 1) % messages.Count;
        }
    }

    private void PositionTextInFrontOfPlayer()
    {
        // Controleer of de vrCamera correct is ingesteld
        if (vrCamera == null)
        {
            Debug.LogError("vrCamera is niet ingesteld op de XR HMD!");
            return;
        }

        // Bereken de richting waarin de VR-camera kijkt (headset)
        Vector3 forwardDirection = vrCamera.forward;
        forwardDirection.y = 0; // Zorg ervoor dat de tekst niet te hoog of laag is

        // Bereken de spawnpositie recht voor de camera op een bepaalde afstand
        Vector3 spawnPosition = vrCamera.position + forwardDirection.normalized * displayDistance;

        // Voeg een willekeurige hoogte toe
        float randomHeight = Random.Range(minHeight, maxHeight);
        spawnPosition.y = vrCamera.position.y + randomHeight;

        // Stel de positie van het object in
        transform.position = spawnPosition;

        // Zorg ervoor dat de tekst naar de speler gericht is
        transform.LookAt(new Vector3(vrCamera.position.x, transform.position.y, vrCamera.position.z));
    }

    private IEnumerator FadeInText()
    {
        float fadeSpeed = 1f / fadeDuration;
        while (textMesh.alpha < 1f)
        {
            textMesh.alpha += fadeSpeed * Time.deltaTime;
            yield return null;
        }
        textMesh.alpha = 1f;
    }

    private IEnumerator FadeOutText()
    {
        float fadeSpeed = 1f / fadeDuration;
        while (textMesh.alpha > 0f)
        {
            textMesh.alpha -= fadeSpeed * Time.deltaTime;
            yield return null;
        }
        textMesh.alpha = 0f;
    }
}

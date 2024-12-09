using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reality : MonoBehaviour
{
    public Transform player;
    public float interactionDistance = 5f;
    public Text messageText;
    public float messageDisplayTime = 5f;
    public float textDistance = 2f;
    public float textHeight = 1.6f;

    public bool showDebugSphere = true;

    private List<string> realityMessages = new List<string>
    {
        "Mensen letten niet op jou, ze zijn bezig met hun eigen zorgen...",
        "Mensen zien vaak niet wat je doet, ze zijn druk met hun leven...",
        "Mensen beoordelen je niet, ze denken nauwelijks over je na...",
        "Anderen merken je niet eens op, je zorgen zijn waarschijnlijk onnodig...",
        "Mensen zijn te druk met zichzelf om zich met jou bezig te houden..."
    };

    private Coroutine messageCoroutine;
    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player Transform is niet toegewezen!");
            enabled = false;
            return;
        }

        if (messageText == null)
        {
            Debug.LogError("Message Text is niet toegewezen! Voeg een UI Text element toe.");
            enabled = false;
            return;
        }

        Canvas canvas = messageText.GetComponentInParent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        
        messageText.text = "";
    }

    void Update()
    {
        if (player == null || messageText == null) return;

        UpdateTextPosition();

        Collider[] hitColliders = Physics.OverlapSphere(player.position, interactionDistance);
        
        Debug.Log($"Aantal gevonden colliders: {hitColliders.Length}");
        
        foreach (Collider hitCollider in hitColliders)
        {
            GameObject human = hitCollider.gameObject;
            // Debug.Log($"Gevonden object: {human.name}, Tag: {human.tag}");
            
            if (human != player.gameObject && human.CompareTag("Human"))
            {
                Debug.Log("Human gevonden! Interactie start...");
                InteractWithHuman(human);
                break;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (showDebugSphere && player != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(player.position, interactionDistance);
        }
    }

    private void InteractWithHuman(GameObject human)
    {
        DisplayRealityMessage();
    }

    private void DisplayRealityMessage()
    {
        if (messageText != null)
        {
            if (messageCoroutine != null)
            {
                StopCoroutine(messageCoroutine);
            }
            string message = realityMessages[Random.Range(0, realityMessages.Count)];
            Debug.Log($"Probeer bericht te tonen: {message}");
            messageCoroutine = StartCoroutine(DisplayMessageCoroutine(message));
        }
        else
        {
            Debug.LogError("MessageText is null in DisplayRealityMessage!");
        }
    }

    private IEnumerator DisplayMessageCoroutine(string message)
    {
        messageText.text = message;
        Debug.Log($"Bericht gezet: {message}");
        yield return new WaitForSeconds(messageDisplayTime);
        messageText.text = "";
        Debug.Log("Bericht verwijderd");
    }

    private void UpdateTextPosition()
    {
        Canvas canvas = messageText.GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                Vector3 position = mainCamera.transform.position + mainCamera.transform.forward * textDistance;
                
                canvas.transform.position = position;
                
                canvas.transform.LookAt(mainCamera.transform);
                canvas.transform.Rotate(0, 180, 0);
            }
        }
    }
}

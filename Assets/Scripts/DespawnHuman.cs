using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DespawnHuman : MonoBehaviour
{
    public Transform player;
    public float safeDistance = 3f;
    public Text messageText;
    public float messageDisplayTime = 3f;

    private bool hasDespawned = false;

    private List<string> depressionMessages = new List<string>
    {
        "Ik voel me gevangen in mijn eigen gedachten...",
        "Waarom kan ik niet gewoon gelukkig zijn?",
        "Het voelt alsof niemand me begrijpt...",
        "Elke dag is een gevecht, maar ik blijf doorgaan...",
        "Zelfs in een kamer vol mensen voel ik me alleen..."
    };

    private Coroutine messageCoroutine;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player Transform is niet toegewezen!");
        }

        if (messageText == null)
        {
            Debug.LogError("Message Text is niet toegewezen! Voeg een UI Text element toe.");
        }
    }

    void Update()
    {
        if (player == null || hasDespawned)
        {
            return;
        }

        Vector3 playerPosition = player.position;
        Vector3 cubePosition = transform.position;
        float distanceToPlayer = Vector3.Distance(cubePosition, playerPosition);

        if (distanceToPlayer < safeDistance)
        {
            DespawnCube();
        }
    }

    private void DespawnCube()
    {
        if (hasDespawned)
        {
            return;
        }

        hasDespawned = true;
        Debug.Log($"{gameObject.name}: Speler te dichtbij! Kubus despawnt.");
        DisplayDepressionMessage();
    }

    private void DisplayDepressionMessage()
    {
        if (messageText != null)
        {
            if (messageCoroutine != null)
            {
                StopCoroutine(messageCoroutine);
            }

            string message = depressionMessages[Random.Range(0, depressionMessages.Count)];
            messageCoroutine = StartCoroutine(DisplayMessageCoroutine(message));
        }
    }

    private IEnumerator DisplayMessageCoroutine(string message)
    {
        messageText.text = message;
        Debug.Log("Start displaying message: " + message);
        yield return new WaitForSeconds(messageDisplayTime);
        messageText.text = "";
        Debug.Log("Message cleared after waiting: " + messageDisplayTime);

        // Vernietig het object nadat het bericht is weergegeven
        Destroy(gameObject);
    }

}



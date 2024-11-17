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

    private List<string> realityMessages = new List<string>
    {
        "Je denkt misschien dat iedereen op je let, maar de waarheid is dat de meeste mensen bezig zijn met hun eigen zorgen...",
        "Mensen zien vaak niet eens wat je doet, omdat ze druk zijn met hun eigen leven...",
        "Het voelt misschien alsof iedereen je beoordeelt, maar de meeste mensen denken nauwelijks na over wat je doet...",
        "Je maakt je waarschijnlijk te veel zorgen over de mening van anderen, terwijl ze het niet eens opmerken...",
        "De realiteit is dat de meeste mensen te druk zijn met hun eigen gedachten om zich met jou bezig te houden..."
    };

    private Coroutine messageCoroutine;
    private bool messageIsDisplaying = false;

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
        if (player == null || messageIsDisplaying)
        {
            return;
        }

        Collider[] hitColliders = Physics.OverlapSphere(player.position, interactionDistance);
        foreach (Collider hitCollider in hitColliders)
        {
            GameObject human = hitCollider.gameObject;
            if (human != player.gameObject && human.CompareTag("Human"))
            {
                InteractWithHuman(human);
                break;
            }
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
            messageCoroutine = StartCoroutine(DisplayMessageCoroutine(message));
        }
    }

    private IEnumerator DisplayMessageCoroutine(string message)
    {
        messageIsDisplaying = true;
        messageText.text = message;
        yield return new WaitForSeconds(messageDisplayTime);
        messageText.text = "";
        messageIsDisplaying = false;
    }

}

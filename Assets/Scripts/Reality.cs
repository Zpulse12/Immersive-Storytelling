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
        "Mensen letten niet op jou, ze zijn bezig met hun eigen zorgen...",
        "Mensen zien vaak niet wat je doet, ze zijn druk met hun leven...",
        "Mensen beoordelen je niet, ze denken nauwelijks over je na...",
        "Anderen merken je niet eens op, je zorgen zijn waarschijnlijk onnodig...",
        "Mensen zijn te druk met zichzelf om zich met jou bezig te houden..."
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

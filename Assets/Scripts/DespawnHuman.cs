using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DespawnHuman : MonoBehaviour
{
    public Transform player;
    public float safeDistance = 3f;
    public Text messageText;
    public float messageDisplayTime = 3f;
    public float textDistance = 2f;
    public float textHeight = 1.6f;

    private bool canShowMessage = true;
    private Coroutine messageCoroutine;
    private int currentMessageIndex = 0;

    private List<string> depressionMessages = new List<string>
    {
        "Ik voel me gevangen in mijn eigen gedachten...",
        "Waarom kan ik niet gewoon gelukkig zijn?",
        "Het voelt alsof niemand me begrijpt...",
        "Elke dag is een gevecht, maar ik blijf doorgaan...",
        "Zelfs in een kamer vol mensen voel ik me alleen..."
    };

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
            Debug.LogError("Message Text is niet toegewezen!");
            enabled = false;
            return;
        }

        Canvas canvas = messageText.GetComponentInParent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.transform.localScale = new Vector3(0.008f, 0.008f, 0.008f);
        messageText.text = "";
    }

    void Update()
    {
        if (player == null) return;

        UpdateTextPosition();

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < safeDistance && canShowMessage)
        {
            DisplayMessage();
            canShowMessage = false;
            StartCoroutine(ResetMessageTimer());
        }
    }

    private void UpdateTextPosition()
    {
        Canvas canvas = messageText.GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                Vector3 position = mainCamera.transform.position + 
                                 mainCamera.transform.forward * textDistance;
                
                canvas.transform.position = position;
                canvas.transform.LookAt(mainCamera.transform);
                canvas.transform.Rotate(0, 180, 0);
            }
        }
    }

    private void DisplayMessage()
    {
        if (messageText != null)
        {
            if (messageCoroutine != null)
            {
                StopCoroutine(messageCoroutine);
            }

            string message = depressionMessages[currentMessageIndex];
            currentMessageIndex = (currentMessageIndex + 1) % depressionMessages.Count;
            
            messageCoroutine = StartCoroutine(DisplayMessageCoroutine(message));
        }
    }

    private IEnumerator DisplayMessageCoroutine(string message)
    {
        messageText.text = message;
        yield return new WaitForSeconds(messageDisplayTime);
        messageText.text = "";
    }

    private IEnumerator ResetMessageTimer()
    {
        yield return new WaitForSeconds(10f);
        canShowMessage = true;
    }
}



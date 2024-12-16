using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public RectTransform canvasTransform; // Assign your Canvas RectTransform in the Inspector
    public float forwardDistance = 100f; // Distance to move forward (adjustable in Inspector)
    public float delay = 5f; // Delay before moving the Canvas

    void Start()
    {
        // Start the coroutine to move the Canvas after the delay
        StartCoroutine(MoveCanvasAfterDelay());
    }

    private IEnumerator MoveCanvasAfterDelay()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Move the Canvas forward
        if (canvasTransform != null)
        {
            canvasTransform.anchoredPosition += new Vector2(0, forwardDistance);
        }
    }
}

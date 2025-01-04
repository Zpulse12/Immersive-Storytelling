using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XRButtonInteraction : MonoBehaviour
{
    private static readonly int ButtonPressed = Animator.StringToHash("ButtonPressed");
    public Animator animator;
    public GameObject endText;

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    public void OnButtonPressed()
    {
        Debug.Log("Button pressed!");
        if (animator == null)
        {
            Debug.LogError("Animator is not assigned!");
            return;
        }

        animator.SetTrigger(ButtonPressed);
        
        // Haptic feedback
        var device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        if (device.TryGetHapticCapabilities(out var capabilities) && capabilities.supportsImpulse)
        {
            device.SendHapticImpulse(0, 0.5f, 0.2f); // Channel, intensity, duration
        }
        
        if (endText != null)
        {
            StartCoroutine(ShowEndTextAfterDelay(1.2f));
        }
    }
    
    private IEnumerator ShowEndTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        endText.SetActive(true);
    }
}

// File: Scripts/TurnAwayFromPlayer.cs

using System.Collections;
using UnityEngine;

public class TurnAwayFromPlayer : MonoBehaviour
{
    public Transform player;
    public float maxRotationSpeed = 10f;
    public float accelerationTime = 1f; 
    private float currentRotationSpeed = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            StartCoroutine(SmoothTurnAway());
        }
    }

    private IEnumerator SmoothTurnAway()
    {
        Vector3 directionAway = (transform.position - player.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionAway);

        float elapsedTime = 0f;

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            elapsedTime += Time.deltaTime;
            currentRotationSpeed = Mathf.SmoothStep(0f, maxRotationSpeed, elapsedTime / accelerationTime);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * currentRotationSpeed);

            yield return null;
        }

        currentRotationSpeed = 0f;
    }
}
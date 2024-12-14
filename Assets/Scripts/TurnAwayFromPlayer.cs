using UnityEngine;

public class TurnAwayFromPlayer : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float rotationSpeed = 5f; // Rotation speed in degrees per second

    private bool isTurning = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            isTurning = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            isTurning = false;
        }
    }

    private void Update()
    {
        if (isTurning)
        {
            TurnAwayFromPlayerSmoothly();
        }
    }

    private void TurnAwayFromPlayerSmoothly()
    {
        // Calculate the direction away from the player
        Vector3 directionAway = transform.position - player.position;

        // Ignore the vertical component to restrict rotation to the Y-axis
        directionAway.y = 0f;

        // Ensure the direction vector is normalized
        directionAway.Normalize();

        // Calculate the target rotation to face away from the player
        Quaternion targetRotation = Quaternion.LookRotation(directionAway);

        // Smoothly interpolate the current rotation towards the target rotation
        transform.rotation = Quaternion.Slerp(
            transform.rotation, 
            targetRotation, 
            rotationSpeed * Time.deltaTime
        );
    }
}
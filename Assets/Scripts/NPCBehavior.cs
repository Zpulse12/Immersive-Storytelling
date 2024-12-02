using UnityEngine;

public class NPCBehavior : MonoBehaviour
{
    private Animator animator;
    
    public float detectionRange = 5f;
    public float walkAwayDelay = 5f;
    public float walkSpeed = 2f;
    public float walkDistance = 10f;

    // Referenties naar de animator controllers
    public RuntimeAnimatorController idleController;
    public RuntimeAnimatorController walkController;

    private bool isWalkingAway = false;
    private bool playerDetected = false;
    private float detectionTimer = 0f;
    private bool canStartWalking = false;
    private Vector3 walkDirection;
    private Camera xrCamera;

    void Start()
    {
        animator = GetComponent<Animator>();
        
        if (animator == null)
        {
            Debug.LogError("Geen Animator component gevonden op NPC!");
            return;
        }

        // Zoek de XR Camera
        xrCamera = Camera.main;
        if (xrCamera == null)
        {
            Debug.LogError("Geen Main Camera gevonden!");
            return;
        }

        // Start met idle controller
        if (idleController != null)
        {
            Debug.Log("Start met idle animatie");
            animator.runtimeAnimatorController = idleController;
        }
        else
        {
            Debug.LogError("IdleController is niet toegewezen in de Inspector!");
        }
    }

    public void EnableWalking()
    {
        canStartWalking = true;
        Debug.Log("EnableWalking aangeroepen - canStartWalking is nu true");
    }

    void Update()
    {
        if (!canStartWalking || xrCamera == null) 
        {
            return;
        }

        // Check afstand tot XR Camera
        float distanceToPlayer = Vector3.Distance(transform.position, xrCamera.transform.position);

        if (distanceToPlayer <= detectionRange && !playerDetected)
        {
            playerDetected = true;
            detectionTimer = 0f;
            Debug.Log("XR Camera gedetecteerd binnen bereik");
        }

        if (playerDetected)
        {
            detectionTimer += Time.deltaTime;
            if (detectionTimer >= walkAwayDelay)
            {
                if (!isWalkingAway)
                {
                    WalkAway();
                }
            }
        }

        // Als we aan het weglopen zijn, beweeg de NPC
        if (isWalkingAway)
        {
            // Beweeg de NPC
            transform.position += walkDirection * walkSpeed * Time.deltaTime;
            
            // Debug info over animatie
            if (animator != null && animator.runtimeAnimatorController != null)
            {
                try
                {
                    AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
                    if (clipInfo.Length > 0)
                    {
                        Debug.Log($"Huidige animatie clip: {clipInfo[0].clip.name}");
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Kon animatie info niet ophalen: {e.Message}");
                }
            }
        }
    }

    void WalkAway()
    {
        if (!isWalkingAway && xrCamera != null)
        {
            Debug.Log("WalkAway functie start");
            isWalkingAway = true;
            
            // Bereken richting weg van de XR Camera
            walkDirection = (transform.position - xrCamera.transform.position).normalized;
            
            // Zorg dat de NPC alleen horizontaal draait (y-as)
            walkDirection.y = 0;
            walkDirection = walkDirection.normalized;
            
            // Draai de NPC in de looprichting
            if (walkDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(walkDirection);
            }

            if (walkController != null)
            {
                Debug.Log("Verander naar walk controller");
                animator.runtimeAnimatorController = walkController;
                
                // Force de animatie te updaten
                animator.Rebind();
                animator.Update(0f);
                
                Debug.Log($"Huidige controller: {animator.runtimeAnimatorController.name}");
                
                try
                {
                    AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
                    if (clipInfo.Length > 0)
                    {
                        Debug.Log($"Huidige animatie clip: {clipInfo[0].clip.name}");
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Kon animatie info niet ophalen: {e.Message}");
                }
            }
            else
            {
                Debug.LogError("WalkController is niet toegewezen in de Inspector!");
            }

            Invoke("DestroyNPC", walkDistance / walkSpeed);
        }
    }

    void DestroyNPC()
    {
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
} 
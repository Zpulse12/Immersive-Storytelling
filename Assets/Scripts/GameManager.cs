using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int SpotlightsVisited { get; private set; }

    private List<ChasePlayer> chasePlayers = new List<ChasePlayer>();
    private bool allStopped = false;
    private bool isTimerRunning = false;
    public float sceneTimer = 4;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameManager initialized");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IncrementSpotlightsVisited()
    {
        SpotlightsVisited++;
        Debug.Log($"[GameManager] Spotlight visited! New total: {SpotlightsVisited}");
    }

    public void ResetSpotlightsVisited()
    {
        SpotlightsVisited = 0;
        Debug.Log("SpotlightsVisited reset to 0");
    }

    public void RegisterChasePlayer(ChasePlayer chasePlayer)
    {
        if (!chasePlayers.Contains(chasePlayer))
        {
            chasePlayers.Add(chasePlayer);
            chasePlayer.OnStopped += CheckAllStopped; // Subscribe to the stop event
        }
    }

    private void CheckAllStopped()
    {
        // Check if all ChasePlayer objects have stopped
        allStopped = true;
        foreach (var player in chasePlayers)
        {
            if (!player.HasStopped)
            {
                allStopped = false;
                break;
            }
        }

        if (allStopped && !isTimerRunning)
        {
            Debug.Log("All objects have stopped. Starting timer...");
            DisableVRMovement(); // Disable VR movement when all have stopped
            StartCoroutine(AllStoppedCoroutine());
        }
    }

    private IEnumerator AllStoppedCoroutine()
    {
        isTimerRunning = true;
        yield return new WaitForSeconds(sceneTimer);

        Debug.Log("Timer finished. Loading new scene...");
        LoadNewScene();
        isTimerRunning = false;
    }

    private void DisableVRMovement()
    {
        // Logic to disable VR movement
        Debug.Log("VR movement disabled.");
        
        var vrController = FindObjectOfType<DynamicMoveProvider>();
        if (vrController != null)
        {
            vrController.enabled = false;
        }
    }

    public void LoadNewScene()
    {
        SceneManager.LoadScene("Scenes/Endscene");
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int SpotlightsVisited { get; private set; }
    
    private List<ChasePlayer> chasePlayers = new List<ChasePlayer>();
    private bool allStopped = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameManager geïnitialiseerd");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IncrementSpotlightsVisited()
    {
        SpotlightsVisited++;
        Debug.Log($"[GameManager] Spotlight bezocht! Nieuw totaal: {SpotlightsVisited}");
        
        Debug.Log($"[GameManager] Trigger positie: {UnityEngine.EventSystems.EventSystem.current?.currentSelectedGameObject?.transform.position}");
    }

    public void ResetSpotlightsVisited()
    {
        SpotlightsVisited = 0;
        Debug.Log("SpotlightsVisited gereset naar 0");
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

        if (allStopped)
        {
            Debug.Log("All objects have stopped. Loading new scene...");
            LoadNewScene();
        }
    }
    
    public void LoadNewScene()
    {
        SceneManager.LoadScene("Scenes/Endscene");
    }
} 
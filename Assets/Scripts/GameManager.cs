using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int SpotlightsVisited { get; private set; }

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
} 
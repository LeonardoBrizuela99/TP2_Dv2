using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   
    
    [SerializeField] private GameObject platformBase;

    private int _score = 0;
    private int _perfectStreak = 0;
    private int _towersPlaced = 0;
    private bool _isGameOver = false;

    public bool IsGameOver => _isGameOver;

    private void OnEnable()
    {
        GameEvents.OnBlockLanded += HandleNormalLanding;
        GameEvents.OnPerfectDrop += HandlePerfectLanding;
        GameEvents.OnBlockFailed += HandleGameOver;
    }

    private void OnDisable()
    {
        GameEvents.OnBlockLanded -= HandleNormalLanding;
        GameEvents.OnPerfectDrop -= HandlePerfectLanding;
        GameEvents.OnBlockFailed -= HandleGameOver;
    }

    private void Start()
    {
        ResetStats();

      
        if (platformBase != null)
        {
            platformBase.layer = LayerMask.NameToLayer("Base");
        }
    }

    private void HandleNormalLanding(float height, float errorX)
    {
        if (_isGameOver) return;

        _score += 100;
        _perfectStreak = 0;
        _towersPlaced++;

       
        if (_towersPlaced == 1 && platformBase != null)
        {
            platformBase.layer = LayerMask.NameToLayer("Floor");
            Debug.Log("GameManager: Plataforma base clausurada. Ahora es zona letal.");
        }

        GameEvents.TriggerScoreChanged(_score);
        GameEvents.TriggerPerfectStreakChanged(_perfectStreak);
        GameEvents.TriggerTowersPlacedChanged(_towersPlaced);
    }

    private void HandlePerfectLanding()
    {
        if (_isGameOver) return;

        _score += 150;
        _perfectStreak++;

        GameEvents.TriggerScoreChanged(_score);
        GameEvents.TriggerPerfectStreakChanged(_perfectStreak);
    }

    private void HandleGameOver()
    {
        if (!_isGameOver)
        {
            _isGameOver = true;
            GameEvents.TriggerGameOver();
            Debug.Log("GAME OVER: El bloque cayó al vacío.");

            Invoke(nameof(ReloadCurrentScene), 2f);
        }
    }

    private void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ResetStats()
    {
        _score = 0;
        _perfectStreak = 0;
        _towersPlaced = 0;
        _isGameOver = false;
    }
}

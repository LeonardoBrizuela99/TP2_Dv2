using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement;

public class GameplayUIManager : MonoBehaviour
{
  
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI strikesText;
    [SerializeField] private TextMeshProUGUI heightText;

   
    [SerializeField] private GameObject gameOverPanel;

    private float _maxHeight = 0f;

    private void OnEnable()
    {
        
        GameEvents.OnScoreChanged += UpdateScoreUI;
        GameEvents.OnPerfectStreakChanged += UpdateStrikesUI;
        GameEvents.OnBlockLanded += UpdateHeightUI;
        GameEvents.OnGameOver += ShowGameOverUI;
    }

    private void OnDisable()
    {
       
        GameEvents.OnScoreChanged -= UpdateScoreUI;
        GameEvents.OnPerfectStreakChanged -= UpdateStrikesUI;
        GameEvents.OnBlockLanded -= UpdateHeightUI;
        GameEvents.OnGameOver -= ShowGameOverUI;
    }

    private void Start()
    {
        
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    private void UpdateScoreUI(int score)
    {
        scoreText.text = "Score: " + score;
    }

    private void UpdateStrikesUI(int streak)
    {
        strikesText.text = "Perfect: " + streak;
    }

    private void UpdateHeightUI(float landedHeight, float errorX)
    {
        if (landedHeight > _maxHeight)
        {
            _maxHeight = landedHeight;
            heightText.text = "Height: " + _maxHeight.ToString("F1") + "m";
        }
    }

    private void ShowGameOverUI()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}

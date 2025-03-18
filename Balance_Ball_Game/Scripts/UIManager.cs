using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject menuScreen;
    private IAudioService _audioService;
    
    public void Initialize(IAudioService audioService)
    {
        _audioService = audioService;
    }
    public void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    public void ShowGameOverScreen(int finalScore)
    {     
        gameOverScreen.SetActive(true);
        finalScoreText.text = $"Final Score: {finalScore}";
    }
    public void OnRestartButton()
    {        
        HandleButtonClick();
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }    
    public void OnJellySerenityButton()
    {        
        HandleButtonClick();
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }  
    
    public void OnMenuButton()
    {        
        HandleButtonClick();
       // Time.timeScale = 0f;
        gameOverScreen.SetActive(false);
        menuScreen.SetActive(true);
    }  
    

    private void HandleButtonClick()
    {
        _audioService.PlayUiButtonClick();
    }
    
}
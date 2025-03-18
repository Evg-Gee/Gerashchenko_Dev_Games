using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PendulumSwing pendulum;
    [SerializeField] private CircleFactory circleFactory; 
    [SerializeField] private ZoneController[] zones;
    [SerializeField] private ComboChecker comboChecker;
    [SerializeField] private GameStateManager gameState;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private AudioManager _audioManager;
    
    
    private ScoreManager scoreManager;
    
    void Awake()
    {
        scoreManager = new ScoreManager();
        gameState.Initialize(scoreManager);
    }
    void Start()
    {
        InitializePendulum();
        uiManager.Initialize(_audioManager);
        _audioManager.PlayOSTSound();
    }
    
    private void OnEnable()
    {
        InputHandler.OnTap += HandleTap;
        comboChecker.OnComboDetected += HandleCombo;
        comboChecker.OnMinusScore += HandleMinusScore;
        gameState.OnGameOver += HandleGameOver;
    }

    private void OnDisable()
    {
        InputHandler.OnTap -= HandleTap;
        comboChecker.OnComboDetected -= HandleCombo;
        comboChecker.OnMinusScore -= HandleMinusScore;
        gameState.OnGameOver -= HandleGameOver;
    }
    private void HandleTap()
    {
        if (pendulum.HasCircle())
        {
            _audioManager.PlayPendulumClick();
            pendulum.ReleaseCircle();
        }
        else
        {
            GameObject circle = circleFactory.CreateCircle();
            if (circle != null)
            {    
                _audioManager.PlayUiButtonClick();       
                pendulum.AttachCircle(circle);
            }
        }
    }
    private void HandleCombo(Color color)
    {
        _audioManager.PlayComboExplosion();
        scoreManager.AddScore(color);
        uiManager.UpdateScore(scoreManager.GetCurrentScore());
    }
    private void HandleMinusScore(Color color)
    {
        _audioManager.PlayMinusScore();
        scoreManager.SubtractScore(color);
        uiManager.UpdateScore(scoreManager.GetCurrentScore());
    }

    private void HandleGameOver()
    {
        _audioManager.PlayGameOver();
        uiManager.ShowGameOverScreen(scoreManager.GetCurrentScore());
        Time.timeScale = 0;
    }
    private void InitializePendulum()
    {
        GameObject initialCircle = circleFactory.CreateCircle();
        pendulum.AttachCircle(initialCircle);
    }
    private void InitializeZones()
    {       
        foreach (var zone in zones)
        {
            int circleCount = Random.Range(1, 3); // 1 или 2 круга
            for (int i = 0; i < circleCount; i++)
            {
                GameObject circle = circleFactory.CreateCircle();
                if (!zone.AddCircle(circle))
                {
                    Destroy(circle);
                }
            }
        }
    }
}
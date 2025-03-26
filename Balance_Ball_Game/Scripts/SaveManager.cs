using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    private ScoreManager scoreManager;
    private const string SAVE_KEY = "GameSave";
    public static SaveData CurrentSave { get; private set; }

    void Awake()
    {
         if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Чтобы сохранить объект при загрузке сцен
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Загрузка при старте игры
    private void Start()
    {
        LoadGame();
    }
    
   
    // Сохранение при выходе
    private void OnApplicationQuit()
    {
        SaveGame();
    }

    // Вызывайте этот метод при проигрыше
    public void OnPlayerDefeat()
    {
        SaveGame();
    }

    public void SaveGame()
    {
        if (scoreManager == null)
        {
            Debug.LogError("ScoreManager не установлен!");
            return;
        }

        CurrentSave.score = scoreManager.GetCurrentScore();
        bool success = WebStorage.Save(SAVE_KEY, CurrentSave);
        Debug.Log(success ? "Сохранено!" + CurrentSave.score : "Ошибка сохранения!" + CurrentSave.score);
    }

    public void LoadGame()
    {
        CurrentSave = WebStorage.Load<SaveData>(SAVE_KEY);
        Debug.Log("Данные загружены. Очков набрано: " + CurrentSave.score);

        if (scoreManager != null)
        {
            scoreManager.SetScore(CurrentSave.score);
        }
        else
        {
            Debug.LogError("ScoreManager не инициализирован!");
        }
    }
    
     public void SetScoreManager(ScoreManager scoreManager)
    {
        this.scoreManager = scoreManager;
    }
}
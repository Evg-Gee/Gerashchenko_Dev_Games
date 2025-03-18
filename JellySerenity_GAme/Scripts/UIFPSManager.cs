using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class UIFPSManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText; // Ссылка на UI Text элемент

    private void Update()
    {
        float fps = Mathf.Round(1f / Time.deltaTime);
        fpsText.text = $"FPS: {fps}";
    }    
    
    public void NextMenuBttn()
        {
            SceneManager.LoadScene(1);
        }
}   



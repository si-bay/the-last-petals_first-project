using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance; // Agar bisa dipanggil dari PlayerHealth
    public CanvasGroup gameOverPanel;       // Link ke UI_GameOver

    void Awake()
    {
        Instance = this;
        // Pastikan tersembunyi saat mulai
        if(gameOverPanel != null)
        {
            gameOverPanel.alpha = 0;
            gameOverPanel.interactable = false;
            gameOverPanel.blocksRaycasts = false;
        }
    }

    public void ShowGameOver()
    {
        gameOverPanel.gameObject.SetActive(true);
        gameOverPanel.alpha = 1;
        gameOverPanel.interactable = true;
        gameOverPanel.blocksRaycasts = true;
        Time.timeScale = 0f; // Stop total game
    }

    public void Retry()
    {
        Time.timeScale = 1f; // Lanjutkan waktu
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload scene
    }

    public void ToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Ganti dengan nama scene menu kamu
    }
}
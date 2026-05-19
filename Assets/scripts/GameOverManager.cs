using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public CanvasGroup gameOverPanel;
    
    // Method ini WAJIB bernama ShowGameOver() agar dipanggil PlayerHealth
    public void ShowGameOver()
    {
        if (gameOverPanel == null) return;
        
        gameOverPanel.gameObject.SetActive(true);
        gameOverPanel.alpha = 1f;
        gameOverPanel.interactable = true;
        gameOverPanel.blocksRaycasts = true;
        Time.timeScale = 0f;
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
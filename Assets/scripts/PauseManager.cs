using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public CanvasGroup pausePanel;
    public NoteInteraction noteSystem; // Drag object Note interaction ke sini

    private bool isPaused = false;

    void Update()
    {
        // Jangan pause jika sedang baca catatan
        if (Input.GetKeyDown(KeyCode.Escape) && (noteSystem == null || !noteSystem.isOpen))
            TogglePause();
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        
        if (isPaused)
        {
            pausePanel.alpha = 1f;
            pausePanel.interactable = true;
            pausePanel.blocksRaycasts = true;
            Time.timeScale = 0f;
        }
        else
        {
            pausePanel.alpha = 0f;
            pausePanel.interactable = false;
            pausePanel.blocksRaycasts = false;
            Time.timeScale = 1f;
        }
    }

    public void Resume() => TogglePause();

    public void ToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
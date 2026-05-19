using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public string gameplaySceneName = "Gameplay"; // Nama scene gameplay kamu

    public void PlayGame()
    {
        PlayerHealth.ResetDeathCounter(); // Reset kematian sebelum mulai
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
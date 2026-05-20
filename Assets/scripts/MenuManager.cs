using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Tooltip("Nama persis file scene gameplay kamu (tanpa .unity)")]
    public string gameplaySceneName = "Gameplay";

    public void PlayGame()
    {
        // ✅ TIDAK PERLU RESET MANUAL LAGI
        // PlayerHealth.cs sudah auto-reset currentLives di method Awake() 
        // saat SceneManager.LoadScene() dijalankan.
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
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [Header("🖼️ UI Reference")]
    public CanvasGroup pausePanel; // Drag UI_Pause ke sini

    private bool isPaused = false;

    void Update()
    {
        // Deteksi tombol Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    // Fungsi utama: Switch antara Pause & Resume
    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            // 🛑 MODE PAUSE
            Time.timeScale = 0f;          // Hentikan fisika & animasi
            pausePanel.alpha = 1f;        // Tampilkan UI
            pausePanel.interactable = true; // Aktifkan klik tombol
            pausePanel.blocksRaycasts = true; // Blokir klik ke game di belakang
        }
        else
        {
            // ▶️ MODE RESUME
            Time.timeScale = 1f;          // Lanjutkan fisika
            pausePanel.alpha = 0f;        // Sembunyikan UI
            pausePanel.interactable = false;
            pausePanel.blocksRaycasts = false;
        }
    }

    // Dipanggil saat tombol RESUME di UI diklik
    public void ResumeGame()
    {
        if (isPaused) TogglePause();
    }
}
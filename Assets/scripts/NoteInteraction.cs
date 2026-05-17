using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NoteInteraction : MonoBehaviour
{
    [Header(" UI References")]
    public CanvasGroup notePanel;      // Panel + CanvasGroup
    public Image darkOverlay;          // Fullscreen hitam

    [Header("⚙️ Settings")]
    public KeyCode interactKey = KeyCode.E;
    public float transitionSpeed = 3f; // Kecepatan animasi buka/tutup

    private bool isNear = false;
    private bool isOpen = false;
    private PlayerController playerController;

    void Start()
    {
        // Cari otomatis PlayerController di scene
        playerController = FindObjectOfType<PlayerController>();
        
        // State awal: invisible & non-aktif
        if (notePanel) 
        {
            notePanel.alpha = 0f;
            notePanel.interactable = false;
            notePanel.blocksRaycasts = false;
            notePanel.gameObject.SetActive(false);
        }
        if (darkOverlay) darkOverlay.gameObject.SetActive(false);
    }

    // Trigger saat Player mendekat
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            isNear = true;
            Debug.Log("📜 Dekat dengan catatan. Tekan [E] untuk membaca.");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            isNear = false;
            if (isOpen) CloseNote(); // Auto-close jika player jalan menjauh
        }
    }

    void Update()
    {
        // Buka catatan
        if (isNear && Input.GetKeyDown(interactKey) && !isOpen)
            OpenNote();
        // Tutup catatan
        else if (isOpen && Input.GetKeyDown(interactKey))
            CloseNote();
    }

    void OpenNote()
    {
        isOpen = true;
        playerController.enabled = false; // Matikan kontrol player
        notePanel.gameObject.SetActive(true);
        darkOverlay.gameObject.SetActive(true);
        StartCoroutine(AnimateOpen());
    }

    void CloseNote()
    {
        isOpen = false;
        StartCoroutine(AnimateClose());
    }

    // Animasi Buka (menggunakan unscaledDeltaTime karena game di-pause)
    IEnumerator AnimateOpen()
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * transitionSpeed;
            
            // Fade overlay & note
            if (darkOverlay) darkOverlay.color = new Color(0, 0, 0, Mathf.Lerp(0f, 0.85f, t));
            if (notePanel) notePanel.alpha = Mathf.Lerp(0f, 1f, t);
            
            // Zoom in note
            if (notePanel) notePanel.transform.localScale = Vector3.one * Mathf.Lerp(0.5f, 1f, t);
            
            yield return null;
        }
        
        // Kunci final state
        if (notePanel) 
        {
            notePanel.alpha = 1f;
            notePanel.interactable = true;
            notePanel.blocksRaycasts = true;
        }
        Time.timeScale = 0f; // Pause game sepenuhnya
    }

    // Animasi Tutup (menggunakan deltaTime normal)
    IEnumerator AnimateClose()
    {
        Time.timeScale = 1f; // Resume game dulu
        
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * transitionSpeed;
            
            if (darkOverlay) darkOverlay.color = new Color(0, 0, 0, Mathf.Lerp(0.85f, 0f, t));
            if (notePanel) notePanel.alpha = Mathf.Lerp(1f, 0f, t);
            if (notePanel) notePanel.transform.localScale = Vector3.one * Mathf.Lerp(1f, 0.5f, t);
            
            yield return null;
        }
        
        // Non-aktifkan UI & kembalikan kontrol
        if (notePanel) 
        {
            notePanel.gameObject.SetActive(false);
            notePanel.interactable = false;
            notePanel.blocksRaycasts = false;
        }
        if (darkOverlay) darkOverlay.gameObject.SetActive(false);
        playerController.enabled = true;
    }
}
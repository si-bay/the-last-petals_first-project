using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NoteInteraction : MonoBehaviour
{
    [Header("🖼️ UI Ref")]
    public CanvasGroup notePanel;
    public Image darkOverlay;
    public CanvasGroup promptUI; // ← DRAG Prompt_PressE KE SINI

    [Header("🔊 Audio")]
    public AudioClip paperFlipSound;
    private AudioSource audioSource;

    [Header("⚙️ Settings")]
    public KeyCode interactKey = KeyCode.E;
    public float animSpeed = 4f;

    private bool isNear = false;
    public bool isOpen { get; private set; } = false;
    private PlayerController playerController;

    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Sembunyikan UI awal
        if (notePanel) { notePanel.alpha = 0f; notePanel.gameObject.SetActive(false); }
        if (darkOverlay) darkOverlay.color = new Color(0, 0, 0, 0);
        if (promptUI) { promptUI.alpha = 0f; promptUI.gameObject.SetActive(false); }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = true;
            ShowPrompt();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = false;
            HidePrompt();
            if (isOpen) CloseNote();
        }
    }

    void Update()
    {
        if (isNear && Input.GetKeyDown(interactKey))
        {
            if (!isOpen) OpenNote();
            else CloseNote();
        }
    }

    void OpenNote()
    {
        isOpen = true;
        HidePrompt(); // Sembunyikan prompt saat membaca
        PlaySound();
        
        playerController.enabled = false;
        if (notePanel) { notePanel.gameObject.SetActive(true); notePanel.alpha = 0f; }
        if (darkOverlay) { darkOverlay.gameObject.SetActive(true); }
        StartCoroutine(AnimateOpen());
    }

    void CloseNote()
    {
        isOpen = false;
        PlaySound();
        StartCoroutine(AnimateClose());
    }

    IEnumerator AnimateOpen()
    {
        float t = 0;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * animSpeed;
            if (darkOverlay) darkOverlay.color = new Color(0, 0, 0, Mathf.Lerp(0f, 0.9f, t));
            if (notePanel) notePanel.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }
        if (notePanel) notePanel.alpha = 1f;
    }

    IEnumerator AnimateClose()
    {
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * animSpeed;
            if (darkOverlay) darkOverlay.color = new Color(0, 0, 0, Mathf.Lerp(0.9f, 0f, t));
            if (notePanel) notePanel.alpha = Mathf.Lerp(1f, 0f, t);
            yield return null;
        }
        
        if (notePanel) notePanel.gameObject.SetActive(false);
        if (darkOverlay) darkOverlay.gameObject.SetActive(false);
        playerController.enabled = true;
        
        if (isNear) ShowPrompt(); // Munculkan lagi jika player masih di zona
    }

    void PlaySound() { if (paperFlipSound && audioSource) audioSource.PlayOneShot(paperFlipSound); }

    void ShowPrompt()
    {
        if (promptUI)
        {
            promptUI.gameObject.SetActive(true);
            promptUI.alpha = 1f;
        }
    }

    void HidePrompt()
    {
        if (promptUI) promptUI.gameObject.SetActive(false);
    }
}
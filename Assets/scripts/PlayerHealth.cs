using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("❤️ Settings")]
    public int maxLives = 3;
    public int currentLives;
    public HealthUI healthUI; // <-- WAJIB DI-LINK DARI INSPECTOR

    [Header("⚠️ Danger")]
    public LayerMask hazardLayer; // Centang layer Hazard & Enemy

    [Header("🔄 Respawn")]
    public float respawnDelay = 1.0f;
    public float invincibilityTime = 2.0f;

    private PlayerController playerController;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isInvincible = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        currentLives = maxLives; // Reset nyawa tiap kali masuk scene
    }

    void Start()
    {
        // Set checkpoint awal jika belum ada
        if (CheckpointManager.LastCheckpointPosition == Vector3.zero)
            CheckpointManager.SetCheckpoint(transform.position);
        
        // Update UI saat game mulai
        if (healthUI != null)
            healthUI.UpdateHearts(currentLives);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isInvincible) return;

        // Cek apakah yang ditabrak ada di layer Hazard/Enemy
        if (((1 << other.gameObject.layer) & hazardLayer) != 0)
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int amount)
    {
        currentLives -= amount;

        // 1. Update Tampilan UI
        if (healthUI != null)
            healthUI.UpdateHearts(currentLives);

        // 2. Cek Mati atau Respawn
        if (currentLives <= 0)
        {
            Die(); // Game Over
        }
        else
        {
            StartCoroutine(RespawnRoutine()); // Respawn di checkpoint
        }
    }

    void Die()
    {
        // Matikan kontrol player
        playerController.enabled = false;
        rb.linearVelocity = Vector2.zero;

        // Panggil Game Over Screen
        if (GameOverManager.Instance != null)
        {
            GameOverManager.Instance.ShowGameOver();
        }
    }

    IEnumerator RespawnRoutine()
    {
        isInvincible = true;
        playerController.enabled = false;
        rb.linearVelocity = Vector2.zero;

        yield return new WaitForSecondsRealtime(respawnDelay);

        // Pindah ke checkpoint
        transform.position = CheckpointManager.LastCheckpointPosition;
        playerController.enabled = true;
        
        StartCoroutine(InvincibilityEffect());
    }

    IEnumerator InvincibilityEffect()
    {
        float t = 0;
        while (t < invincibilityTime)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled; // Kedip-kedip
            yield return new WaitForSecondsRealtime(0.1f);
            t += 0.1f;
        }
        spriteRenderer.enabled = true;
        isInvincible = false;
    }
}
using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("❤️ Settings")]
    public int maxHealth = 1;
    public int maxDeathsBeforeGameOver = 3;
    private static int sessionDeathCount = 0; // Reset di MenuManager

    [Header("⚠️ Hazard")]
    public LayerMask hazardLayer; // Centang layer Hazard & Enemy

    [Header("🔄 Respawn")]
    public float respawnDelay = 1.2f;
    public float invincibilityTime = 2f;

    private PlayerController playerController;
    private Rigidbody2D rb;
    private bool isInvincible = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
    }

    void Start()
    {
        if (CheckpointManager.LastCheckpointPosition == Vector3.zero)
            CheckpointManager.SetCheckpoint(transform.position);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isInvincible) return;

        // Cek apakah layer objek bertabrakan ada di mask hazard
        if (((1 << other.gameObject.layer) & hazardLayer) != 0)
        {
            TakeDamage(maxHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        // Jika HP <= 0 langsung mati (Instant Death logic)
        Die();
    }

    void Die()
    {
        sessionDeathCount++;

        if (sessionDeathCount >= maxDeathsBeforeGameOver)
        {
            // ✅ GANTI DENGAN LOOKUP DINAMIS
            GameOverManager gm = FindFirstObjectByType<GameOverManager>();
            if (gm != null) gm.ShowGameOver();
            return;
        }

        StartCoroutine(RespawnRoutine());
    }
    IEnumerator RespawnRoutine()
    {
        // Matikan player
        playerController.enabled = false;
        rb.linearVelocity = Vector2.zero;

        // Tunggu sebentar (opsional: play death animasi)
        yield return new WaitForSeconds(respawnDelay);

        // Pindah ke checkpoint
        transform.position = CheckpointManager.LastCheckpointPosition;
        rb.linearVelocity = Vector2.zero;
        playerController.enabled = true;
        isInvincible = true;

        // Invincibility frames (kedip-kedip)
        yield return new WaitForSeconds(invincibilityTime);
        isInvincible = false;
    }

    public static void ResetDeathCounter() => sessionDeathCount = 0;
}
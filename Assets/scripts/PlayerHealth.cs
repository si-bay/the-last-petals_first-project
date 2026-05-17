using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("️ Health Settings")]
    public int maxHealth = 3;
    [HideInInspector] public int currentHealth;

    [Header("🔥 Hazard Detection")]
    public LayerMask hazardLayer; // Assign layer "Hazard" di Inspector

    [Header("🔄 Respawn Settings")]
    public float respawnDelay = 1.5f;
    public float invincibilityTime = 2f;

    private PlayerController playerController;
    private Rigidbody2D rb;
    private bool isInvincible = false;

    void Awake()
    {
        currentHealth = maxHealth;
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Set checkpoint awal ke posisi spawn pertama kali
        if (CheckpointManager.LastCheckpointPosition == Vector3.zero)
            CheckpointManager.SetCheckpoint(transform.position);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isInvincible) return;

        // Cek apakah objek yang disentuh ada di layer Hazard
        if (((1 << other.gameObject.layer) & hazardLayer) != 0)
        {
            // Langsung mati saat sentuh obstacle (bisa diubah ke TakeDamage(1) kalau mau sistem HP bertahap)
            TakeDamage(maxHealth); 
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"❤️ Health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("💀 Player mati! Respawn...");
        
        // Matikan kontrol movement & jump
        if (playerController != null) playerController.enabled = false;
        
        // TODO: Play death animation / sound di sini
        
        Invoke(nameof(Respawn), respawnDelay);
    }

    void Respawn()
    {
        // Pindah ke checkpoint terakhir
        transform.position = CheckpointManager.LastCheckpointPosition;
        
        // Reset state
        currentHealth = maxHealth;
        rb.linearVelocity = Vector2.zero;
        isInvincible = true;
        
        // Nyalakan kembali kontrol
        if (playerController != null) playerController.enabled = true;
        
        Debug.Log("✨ Respawn berhasil!");
        StartCoroutine(EndInvincibility());
    }

    IEnumerator EndInvincibility()
    {
        // TODO: Tambahkan efek flicker/flash sprite selama invincible
        yield return new WaitForSeconds(invincibilityTime);
        isInvincible = false;
    }
}
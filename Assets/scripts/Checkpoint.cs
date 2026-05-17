using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Tooltip("Opsional: Offset Y agar player tidak spawn di dalam tanah")]
    public float spawnYOffset = 0.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Simpan posisi + offset agar kaki player pas di atas ground
            Vector3 spawnPos = transform.position + Vector3.up * spawnYOffset;
            CheckpointManager.SetCheckpoint(spawnPos);
            
            // TODO: Nanti bisa tambah efek visual (ganti sprite checkpoint jadi aktif)
        }
    }
}
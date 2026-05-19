using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public float spawnYOffset = 0.5f; // Agar player tidak spawn di dalam tanah

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 pos = transform.position + Vector3.up * spawnYOffset;
            CheckpointManager.SetCheckpoint(pos);
            
            // Opsional: Ganti sprite checkpoint jadi "aktif" di sini
        }
    }
}
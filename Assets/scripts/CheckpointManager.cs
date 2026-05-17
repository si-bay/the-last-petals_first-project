using UnityEngine;

public static class CheckpointManager
{
    // Posisi respawn terakhir. Default (0,0,0) akan ditimpa saat start
    public static Vector3 LastCheckpointPosition { get; private set; } = Vector3.zero;

    public static void SetCheckpoint(Vector3 position)
    {
        LastCheckpointPosition = position;
        Debug.Log($" Checkpoint disimpan di: {position}");
    }

    public static void ResetToDefault(Vector3 spawnPoint)
    {
        LastCheckpointPosition = spawnPoint;
    }
}
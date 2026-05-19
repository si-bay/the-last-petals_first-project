using UnityEngine;

public static class CheckpointManager
{
    public static Vector3 LastCheckpointPosition { get; private set; } = Vector3.zero;

    public static void SetCheckpoint(Vector3 position)
    {
        LastCheckpointPosition = position;
    }

    public static void ResetToDefault(Vector3 spawnPoint)
    {
        LastCheckpointPosition = spawnPoint;
    }
}
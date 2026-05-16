using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0f, 1f, -10f);
    public float smoothSpeed = 5f;

    private void LateUpdate()
    {
        if (player == null) return;

        // Posisi yang diinginkan kamera
        Vector3 desiredPosition = player.position + offset;
        // Interpolasi linear agar pergerakan halus
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    public int backgroundIndex;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            BackgroundManager.Instance.SwitchTo(backgroundIndex);
        }
    }
}
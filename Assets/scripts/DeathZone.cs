using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class DeathZone : MonoBehaviour
{
    [Header("Scene Management")]
    [Tooltip("Jika dicentang, akan reload scene saat ini. Jika tidak, akan memuat scene di bawah.")]
    [SerializeField] private bool reloadCurrentScene = true;

    [Tooltip("Nama scene tujuan (hanya dipakai jika reloadCurrentScene = false). WAJIB ada di Build Settings.")]
    [SerializeField] private string targetSceneName = "GameOver";

    [Header("Behavior")]
    [Tooltip("Jeda (detik) sebelum scene dimuat. Isi 0 untuk transisi instan.")]
    [SerializeField, Min(0f)] private float reloadDelay = 0f;

    [Tooltip("Tag yang menandai player. Harus sama persis dengan Tag di GameObject Player.")]
    [SerializeField] private string playerTag = "Player";

    // Mencegah method LoadScene dipanggil berulang kali
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // BARIS BARU - DEBUG LOG
        Debug.Log($"🔥 TRIGGER MASUK! Objek: {other.name} | Tag: {other.tag}");

        if (hasTriggered || !other.CompareTag(playerTag)) return;
        hasTriggered = true;
        StartDeathSequence();
    }

    private void StartDeathSequence()
    {
        if (reloadDelay > 0f)
        {
            Invoke(nameof(ExecuteSceneLoad), reloadDelay);
        }
        else
        {
            ExecuteSceneLoad();
        }
    }

private void ExecuteSceneLoad()
{
    string sceneToLoad = reloadCurrentScene ? SceneManager.GetActiveScene().name : targetSceneName;
    
    Debug.Log($"📂 Loading scene: {sceneToLoad}");
    
    if (string.IsNullOrWhiteSpace(sceneToLoad))
    {
        Debug.LogError("[DeathZone] Nama scene kosong!");
        return;
    }
    
    SceneManager.LoadScene(sceneToLoad);
}

}
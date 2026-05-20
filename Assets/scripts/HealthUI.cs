using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image[] hearts;      // Array yang tadi diset Size 3
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public void UpdateHearts(int currentHP)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            // Jika index lebih kecil dari nyawa -> Hati Penuh
            // Jika index lebih besar/sama -> Hati Kosong
            if (i < currentHP)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }
}
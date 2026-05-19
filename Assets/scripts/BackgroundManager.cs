using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager Instance { get; private set; }

    public List<SpriteRenderer> backgrounds;
    public float fadeSpeed = 2f;

    private int currentIndex = 0;
    private Coroutine transitionCoroutine;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        // Setup awal: BG 0 aktif, sisanya transparan
        for (int i = 0; i < backgrounds.Count; i++)
        {
            if (backgrounds[i] != null)
            {
                Color c = backgrounds[i].color;
                c.a = (i == 0) ? 1f : 0f;
                backgrounds[i].color = c;
                if (i != 0) backgrounds[i].gameObject.SetActive(false);
            }
        }
    }

    public void SwitchTo(int targetIndex)
    {
        if (targetIndex < 0 || targetIndex >= backgrounds.Count || targetIndex == currentIndex) return;
        
        if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);
        transitionCoroutine = StartCoroutine(Crossfade(currentIndex, targetIndex));
        currentIndex = targetIndex;
    }

    IEnumerator Crossfade(int fromIndex, int toIndex)
    {
        SpriteRenderer fromBG = backgrounds[fromIndex];
        SpriteRenderer toBG = backgrounds[toIndex];

        if (!toBG.gameObject.activeSelf) toBG.gameObject.SetActive(true);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;
            
            Color cFrom = fromBG.color; cFrom.a = Mathf.Lerp(1f, 0f, t); fromBG.color = cFrom;
            Color cTo = toBG.color; cTo.a = Mathf.Lerp(0f, 1f, t); toBG.color = cTo;
            
            yield return null;
        }

        fromBG.gameObject.SetActive(false);
    }
}
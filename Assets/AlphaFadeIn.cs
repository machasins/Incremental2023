using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaFadeIn : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float time = 0.0f;

        while (time < 0.5f)
        {
            GetComponent<SpriteRenderer>().color = Color.Lerp(Color.clear, Color.white, time / 0.5f);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        GetComponent<SpriteRenderer>().color = Color.white;
    }
}

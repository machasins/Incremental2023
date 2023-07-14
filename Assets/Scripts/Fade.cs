using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public float fadeTime = 0.25f;
    void Start()
    {
        StartCoroutine(FadeScreen(1.0f, 0.0f));
    }

    public void SwitchScene(string sceneName)
    {
        StartCoroutine(FadeScreen(0.0f, 1.0f, sceneName));
    }

    public IEnumerator FadeScreen(float f, float t, string sceneName = "")
    {
        Image fade = GetComponent<Image>();
        float time = 0.0f;

        Color from = fade.color;
        Color to = fade.color;
        from.a = f;
        to.a = t;

        while (time < fadeTime)
        {
            fade.color = Color.Lerp(from, to, time / fadeTime);

            time += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        fade.color = to;

        if (!sceneName.Equals(""))
            SceneManager.LoadScene(sceneName);
    }
}

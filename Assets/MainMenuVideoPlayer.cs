using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Networking;

public class MainMenuVideoPlayer : MonoBehaviour
{
    public string initialVideoName;
    public string clickVideoName;
    public GameObject firstFrame;
    public CanvasGroup MenuUI;
    public VideoPlayer initial;
    public float clickFrame;
    public GameObject clickObject;
    public VideoPlayer click;
    public AudioSource source;
    public GameObject mainMenu;
    public Fade fade;

    public string sceneName;

    void Start()
    {
        initial.url = System.IO.Path.Combine(Application.streamingAssetsPath, initialVideoName);
        click.url = System.IO.Path.Combine(Application.streamingAssetsPath, clickVideoName);

        StartCoroutine(PreloadVideos());
    }

    IEnumerator PreloadVideos()
    {
        float volume = source.volume;
        source.volume = 0;
        //initial.SetTargetAudioSource(0, null);
        //click.SetTargetAudioSource(0, null);

        initial.Play();
        click.Play();

        yield return new WaitForSeconds(0.25f);

        initial.Pause();
        click.Pause();

        //initial.SetTargetAudioSource(0, source);
        //click.SetTargetAudioSource(0, source);
        source.volume = volume;

        initial.frame = 0;
        click.frame = 0;
    }

    public void StartVideo()
    {
        initial.loopPointReached += WaitForClick;

        firstFrame.SetActive(false);
        StartCoroutine(DisableMenu());
        initial.Play();

        StartCoroutine(FindWhenClickable());
    }

    IEnumerator DisableMenu()
    {
        float time = 0.0f;

        while (time < 0.25f)
        {
            MenuUI.alpha = Mathf.Lerp(1.0f, 0.0f, time / 0.25f);

            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        MenuUI.gameObject.SetActive(false);
    }

    IEnumerator FindWhenClickable()
    {
        while (initial.isPlaying)
        {
            if (initial.frame >= clickFrame)
            {
                clickObject.SetActive(true);
                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    void WaitForClick(VideoPlayer vp)
    {
        clickObject.SetActive(true);
    }

    public void Click()
    {
        clickObject.SetActive(false);
        initial.gameObject.SetActive(false);
        click.loopPointReached += FinishScene;

        click.Play();
    }

    void FinishScene(VideoPlayer vp)
    {
        MenuUI.alpha = 1.0f;
        MenuUI.gameObject.SetActive(true);
        mainMenu.SetActive(false);

        fade.SwitchScene(sceneName);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    public AudioClip loveLow;
    public AudioClip loveHigh;
    public AudioClip hateLow;
    public AudioClip hateHigh;
    private AudioSource source1;
    private AudioSource source2;
    private AudioSource activeSource;
    private AudioSource inactiveSource;
    private float maxVolume;
    private bool crossFading = false;

    void Start()
    {
        source1 = GetComponent<AudioSource>();
        source1.clip = loveLow;
        maxVolume = source1.volume;

        source2 = gameObject.AddComponent<AudioSource>();
        source2.loop = true;

        activeSource = source1;
        inactiveSource = source2;

        source1.volume = 0;
        source2.volume = 0;
        source1.Play();

        Fade(true);
    }

    public void Fade(bool active)
    {
        if (!crossFading)
            StartCoroutine(ToggleFade(active));
    }

    public void CrossFade(AudioClip nextSong)
    {
        if (!crossFading && activeSource.clip != nextSong)
        {
            inactiveSource.clip = nextSong;
            inactiveSource.Play();

            StartCoroutine(CrossFade(activeSource, inactiveSource));
            
            AudioSource temp = activeSource;
            activeSource = inactiveSource;
            inactiveSource = temp;
        }
    }

    IEnumerator ToggleFade(bool active)
    {
        float time = 0.0f;

        crossFading = true;

        while (time < 0.5f)
        {
            activeSource.volume = Mathf.Lerp(active ? 0.0f : maxVolume, active ? maxVolume : 0.0f, time / 0.5f);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        activeSource.volume = active ? maxVolume : 0.0f;

        crossFading = false;
    }

    IEnumerator CrossFade(AudioSource current, AudioSource auxilary)
    {
        float time = 0.0f;

        crossFading = true;

        while (time < 0.5f)
        {
            current.volume = Mathf.Lerp(maxVolume, 0.0f, time / 0.5f);
            auxilary.volume = Mathf.Lerp(0.0f, maxVolume, time / 0.5f);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        current.volume = 0.0f;
        auxilary.volume = maxVolume;

        yield return new WaitForSeconds(1.0f);

        crossFading = false;
    }
}

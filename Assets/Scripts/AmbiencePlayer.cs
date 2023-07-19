using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbiencePlayer : MonoBehaviour
{
    public AudioClip[] randomSounds;
    public float volume;
    public float minTime;
    public float maxTime;

    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.volume = volume;

        StartCoroutine(PlaySounds());
    }

    IEnumerator PlaySounds()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
            source.clip = randomSounds[Random.Range(0, randomSounds.Length)];
            source.Play();
        }
    }
}

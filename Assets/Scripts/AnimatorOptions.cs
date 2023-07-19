using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorOptions : MonoBehaviour
{
    public GameObject triggerGameobject;
    public AudioClip[] randomSound;
    public float volume = 0.5f;
    Animator anim;
    AudioSource audioObject;

    void Start()
    {
        anim = GetComponent<Animator>();
        audioObject = GetComponent<AudioSource>();
        if (audioObject == null)
        {
            audioObject = gameObject.AddComponent<AudioSource>();
            audioObject.volume = volume;
        }
    }

    void ActivateBool(string param)
    {
        anim.SetBool(param, true);
    }
    
    void DeactivateBool(string param)
    {
        anim.SetBool(param, false);
    }

    public void ToggleGameObject(int active)
    {
        triggerGameobject.SetActive(active > 0);
    }

    public void PlaySound(Object clip)
    {
        audioObject.PlayOneShot(clip as AudioClip);
    }

    public void PlayRandomSound()
    {
        audioObject.PlayOneShot(randomSound[Random.Range(0, randomSound.Length)]);
    }

    public void SpawnObject(Object gameObject)
    {
        Instantiate(gameObject as GameObject, transform);
    }

    public void PlayControlledSound(Object audio)
    {
        if (!audioObject.isPlaying)
        {
            audioObject.clip = audio as AudioClip;
            audioObject.loop = true;
            audioObject.Play();
        }
    }

    public void StopControlledSound()
    {
        if (audioObject.isPlaying)
            audioObject.Stop();
    }
}

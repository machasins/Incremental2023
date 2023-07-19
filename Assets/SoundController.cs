using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    public static SoundController sound;
    public Slider slider;

    float volume;

    // Start is called before the first frame update
    void Start()
    {
        AudioListener.volume = 1.0f;

        if (sound)
            Destroy(gameObject);
        else
        {
            sound = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Update()
    {
        ChangeVolume();
        AudioListener.volume = volume;
    }

    public void ChangeVolume()
    {
        if (slider)
            volume = slider.value;
    }
}

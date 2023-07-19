using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class FinalVideoPlayer : MonoBehaviour
{
    public string videoName;
    public Fade fade;
    // Start is called before the first frame update
    void Start()
    {
        VideoPlayer vp = GetComponent<VideoPlayer>();
        vp.url = System.IO.Path.Combine(Application.streamingAssetsPath, videoName);
        vp.loopPointReached += BackToMenu;
        vp.Play();
    }

    // Update is called once per frame
    void BackToMenu(VideoPlayer vp)
    {
        fade.SwitchScene("MainMenu");
    }
}

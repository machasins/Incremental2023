using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class StreamController : MonoBehaviour
{
    public Transform streamPrefab;
    public string idleFile;
    public string interactFile;
    public string donateFile;

    private VideoPlayer[] videos;
    private bool[] doEarly;
    private int nextVideo = -1;

    private VideoPlayer activePlayer;
    private VideoPlayer waitingPlayer;

    // Start is called before the first frame update
    void Awake()
    {
        string[] files = { idleFile, interactFile, donateFile };
        videos = new VideoPlayer[files.Length];
        doEarly = new bool[files.Length];
        for (int i = 0; i < files.Length; ++i)
        {
            if (!files[i].Equals(""))
            {
                Transform stream = Instantiate(streamPrefab, transform, false);
                videos[i] = stream.GetComponent<VideoPlayer>();
                videos[i].url = System.IO.Path.Combine(Application.streamingAssetsPath, files[i]);
                videos[i].loopPointReached += EndOfClipReached;
                videos[i].playOnAwake = false;

                videos[i].Prepare();
                videos[i].GetComponent<SpriteRenderer>().enabled = false;
            }

            doEarly[i] = true;
        }

        doEarly[0] = false;
    }

    void Update()
    {
        // The below code starts the playback of the waiting player early; when the active player is almost finished with its segment.
        // This gives the waiting player time to "play out" the first few "black" frames it starts out with and get to the actual video.
        // The waiting player will become visible (and active) when the current, active player completely finishes playing.
        if (activePlayer.isPlaying && nextVideo != -1 && doEarly[nextVideo])
        {
            int frameCount = 2;
            
            #if UNITY_EDITOR
            frameCount = 3;
            #endif
            
            float framesInSeconds = frameCount / activePlayer.frameRate;

            if (activePlayer.time >= activePlayer.length - framesInSeconds)
            {
                doEarly[nextVideo] = false;
                videos[nextVideo].Play();
            }
        }
    }

    void OnEnable()
    {
        videos[0].GetComponent<SpriteRenderer>().enabled = true;
        videos[0].Play();

        activePlayer = videos[0];
    }

    void OnDisable()
    {
        foreach (VideoPlayer v in videos)
        {
            if (v)
            {
                v.Pause();
                v.frame = 0;

                v.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }    

    void EndOfClipReached(VideoPlayer vp)
    {
        if ((nextVideo != -1 && vp == videos[nextVideo]) || (vp == videos[0] && nextVideo == -1))
            vp.Play();
        else
        {
            vp.GetComponent<SpriteRenderer>().enabled = false;
            vp.Play();
            vp.Pause();

            if (nextVideo == -1)
            {
                videos[0].GetComponent<SpriteRenderer>().enabled = true;
                videos[0].Play();

                activePlayer = videos[0];
            }
            else
            {
                videos[nextVideo].GetComponent<SpriteRenderer>().enabled = true;
                videos[nextVideo].Play();

                activePlayer = videos[nextVideo];
            }

        }
        
        nextVideo = -1;
    }

    public void Interact()
    {
        if (nextVideo == -1)
        {
            nextVideo = 1;
        }
    }

    public void Donate()
    {
        nextVideo = 2;
    }
}

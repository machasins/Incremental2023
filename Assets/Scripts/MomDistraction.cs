using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomDistraction : MonoBehaviour
{
    public float interval;
    public float intervalVariance;
    public float stuckTime;

    public CameraController cam;
    public Transform moveBoxes;
    
    public AudioClip walkingSound;
    public float prewalkTime;

    public Transform stuckPosition;
    public GameObject mom;

    private AudioSource audioObject;
    private bool preWalked = false;

    private float time;
    private float intervalTime;
    private bool stuck;

    // Start is called before the first frame update
    void Start()
    {
        intervalTime = interval + Random.Range(-intervalVariance, intervalVariance);
        mom.SetActive(false);

        audioObject = FindFirstObjectByType<PlayerData>().GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!stuck)
        {
            if (time >= intervalTime - prewalkTime && !preWalked)
            {
                audioObject.PlayOneShot(walkingSound);
                preWalked = true;
            }
            if (time >= intervalTime)
                EnterStuck();
            time += Time.fixedDeltaTime;
        }
        else
        {
            if (time >= stuckTime)
                ExitStuck();
            time += Time.fixedDeltaTime;
        }
    }

    void EnterStuck()
    {
        if (!cam.isZooming)
        {
            stuck = true;
            time = 0.0f;

            mom.SetActive(true);

            cam.GetStuck();

            moveBoxes.gameObject.SetActive(false);

            transform.localPosition = stuckPosition.localPosition;
        }
    }

    public void ExitStuck()
    {
        if (stuck)
        {
            stuck = false;
            time = 0.0f;

            preWalked = false;

            mom.GetComponent<Animator>().SetBool("closeDoor", true);
            mom.GetComponent<AnimatorOptions>().StopControlledSound();

            cam.Unstuck();

            moveBoxes.gameObject.SetActive(true);

            intervalTime = interval + Random.Range(-intervalVariance, intervalVariance);
        }
    }
}

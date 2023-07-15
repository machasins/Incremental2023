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

    public Transform stuckPosition;
    public GameObject mom;

    private float time;
    private float intervalTime;
    private bool stuck;

    // Start is called before the first frame update
    void Start()
    {
        intervalTime = interval + Random.Range(-intervalVariance, intervalVariance);
        mom.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!stuck)
        {
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
        stuck = true;
        time = 0.0f;

        mom.SetActive(true);

        cam.GetStuck();

        moveBoxes.gameObject.SetActive(false);

        transform.localPosition = stuckPosition.localPosition;
    }

    public void ExitStuck()
    {
        if (stuck)
        {
            stuck = false;
            time = 0.0f;

            mom.SetActive(false);

            cam.Unstuck();

            moveBoxes.gameObject.SetActive(true);

            intervalTime = interval + Random.Range(-intervalVariance, intervalVariance);
        }
    }
}

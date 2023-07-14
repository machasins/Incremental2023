using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitchCoordinator : MonoBehaviour
{
    enum State
    {
        girl,
        guy,
        wait
    }

    public GameObject girlStream;
    public GameObject guyStream;
    public GameObject waitingScreen;
    public float girlStreamChance;
    public float girlStreamTime;
    public float girlStreamTimeVariance;
    public float guyStreamTime;
    public float guyStreamTimeVariance;
    public float waitingTime;
    public float waitingTimeVariance;

    private State state;
    private float currentTime = 0.0f;

    private float time = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        girlStream.SetActive(true);
        guyStream.SetActive(false);
        waitingScreen.SetActive(false);

        currentTime = girlStreamTime + Random.Range(-girlStreamTimeVariance, girlStreamTimeVariance);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        if (time >= currentTime)
        {
            time = 0.0f;
            ChangeStates();
        }
    }

    void ChangeStates()
    {
        if (state == State.girl || state == State.guy)
        {
            state = State.wait;

            girlStream.SetActive(false);
            guyStream.SetActive(false);
            waitingScreen.SetActive(true);

            currentTime = waitingTime + Random.Range(-waitingTimeVariance, waitingTimeVariance);
        }
        else if (state == State.wait)
        {
            waitingScreen.SetActive(false);
            if (Random.value <= girlStreamChance)
            {
                state = State.girl;

                girlStream.SetActive(true);
                guyStream.SetActive(false);

                currentTime = girlStreamTime + Random.Range(-girlStreamTimeVariance, girlStreamTimeVariance);
            }
            else
            {
                state = State.guy;

                girlStream.SetActive(false);
                guyStream.SetActive(true);

                currentTime = guyStreamTime + Random.Range(-guyStreamTimeVariance, guyStreamTimeVariance);
            }
        }
    }
}

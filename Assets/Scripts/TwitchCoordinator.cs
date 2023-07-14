using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitchCoordinator : MonoBehaviour
{
    public enum State
    {
        girl,
        guy,
        wait
    }

    public ServerHandler server;
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

    [HideInInspector] public State state;
    private float currentTime = 0.0f;

    private float time = 0.0f;

    private float streamerMessageTimeMult;
    private float streamerUserTimeMult;
    private float streamerBannableRateMult;

    // Start is called before the first frame update
    void Start()
    {
        girlStream.SetActive(true);
        guyStream.SetActive(false);
        waitingScreen.SetActive(false);

        streamerMessageTimeMult = 1.0f;
        streamerUserTimeMult = 1.0f;
        streamerBannableRateMult = 1.0f;

        server.streamerMessageTimeMult = 1.0f;
        server.streamerUserTimeMult = 1.0f;
        server.streamerBannableRateMult = 1.0f;

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

            server.streamerMessageTimeMult = 1.0f;
            server.streamerUserTimeMult = 1.0f;
            server.streamerBannableRateMult = 1.0f;

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

                server.streamerMessageTimeMult = streamerMessageTimeMult;
                server.streamerUserTimeMult = streamerUserTimeMult;
                server.streamerBannableRateMult = 1.0f;

                currentTime = girlStreamTime + Random.Range(-girlStreamTimeVariance, girlStreamTimeVariance);
            }
            else
            {
                state = State.guy;

                girlStream.SetActive(false);
                guyStream.SetActive(true);
                
                server.streamerMessageTimeMult = 1.0f;
                server.streamerUserTimeMult = 1.0f;
                server.streamerBannableRateMult = streamerBannableRateMult;

                currentTime = guyStreamTime + Random.Range(-guyStreamTimeVariance, guyStreamTimeVariance);
            }
        }
    }

    public void ImproveWaitingTime(float amount)
    {
        waitingTime *= amount;
        waitingTimeVariance *= amount;
    }

    public void ImproveGirlStreamTime(float amount)
    {
        girlStreamTime *= amount;
        girlStreamTimeVariance *= amount;
    }

    public void ImproveGuyStreamTime(float amount)
    {
        guyStreamTime *= amount;
        guyStreamTimeVariance *= amount;
    }

    public void ImproveMessageTimeWithStream(float amount)
    {
        streamerMessageTimeMult *= amount;
    }

    public void ImproveUserTimeWithStream(float amount)
    {
        streamerUserTimeMult *= amount;
    }

    public void ImproveBannableRateWithStream(float amount)
    {
        streamerBannableRateMult *= amount;
    }
}

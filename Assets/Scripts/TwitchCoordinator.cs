using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitchCoordinator : MonoBehaviour
{
    public ServerHandler server;

    public GameObject normalParent;
    public GameObject miniParent;

    public GameObject girlStream;
    public GameObject girlOfflineScreen;
    public GameObject[] girlAuxiliary;
    public GameObject donateButton;
    public GameObject guyStream;
    public GameObject guyOfflineScreen;
    public GameObject[] guyAuxiliary;
    
    public float girlStreamTime;
    public float girlStreamTimeVariance;
    public float girlOfflineTime;
    public float girlOfflineTimeVariance;
    public float guyStreamTime;
    public float guyStreamTimeVariance;
    public float guyOfflineTime;
    public float guyOfflineTimeVariance;

    [HideInInspector] public bool girlOnline;
    [HideInInspector] public bool guyOnline;
    [HideInInspector] public bool girlActive;

    private float currentGirlTime = 0.0f;
    private float currentGuyTime = 0.0f;

    private float girlTime = 0.0f;
    private float guyTime = 0.0f;

    private float streamerMessageTimeMult;
    private float streamerUserTimeMult;
    private float streamerBannableRateMult;

    // Start is called before the first frame update
    void Start()
    {
        girlActive = true;

        girlOnline = true;
        GirlSetEnabled(girlOnline, girlActive);

        guyOnline = false;
        GuySetEnabled(guyOnline, !girlActive);

        streamerMessageTimeMult = 1.0f;
        streamerUserTimeMult = 1.0f;
        streamerBannableRateMult = 1.0f;

        server.streamerMessageTimeMult = 1.0f;
        server.streamerUserTimeMult = 1.0f;
        server.streamerBannableRateMult = 1.0f;

        currentGirlTime = girlStreamTime + Random.Range(-girlStreamTimeVariance, girlStreamTimeVariance);
        currentGuyTime = Random.Range(girlStreamTime / 2.0f, girlStreamTime / 1.25f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        girlTime += Time.fixedDeltaTime;
        if (girlTime >= currentGirlTime)
        {
            girlTime = 0.0f;
            if (girlOnline)
            {
                girlOnline = false;

                GirlSetEnabled(girlOnline, girlActive);

                server.streamerMessageTimeMult = 1.0f;
                server.streamerUserTimeMult = 1.0f;

                currentGirlTime = girlOfflineTime + Random.Range(-girlOfflineTimeVariance, girlOfflineTimeVariance);
            }
            else
            {
                girlOnline = true;
                
                GirlSetEnabled(girlOnline, girlActive);

                server.streamerMessageTimeMult = streamerMessageTimeMult;
                server.streamerUserTimeMult = streamerUserTimeMult;

                currentGirlTime = girlStreamTime + Random.Range(-girlStreamTimeVariance, girlStreamTimeVariance);
            }
        }

        guyTime += Time.fixedDeltaTime;
        if (guyTime >= currentGuyTime)
        {
            guyTime = 0.0f;
            if (guyOnline)
            {
                guyOnline = false;
                
                GuySetEnabled(guyOnline, !girlActive);

                server.streamerBannableRateMult = 1.0f;

                currentGuyTime = guyOfflineTime + Random.Range(-guyOfflineTimeVariance, guyOfflineTimeVariance);
            }
            else
            {
                guyOnline = true;
                
                GuySetEnabled(guyOnline, !girlActive);

                server.streamerBannableRateMult = streamerBannableRateMult;

                currentGuyTime = guyStreamTime + Random.Range(-guyStreamTimeVariance, guyStreamTimeVariance);
            }
        }
    }

    void GirlSetEnabled(bool isOnline, bool isActive)
    {
        if (!isActive && girlStream.transform.parent.parent != miniParent.transform)
            girlStream.transform.parent.SetParent(miniParent.transform, false);
        else if (isActive && girlStream.transform.parent.parent != normalParent.transform)
            girlStream.transform.parent.SetParent(normalParent.transform, false);

        girlStream.GetComponent<StreamController>().SetActive(isOnline);
        girlOfflineScreen.SetActive(!isOnline);
        
        foreach(GameObject g in girlAuxiliary)
            g.SetActive(isOnline);
        
        donateButton.SetActive(isOnline && isActive);
    }

    void GuySetEnabled(bool isOnline, bool isActive)
    {
        if (!isActive && guyStream.transform.parent.parent != miniParent.transform)
            guyStream.transform.parent.SetParent(miniParent.transform, false);
        else if (isActive && guyStream.transform.parent.parent != normalParent.transform)
            guyStream.transform.parent.SetParent(normalParent.transform, false);

        guyStream.GetComponent<StreamController>().SetActive(isOnline);
        guyOfflineScreen.SetActive(!isOnline);
        
        foreach(GameObject g in guyAuxiliary)
            g.SetActive(isOnline);
    }

    public void SwitchStreams()
    {
        girlActive = !girlActive;
        GirlSetEnabled(girlOnline, girlActive);
        GuySetEnabled(guyOnline, !girlActive);
    }

    public void ImproveWaitingTime(float amount)
    {
        girlOfflineTime *= amount;
        girlOfflineTimeVariance *= amount;

        guyOfflineTime *= amount;
        guyOfflineTimeVariance *= amount;
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

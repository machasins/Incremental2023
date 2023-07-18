using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitchCoordinator : MonoBehaviour
{
    public ServerHandler server;

    public GameObject normalParent;
    public GameObject miniParent;

    public GameObject girlStream;
    public GameObject donateButton;
    public GameObject guyStream;

    [HideInInspector] public bool girlOnline;
    [HideInInspector] public bool guyOnline;
    [HideInInspector] public bool girlActive;

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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    void GirlSetEnabled(bool isOnline, bool isActive)
    {
        if (!isActive && girlStream.transform.parent.parent != miniParent.transform)
            girlStream.transform.parent.SetParent(miniParent.transform, false);
        else if (isActive && girlStream.transform.parent.parent != normalParent.transform)
            girlStream.transform.parent.SetParent(normalParent.transform, false);
        
        donateButton.SetActive(isOnline && isActive);
    }

    void GuySetEnabled(bool isOnline, bool isActive)
    {
        if (!isActive && guyStream.transform.parent.parent != miniParent.transform)
            guyStream.transform.parent.SetParent(miniParent.transform, false);
        else if (isActive && guyStream.transform.parent.parent != normalParent.transform)
            guyStream.transform.parent.SetParent(normalParent.transform, false);
    }

    public void SwitchStreams()
    {
        girlActive = !girlActive;
        GirlSetEnabled(girlOnline, girlActive);
        GuySetEnabled(guyOnline, !girlActive);
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

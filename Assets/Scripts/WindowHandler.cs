using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowHandler : MonoBehaviour
{
    public Transform desktop;
    public Transform discord;
    public Transform website;

    void Start()
    {
        desktop.gameObject.SetActive(false);
        website.gameObject.SetActive(false);

        discord.gameObject.SetActive(true);
    }

    public void OnClickWebsite()
    {
        if (website.gameObject.activeInHierarchy)
            OnClickDesktop();
        else
        {
            desktop.gameObject.SetActive(false);
            discord.gameObject.SetActive(false);

            website.gameObject.SetActive(true);
        }
    }

    public void OnClickDiscord()
    {
        if (discord.gameObject.activeInHierarchy)
            OnClickDesktop();
        else
        {
            desktop.gameObject.SetActive(false);
            website.gameObject.SetActive(false);

            discord.gameObject.SetActive(true);
        }
    }

    public void OnClickDesktop()
    {
        discord.gameObject.SetActive(false);
        website.gameObject.SetActive(false);

        desktop.gameObject.SetActive(true);
    }
}

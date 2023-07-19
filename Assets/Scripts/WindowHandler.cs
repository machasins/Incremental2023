using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowHandler : MonoBehaviour
{
    public ServerSwitcher serverSwitcher;

    public Transform desktop;
    public Transform discord;
    public Transform website;

    public GameObject discordSelected;
    public GameObject discordNotification;
    public GameObject websiteSelected;

    public AudioClip notifSound;
    public float minNotifTime;
    public GameObject activate;

    private bool ableToNotif;
    private float notifCooldown;

    private AudioSource audioObject;

    void Start()
    {
        desktop.gameObject.SetActive(false);
        website.gameObject.SetActive(false);
        websiteSelected.SetActive(false);

        discord.gameObject.SetActive(true);
        discordSelected.SetActive(true);
        discordNotification.SetActive(false);
        
        serverSwitcher.girlServer.GetComponentInChildren<MessageSpawner>().newMessage += OnRecieveMessage;
        serverSwitcher.dmServer.GetComponentInChildren<MessageSpawner>().newMessage += OnRecieveMessage;

        audioObject = FindFirstObjectByType<PlayerData>().GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        notifCooldown += Time.fixedDeltaTime;
        if (notifCooldown >= minNotifTime)
            ableToNotif = true;
    }

    public void OnClickWebsite()
    {
        if (website.gameObject.activeInHierarchy)
            OnClickDesktop();
        else
        {
            desktop.gameObject.SetActive(false);
            discord.gameObject.SetActive(false);
            discordSelected.SetActive(false);

            website.gameObject.SetActive(true);
            websiteSelected.SetActive(true);
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
            websiteSelected.SetActive(false);

            discord.gameObject.SetActive(true);
            discordSelected.SetActive(true);
            discordNotification.SetActive(false);
        }
    }

    public void OnClickDesktop()
    {
        discord.gameObject.SetActive(false);
        discordSelected.SetActive(false);
        website.gameObject.SetActive(false);
        websiteSelected.SetActive(false);

        desktop.gameObject.SetActive(true);
    }

    public void OnRecieveMessage(MessageCreator m)
    {
        if (!discord.gameObject.activeInHierarchy)
            discordNotification.SetActive(true);
        if (ableToNotif && (!discord.gameObject.activeInHierarchy || activate.activeInHierarchy))
        {
            ableToNotif = false;
            notifCooldown = 0.0f;
            audioObject.PlayOneShot(notifSound);
        }
    }
}

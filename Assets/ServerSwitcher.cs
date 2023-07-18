using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ServerSwitcher : MonoBehaviour
{
    public Transform girlServer;
    public Transform dmServer;

    public TMP_Text girlNotif;
    public TMP_Text dmNotif;

    private int girlNotifNum = 0;
    private int dmNotifNum = 0;

    void Start()
    {
        girlServer.gameObject.SetActive(false);
        dmServer.gameObject.SetActive(true);

        girlNotif.transform.parent.gameObject.SetActive(girlNotifNum > 0);
        dmNotif.transform.parent.gameObject.SetActive(dmNotifNum > 0);

        girlNotif.text = girlNotifNum.ToString();
        dmNotif.text = dmNotifNum.ToString();

        girlServer.GetComponentInChildren<MessageSpawner>().newMessage += RecievedMessageGirl;
        dmServer.GetComponentInChildren<MessageSpawner>().newMessage += RecievedMessageDM;
    }

    void FixedUpdate()
    {
        girlNotifNum = girlServer.gameObject.activeInHierarchy ? 0 : girlNotifNum;
        dmNotifNum = dmServer.gameObject.activeInHierarchy ? 0 : dmNotifNum;

        girlNotif.transform.parent.gameObject.SetActive(girlNotifNum > 0);
        dmNotif.transform.parent.gameObject.SetActive(dmNotifNum > 0);

        girlNotif.text = girlNotifNum.ToString();
        dmNotif.text = dmNotifNum.ToString();
    }

    public void ActivateGirlServer()
    {
        girlServer.gameObject.SetActive(true);
        dmServer.gameObject.SetActive(false);

        girlNotifNum = 0;
    }

    public void ActivateDMServer()
    {
        girlServer.gameObject.SetActive(false);
        dmServer.gameObject.SetActive(true);

        dmNotifNum = 0;
    }

    void RecievedMessageDM(MessageCreator m)
    {
        if (!dmServer.gameObject.activeInHierarchy)
            dmNotifNum++;
    }

    void RecievedMessageGirl(MessageCreator m)
    {
        if (!girlServer.gameObject.activeInHierarchy)
            girlNotifNum++;
    }
}

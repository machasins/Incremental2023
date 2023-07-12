using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageSpawner : MonoBehaviour
{
    public Scrollbar scrollbar;
    public float topHeight;

    public Transform messagePrefab;
    public int messagePoolAmount;

    private List<MessageCreator> messagePool;
    private MessageCreator lastMessage;
    private UserData userData;
    private int messageIndex = 0;
    [HideInInspector] public float totalHeight = 0.0f;
    private float additionalHeight = 0.0f;
    private float historyHeight = 0.0f;
    private bool isScrolling = false;
    private Vector3 startingPosition;

    void Start()
    {
        userData = GetComponent<UserData>();
        startingPosition = transform.localPosition;

        InstantiateMessagePool();

        StartCoroutine(loop());
    }

    void InstantiateMessagePool()
    {
        messagePool = new List<MessageCreator>();
        for (int i = 0; i < messagePoolAmount; ++i)
        {
            Transform m = Instantiate(messagePrefab, transform);
            m.gameObject.SetActive(false);
            messagePool.Add(m.GetComponent<MessageCreator>());
        }
    }

    public void OnScroll()
    {
        if (scrollbar.value > 0.0f)
        {
            isScrolling = true;
            transform.localPosition = Vector3.Lerp(startingPosition + Vector3.up * totalHeight, startingPosition + Vector3.up * (topHeight + additionalHeight), scrollbar.value);
        }
        else
        {
            isScrolling = false;
            transform.localPosition = startingPosition + Vector3.up * totalHeight;
        }
    }

    MessageCreator GetMessageFromPool()
    {
        MessageCreator ret = messagePool[messageIndex];

        if (ret.gameObject.activeInHierarchy)
        {
            historyHeight -= ret.messageHeight;
            additionalHeight += ret.messageHeight;
        }

        ret.transform.localPosition = Vector3.zero;

        messageIndex = (messageIndex + 1) % messagePoolAmount;

        return ret;
    }

    void AddMessage(Sprite icon, Color userColor, string username, string message)
    {
        if (lastMessage && string.Equals(username, lastMessage.user))
        {
            totalHeight -= lastMessage.messageHeight;
            historyHeight -= lastMessage.messageHeight;
            lastMessage.Append(message);
            totalHeight += lastMessage.messageHeight;
            historyHeight += lastMessage.messageHeight;
        }
        else
        {
            MessageCreator mc = GetMessageFromPool();
            mc.gameObject.SetActive(true);
            mc.Create(icon, userColor, username, message, GetTime());

            mc.transform.localPosition = Vector3.down * totalHeight;
            totalHeight += mc.messageHeight;
            historyHeight += mc.messageHeight;

            lastMessage = mc;
        }

        if (!isScrolling)
            transform.localPosition = startingPosition + Vector3.up * totalHeight;
    }

    void AddMessage(Sprite icon, Color userColor, string username, Sprite message)
    {
        MessageCreator mc = GetMessageFromPool();
        mc.gameObject.SetActive(true);
        mc.Create(icon, userColor, username, message, GetTime());

        mc.transform.localPosition = Vector3.down * totalHeight;
        totalHeight += mc.messageHeight;
        historyHeight += mc.messageHeight;

        lastMessage = null;

        if (!isScrolling)
            transform.localPosition = startingPosition + Vector3.up * totalHeight;
    }

    string GetTime()
    {
        const int timescale = 2;
        int time = (int)(Time.fixedTime % (1440 * timescale) / timescale);
        string hours = (((time / 60) % 12 == 0) ? 12 : (time / 60) % 12).ToString();
        string minutes = (time % 60).ToString("D2");
        string ampm = (time > 719) ? "PM" : "AM";
        string formattedTime = hours + ":" + minutes + " " + ampm;

        return formattedTime;
    }

    void AddMessages(int amount)
    {
        for (int i = 0; i < amount; ++i)
        {
            User u = userData.GetUser();
            if (Random.value < 0.1f)
                AddMessage(u.userIcon, u.userColor, u.username, userData.GetImageMessage(u.type));
            else
                AddMessage(u.userIcon, u.userColor, u.username, userData.GetMessage(u.type));
        }
    }

    IEnumerator loop()
    {
        userData.AddUser(20);
        //AddMessages(messagePoolAmount);
        int count = 0;
        while (count < 150)
        {
            AddMessages(1);
            yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
            count++;
        }

    }
}

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
    private float staticHeight = 0.0f;
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

    void MoveList()
    {
        float precompScroll = Mathf.Lerp(0.0f, totalHeight - topHeight, scrollbar.value);
        float previousHeight = totalHeight;

        totalHeight = 0.0f;
        for (int i = messagePoolAmount - 1; i >= 0; --i)
        {
            int index = (i + messageIndex) % messagePoolAmount;
            if (messagePool[index].gameObject.activeInHierarchy)
                totalHeight += messagePool[index].messageHeight;
        }

        scrollbar.value += (totalHeight - previousHeight) / (totalHeight - topHeight);

        float currentHeight = 0.0f;
        float modifier = -Mathf.Lerp(0.0f, totalHeight - topHeight, scrollbar.value);

        for (int i = messagePoolAmount - 1; i >= 0; --i)
        {
            int index = (i + messageIndex) % messagePoolAmount;
            if (messagePool[index].gameObject.activeInHierarchy)
            {
                currentHeight += messagePool[index].messageHeight;
                messagePool[index].transform.localPosition = Vector3.up * (currentHeight + modifier);
            }
        }
    }

    public void OnScroll()
    {
        MoveList();
    }

    MessageCreator GetMessageFromPool()
    {
        MessageCreator ret = messagePool[messageIndex];
        ret.Create(null, Color.black, "", "", "");
        ret.transform.localPosition = Vector3.zero;

        messageIndex = (messageIndex + 1) % messagePoolAmount;

        return ret;
    }

    void AddMessage(Sprite icon, Color userColor, string username, string message, bool update = true)
    {
        if (lastMessage && string.Equals(username, lastMessage.user))
            lastMessage.Append(message);
        else
        {
            MessageCreator mc = GetMessageFromPool();
            mc.gameObject.SetActive(true);
            mc.Create(icon, userColor, username, message, GetTime());
            lastMessage = mc;
        }
        if (update)
            MoveList();
    }

    void AddMessage(Sprite icon, Color userColor, string username, Sprite message, bool update = true)
    {
        MessageCreator mc = GetMessageFromPool();
        mc.gameObject.SetActive(true);
        mc.Create(icon, userColor, username, message, GetTime());
        lastMessage = null;

        if (update)
            MoveList();
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
                AddMessage(u.userIcon, u.userColor, u.username, userData.GetImageMessage(u.type), false);
            else
                AddMessage(u.userIcon, u.userColor, u.username, userData.GetMessage(u.type), false);
        }

        MoveList();
    }

    IEnumerator loop()
    {
        userData.AddUser(20);
        AddMessages(messagePoolAmount);
        int count = 0;
        while (count < 150)
        {
            AddMessages(1);
            yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
            count++;
        }

    }
}

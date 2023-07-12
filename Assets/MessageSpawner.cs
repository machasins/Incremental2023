using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MessageSpawner : MonoBehaviour
{
    public Scrollbar scrollbar;
    public float topHeight;
    public GameObject rightClickMenu;

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
        rightClickMenu.SetActive(false);

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

    public void OnRightClick(Sprite icon, Color userColor, string username, MessageCreator message)
    {
        rightClickMenu.SetActive(true);

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 20;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = rightClickMenu.transform.localPosition.z;
        rightClickMenu.transform.localPosition = mousePos;

        rightClickMenu.GetComponent<BanMenuSetup>().Create(rightClickMenu.transform.localPosition, icon, userColor, username);
    }

    MessageCreator GetMessageFromPool()
    {
        MessageCreator ret = messagePool[messageIndex];

        if (ret.gameObject.activeInHierarchy)
        {
            historyHeight -= ret.height;
            additionalHeight += ret.height;
        }

        ret.transform.localPosition = Vector3.zero;

        messageIndex = (messageIndex + 1) % messagePoolAmount;

        return ret;
    }

    void AddMessage(Sprite icon, Color userColor, string username, string message)
    {
        if (lastMessage && string.Equals(username, lastMessage.user))
        {
            totalHeight -= lastMessage.height;
            historyHeight -= lastMessage.height;
            lastMessage.Append(message);
            totalHeight += lastMessage.height;
            historyHeight += lastMessage.height;
        }
        else
        {
            MessageCreator mc = GetMessageFromPool();
            mc.gameObject.SetActive(true);
            mc.Create(icon, userColor, username, message, GetTime());

            mc.transform.localPosition = Vector3.down * totalHeight;
            totalHeight += mc.height;
            historyHeight += mc.height;

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
        totalHeight += mc.height;
        historyHeight += mc.height;

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

    void AddBannableMessages(int amount)
    {
        for (int i = 0; i < amount; ++i)
        {
            User u = userData.GetUser();
            if (Random.value < 0.1f)
                AddMessage(u.userIcon, u.userColor, u.username, userData.GetBannableImageMessage(u.type));
            else
                AddMessage(u.userIcon, u.userColor, u.username, userData.GetBannableMessage(u.type));
        }
    }

    IEnumerator loop()
    {
        userData.AddUser(20);
        AddMessages(messagePoolAmount);
        int count = 0;
        while (count < 150)
        {
            if (count % 5 == 0)
                AddBannableMessages(1);
            else
                AddMessages(1);
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
            count++;
        }

    }
}

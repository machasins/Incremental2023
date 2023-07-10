using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageSpawner : MonoBehaviour
{
    public Transform messagePrefab;
    public int messagePoolAmount;
    private List<MessageCreator> messagePool;
    private MessageCreator lastMessage;
    private int messageIndex = 0;

    void Start()
    {
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
        float currentHeight = 0.0f;
        for (int i = messagePoolAmount - 1; i >= 0; --i)
        {
            int index = (i + messageIndex) % messagePoolAmount;
            if (messagePool[index].gameObject.activeInHierarchy)
            {
                currentHeight += messagePool[index].messageHeight;
                messagePool[index].transform.localPosition = Vector3.up * currentHeight;
            }
        }
    }

    MessageCreator GetMessageFromPool()
    {
        MessageCreator ret = messagePool[messageIndex];
        ret.Create(null, Color.black, "", "", "");
        ret.transform.localPosition = Vector3.zero;

        messageIndex = (messageIndex + 1) % messagePoolAmount;

        return ret;
    }

    void AddMessage(Sprite icon, Color userColor, string username, string message)
    {
        if (lastMessage && string.Equals(username, lastMessage.user))
            lastMessage.Append(message);
        else
        {
            MessageCreator mc = GetMessageFromPool();
            mc.gameObject.SetActive(true);
            mc.Create(null, Color.cyan, username, message, "12:59 PM");
            lastMessage = mc;
        }
        MoveList();
    }

    IEnumerator loop()
    {
        string note = "ha ";
        int count = 0;
        while (count < 75)
        {
            AddMessage(null, Color.cyan, count % 4 == 0 ? "spammerIRL1995" : "ffsmyballsache222", note);
            yield return new WaitForSeconds(0.25f);
            count++;
            note += "ha ";
        }

    }
}

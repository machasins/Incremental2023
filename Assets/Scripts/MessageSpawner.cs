using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MessageSpawner : MonoBehaviour
{
    public Scrollbar scrollbar;
    public float scrollAmount;
    public float topHeight;
    public GameObject rightClickMenu;
    public PlayerData player;
    public UserData userData;

    public Transform messagePrefab;
    public int messagePoolAmount;

    public int maximumConsecutiveNormalMessages = 10;

    [HideInInspector] public List<MessageCreator> messagePool;
    public delegate void PingNewMessage(MessageCreator m);
    [HideInInspector] public PingNewMessage newMessage;

    private BotHandler bots;

    private MessageCreator lastMessage;
    private int messageIndex = 0;
    [HideInInspector] public float totalHeight = 0.0f;
    private float additionalHeight = 0.0f;
    private float historyHeight = 0.0f;
    private bool isScrolling = false;
    private Vector3 startingPosition;
    private MessageCreator clickedMessage;
    private int lastBannableMessage = 0;

    void Start()
    {
        bots = player.GetComponent<BotHandler>();

        startingPosition = transform.localPosition;
        rightClickMenu.SetActive(false);

        InstantiateMessagePool();
    }

    void Update()
    {
        Vector2 scrollValue = Mouse.current.scroll.ReadValue().normalized;
        Vector2 limits = new Vector2(topHeight + additionalHeight, totalHeight);
        scrollbar.value = Mathf.Clamp01(scrollbar.value + scrollAmount / (limits.y - limits.x) * scrollValue.y);
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

    public void OnRightClick(User user, MessageCreator message)
    {
        rightClickMenu.SetActive(true);

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 20;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = rightClickMenu.transform.localPosition.z;
        rightClickMenu.transform.localPosition = mousePos;

        rightClickMenu.GetComponent<BanMenuSetup>().Create(rightClickMenu.transform.localPosition, user, message.bannable, this);

        clickedMessage = message;
    }

    MessageCreator GetMessageFromPool()
    {
        MessageCreator ret = messagePool[messageIndex];

        if (ret.gameObject.activeInHierarchy)
        {
            historyHeight -= ret.height;
            additionalHeight += ret.height;

            if (ret.bannable && bots.CleanupSuccess())
            {
                clickedMessage = ret;
                clickedMessage.bannable = false;
                BanUser();

                player.AddMoney(player.baseBanMoney * bots.cleanupBotPercentMoneyPerBan);
            }
        }

        ret.transform.localPosition = Vector3.zero;

        messageIndex = (messageIndex + 1) % messagePoolAmount;

        return ret;
    }

    public void AddMessage(User user, string message, bool bannable = false)
    {
        if (lastMessage && string.Equals(user.username, lastMessage.user.username))
        {
            totalHeight -= lastMessage.height;
            historyHeight -= lastMessage.height;
            lastMessage.Append(message, bannable);
            totalHeight += lastMessage.height;
            historyHeight += lastMessage.height;
        }
        else
        {
            MessageCreator mc = GetMessageFromPool();
            mc.gameObject.SetActive(true);
            mc.Create(user, message, GetTime(), bannable);

            mc.transform.localPosition = Vector3.down * totalHeight;
            totalHeight += mc.height;
            historyHeight += mc.height;

            lastMessage = mc;
        }

        if (!isScrolling)
            transform.localPosition = startingPosition + Vector3.up * totalHeight;

        if (newMessage != null) newMessage(lastMessage);
    }

    public void AddMessage(User user, Sprite message, bool bannable = false)
    {
        MessageCreator mc = GetMessageFromPool();
        mc.gameObject.SetActive(true);
        mc.Create(user, message, GetTime(), bannable);

        mc.transform.localPosition = Vector3.down * totalHeight;
        totalHeight += mc.height;
        historyHeight += mc.height;

        lastMessage = null;

        if (!isScrolling)
            transform.localPosition = startingPosition + Vector3.up * totalHeight;

        if (newMessage != null) newMessage(lastMessage);
    }

    static public string GetTime()
    {
        const int timescale = 2;
        int time = (int)((Time.fixedTime + 17*60*timescale) % (1440 * timescale) / timescale);
        string hours = (((time / 60) % 12 == 0) ? 12 : (time / 60) % 12).ToString();
        string minutes = (time % 60).ToString("D2");
        string ampm = (time > 719) ? "PM" : "AM";
        string formattedTime = hours + ":" + minutes + " " + ampm;

        return formattedTime;
    }

    public void AddMessages(int amount, float imageRate, float bannableRate)
    {
        for (int i = 0; i < amount; ++i)
        {
            User u = userData.GetUser();
            bool bannable = Random.value < bannableRate || 
                Random.value < player.factionBannableBonusRate[(int)u.type] || 
                lastBannableMessage >= maximumConsecutiveNormalMessages;
            if (Random.value < imageRate)
                AddMessage(u, bannable ? userData.GetBannableImageMessage(u.type) : userData.GetImageMessage(u.type), bannable);
            else
                AddMessage(u, bannable ? userData.GetBannableMessage(u.type) : userData.GetMessage(u.type), bannable);
            
            lastBannableMessage = (bannable) ? 0 : lastBannableMessage + 1;
        }
    }

    public void BanUser()
    {
        if (clickedMessage)
        {
            bool bannable = clickedMessage.bannable;

            User user = clickedMessage.user;
            if (user.canBan)
            {
                if (bannable)
                    player.AddBanMoney(user.type);

                if (lastMessage && lastMessage.user.guid == user.guid)
                    lastMessage = null;

                foreach (MessageCreator m in messagePool)
                {
                    if (m.gameObject.activeInHierarchy && m.user.guid == user.guid)
                    {
                        bannable |= m.bannable;
                        m.DisplayBanned(userData.invalidUser);
                    }
                }

                userData.RemoveUser(user);
            }

            clickedMessage = null;
        }
    }

}

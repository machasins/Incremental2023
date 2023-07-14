using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerHandler : MonoBehaviour
{
    public MessageSpawner messages;
    
    public float secondsPerMessage; // Messages per server member per second
    public float messageTimeVariation; // Varience of message rate
    public float banMessageRate; // Rate of bannable messages compared to non-bannable

    public int maxMembers;
    public float secondsPerUser; // Rate that the population will increase
    public float newUserTimeVariation; // Varience of user rate

    [HideInInspector] public float streamerMessageTimeMult;
    [HideInInspector] public float streamerUserTimeMult;
    [HideInInspector] public float streamerBannableRateMult;

    private UserData data;

    private float currentMessageTime = 0.0f;
    private float currentUserTime = 0.0f;
    private float messageTime = 0.0f;
    private float userTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        data = GetComponent<UserData>();

        data.AddUser(maxMembers / 2);
        messages.AddMessages(10, 0.1f, 0.0f);

        currentMessageTime = secondsPerMessage + Random.Range(-messageTimeVariation, messageTimeVariation);
        currentUserTime = secondsPerUser + Random.Range(-newUserTimeVariation, newUserTimeVariation);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (data.users.Count < maxMembers)
        {
            userTime += Time.fixedDeltaTime;
            if (userTime >= currentUserTime)
            {
                userTime = 0.0f;
                float memberMult = Mathf.Lerp(2.0f, 1.0f, maxMembers / 1000.0f) * streamerUserTimeMult;
                currentUserTime = (secondsPerUser + Random.Range(-newUserTimeVariation, newUserTimeVariation)) * memberMult;
                data.AddUser();
            }
        }

        messageTime += Time.fixedDeltaTime;
        if (messageTime >= currentMessageTime)
        {
            messageTime = 0.0f;
            float memberMult = Mathf.Lerp(2.0f, 1.0f, data.users.Count / 1000.0f) * streamerMessageTimeMult;
            currentMessageTime = (secondsPerMessage + Random.Range(-messageTimeVariation, messageTimeVariation)) * memberMult;
            messages.AddMessages(1, 0.1f, banMessageRate * streamerBannableRateMult);
        }
    }

    public void IncreaseUserCap(int amount)
    {
        maxMembers += amount;
    }
    
    public void IncreaseSecondsPerMessage(float amount)
    {
        secondsPerMessage *= amount;
        messageTimeVariation *= amount;
    }

    public void IncreaseSecondsPerUser(float amount)
    {
        secondsPerUser *= amount;
        newUserTimeVariation *= amount;
    }

    public void IncreaseBanMessageRate(float amount)
    {
        banMessageRate *= amount;
    }

    public void UnlockVisibleBanMessages()
    {
        MessageCreator.UnlockVisibleBanMessages();
        foreach (MessageCreator m in messages.messagePool)
            m.RefreshBanVision();
    }
}

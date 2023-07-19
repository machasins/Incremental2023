using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotHandler : MonoBehaviour
{
    public TwitchCoordinator stream;

    public float backgroundBotTime;
    public float backgroundBotMoney;

    public float cleanupBotChance;
    public float cleanupBotPercentMoneyPerBan;

    public float viewBotTime;
    public float viewBotMoneyPercent;

    [HideInInspector] public int backgroundBots;
    [HideInInspector] public int cleanupBots;
    [HideInInspector] public int viewBots;

    private PlayerData player;

    private float bkTime = 0.0f;
    private float viewTime = 0.0f;

    void Start()
    {
        player = GetComponent<PlayerData>();
    }

    void FixedUpdate()
    {
        if (backgroundBots > 0)
        {
            bkTime += Time.fixedDeltaTime;
            if (bkTime >= backgroundBotTime)
            {
                player.AddMoney(backgroundBotMoney * backgroundBots);
                bkTime = 0.0f;
            }
        }

        if (viewBots > 0)
        {
            viewTime += Time.fixedDeltaTime;
            if (viewTime >= viewBotTime)
            {
                player.AddMoney(player.baseInteractMoney[0] * viewBotMoneyPercent * viewBots);
                viewTime = 0.0f;
            }
        }
    }

    public bool CleanupSuccess()
    {
        return Random.value < cleanupBots * cleanupBotChance;
    }

    public void AddBackgroundAutoMod()
    {
        backgroundBots++;
    }

    public void AddCleanupAutoMod()
    {
        cleanupBots++;
    }

    public void AddViewBot()
    {
        viewBots++;
    }
}

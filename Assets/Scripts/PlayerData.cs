using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public float baseBanMoney = 1.0f;
    public float[] baseInteractMoney = { 0.02f, 0.01f };
    private float money = 0.0f;
    private float moneyMult = 1.0f;

    private User.Type currentFactionType = User.Type.maxUserType;
    [HideInInspector] public float[] factionBanBonus;
    [HideInInspector] public float[] factionBannableBonusRate;

    void Start()
    {
        factionBanBonus = new float[(int)User.Type.maxUserType];
        for (int i = 0; i < factionBanBonus.Length; ++i)
            factionBanBonus[i] = 0.0f;

        factionBannableBonusRate = new float[(int)User.Type.maxUserType];
        for (int i = 0; i < factionBannableBonusRate.Length; ++i)
            factionBannableBonusRate[i] = 0.0f;
    }

    public void AddMoney(float amount)
    {
        money += amount * moneyMult;
    }
    
    public void RemoveMoney(float amount)
    {
        if (money >= amount)
            money -= amount;
    }
    
    public void AddMoneyMult(float amount)
    {
        moneyMult *= amount;
    }

    public float GetMoney()
    {
        return money;
    }

    public float GetMoneyMult()
    {
        return moneyMult;
    }

    public void AddBanMoney(User.Type type)
    {
        money += baseBanMoney + factionBanBonus[(int)type];
    }

    public void AddBanMoney(string userType)
    {
        money += baseBanMoney + factionBanBonus[(int)(User.Type)System.Enum.Parse(typeof(User.Type), userType)];
    }

    public void AddInteractMoney(int streamer)
    {
        money += baseInteractMoney[streamer];
    }

    public void ChangeIncrementUserType(string userType)
    {
        currentFactionType = (User.Type)System.Enum.Parse(typeof(User.Type), userType);
    }

    public void IncreaseFactionBanBonus(float amount)
    {
        factionBanBonus[(int)currentFactionType] += amount;
    }

    public void IncreaseFactionBannableBonusRate(float amount)
    {
        factionBannableBonusRate[(int)currentFactionType] += amount;
    }

    public void IncreaseBaseBanMoney(float amount)
    {
        baseBanMoney += amount;
    }

    public void IncreaseAmoInteractBonus(float amount)
    {
        baseInteractMoney[0] += amount;
    }

    public void IncreaseTrainInteractBonus(float amount)
    {
        baseInteractMoney[1] += amount;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    private float money = 0.0f;
    private float moneyMult = 1.0f;

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
}

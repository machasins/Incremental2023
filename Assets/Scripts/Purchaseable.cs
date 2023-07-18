using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Purchaseable : MonoBehaviour
{
    public float price;
    public int amount;
    public float increaseMult;
    public PlayerData player;
    public TMP_Text label;
    public TMP_Text stock;

    private WorldButton button;
    private Color affordableColor;

    private int amountTotal;

    void Start()
    {
        button = GetComponent<WorldButton>();

        amountTotal = amount;

        if (button) button.Disable(player.GetMoney() < price);
    }

    public void Purchase()
    {
        if (player.GetMoney() >= price)
        {
            player.RemoveMoney(price);

            amount -= 1;
            price *= increaseMult;
            if (amount <= 0)
                price = Mathf.Infinity;
        }
    }

    void FixedUpdate()
    {
        if (button) button.Disable(player ? player.GetMoney() < price : false);

        UpdateData();
    }

    void OnDrawGizmosSelected()
    {
        amountTotal = amount;
        UpdateData();
    }

    void UpdateData()
    {
        if (label) 
            label.text = (amount > 0) ? "$" + price.ToString("F2") : "SOLD OUT";
        if (stock)
            stock.text = (amountTotal > 1 && amount > 0) ? amount + " in stock" : "";
    }
}

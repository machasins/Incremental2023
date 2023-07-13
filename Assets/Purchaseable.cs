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
    public Color unaffordableColor;

    private WorldButton button;
    private Color affordableColor;

    void Start()
    {
        button = GetComponent<WorldButton>();

        if (label) affordableColor = label.color;
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
        if (button) button.disabled = player.GetMoney() < price;
        if (label) 
        {
            label.text = (amount > 0) ? "$ " + price.ToString("F2") : "SOLD OUT";
            label.color = (player.GetMoney() >= price) ? affordableColor : unaffordableColor;
        }
    }
}

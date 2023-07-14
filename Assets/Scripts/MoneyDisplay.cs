using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyDisplay : MonoBehaviour
{
    public PlayerData player;
    private TMP_Text money;
    
    void Start()
    {
        money = GetComponent<TMP_Text>();
        money.text = "$ " + player.GetMoney().ToString("F2");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        money.text = "$ " + player.GetMoney().ToString("F2");
    }
}

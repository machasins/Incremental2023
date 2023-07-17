using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyDisplay : MonoBehaviour
{
    public PlayerData player;
    public string prefix = "$ ";
    private TMP_Text money;
    
    void Start()
    {
        money = GetComponent<TMP_Text>();
        money.text = prefix + player.GetMoney().ToString("F2");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        money.text = prefix + player.GetMoney().ToString("F2");
    }
}

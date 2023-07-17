using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDisplay : MonoBehaviour
{
    public SpriteRenderer coins;
    public PlayerData money;
    public Sprite[] coinGrowthSprites;
    public float maxMoney;

    void Start()
    {
        coins.sprite = coinGrowthSprites[0];
    }

    void Update()
    {
        int index = (int)Mathf.Lerp(0, coinGrowthSprites.Length - 1, money.GetMoney() / maxMoney);
        coins.sprite = coinGrowthSprites[index];
    }
}

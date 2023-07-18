using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaleItemSetup : MonoBehaviour
{
    public Sprite item;
    public string itemName;
    [TextArea]
    public string description;
    public float price;
    public int stock;
    public bool startUnlocked;

    public PlayerData player;
    public SpriteRenderer itemRender;
    public TMP_Text nameText;
    public TMP_Text descText;
    public Purchaseable purchase;

    void Start()
    {
        SetData();
    }

    void OnDrawGizmosSelected()
    {
        SetData();
    }

    void SetData()
    {
        if (itemRender)
        {
            itemRender.sprite = item;
            itemRender.size = item.bounds.size / (Mathf.Max(item.bounds.size.x, item.bounds.size.y));
        }
        if (nameText)
            nameText.text = itemName;
        if (descText)
            descText.text = description;
        if (purchase)
        {
            purchase.player = player;
            purchase.price = price;
            purchase.amount = stock;


            purchase.OnDrawGizmosSelected();
        }

        GetComponent<UnlockHandler>().InstantToggleLock(!startUnlocked);
    }
}

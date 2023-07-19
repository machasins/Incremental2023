using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipHandler : MonoBehaviour
{
    public enum Slot
    {
        wall,
        doorLeft,
        doorCenter,
        doorRight,
        computerRight,
        tabletFront,
        shelfLeft,
        shelfRight,
        cabinetTop,
        cabinetBottom,
        computer,
        mousePad,
        window,
        maxSlots
    };

    public Slot itemSlot;
    public Sprite slotSprite;
    public GameObject equipButton;

    public delegate void Unequip();
    private Unequip removeAll;
    private WorldButton equipButtonMechanics;
    private CollectibleHandler display;
    private ConsumeHandler consume;
    private SaleItemSetup sale;

    void Start()
    {
        EquipHandler[] others = transform.parent.GetComponentsInChildren<EquipHandler>();
        foreach(EquipHandler e in others)
            if (e != this && e.itemSlot == itemSlot)
                removeAll += e.UnequipItem;
        
        equipButtonMechanics = equipButton.GetComponent<WorldButton>();
        display = FindFirstObjectByType<CollectibleHandler>();
        consume = GetComponent<ConsumeHandler>();
        sale = GetComponent<SaleItemSetup>();
    }

    public void EquipItem()
    {
        equipButtonMechanics.Disable(true);

        removeAll();
        display.PlaceItem(itemSlot, slotSprite);

        consume.consumeButton.SetActive(sale.purchase.amount > 0);
    }

    public void UnequipItem()
    {
        equipButtonMechanics.Disable(false);

        if (consume.boughtOnce)
            consume.consumeButton.SetActive(false);
    }
}

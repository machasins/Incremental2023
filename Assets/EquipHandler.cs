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
    public SpriteRenderer slotRender;
    public GameObject equipButton;

    public delegate void Unequip();
    private Unequip removeAll;
    private WorldButton equipButtonMechanics;

    void Start()
    {
        EquipHandler[] others = transform.parent.GetComponentsInChildren<EquipHandler>();
        foreach(EquipHandler e in others)
            if (e != this && e.itemSlot == itemSlot)
                removeAll += e.UnequipItem;
        
        equipButtonMechanics = equipButton.GetComponent<WorldButton>();
    }

    public void EquipItem()
    {
        equipButtonMechanics.Disable(false);

        removeAll();
        slotRender.sprite = slotSprite;
    }

    public void UnequipItem()
    {
        equipButtonMechanics.Disable(true);
    }
}

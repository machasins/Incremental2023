using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConsumeHandler : MonoBehaviour
{
    public UnityEvent actions;
    public GameObject consumeButton;

    private EquipHandler equip;
    private SaleItemSetup sale;

    void Start()
    {
        equip = GetComponent<EquipHandler>();
        sale = GetComponent<SaleItemSetup>();
    }

    public void Consume()
    {
        if (sale.purchase.amount <= 0)
            consumeButton.SetActive(false);
            
        actions.Invoke();
        equip.EquipItem();
    }
}

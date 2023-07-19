using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConsumeHandler : MonoBehaviour
{
    public UnityEvent actions;
    public GameObject consumeButton;

    private EquipHandler equip;
    private UnlockHandler unlock;
    private SaleItemSetup sale;

    [HideInInspector] public bool boughtOnce = false;

    void Start()
    {
        equip = GetComponent<EquipHandler>();
        unlock = GetComponent<UnlockHandler>();
        sale = GetComponent<SaleItemSetup>();
    }

    public void Consume()
    {
        if (sale.purchase.amount <= 0)
            consumeButton.SetActive(false);

        boughtOnce = true;
            
        actions.Invoke();
        equip.EquipItem();
        unlock.Purchase();
    }
}

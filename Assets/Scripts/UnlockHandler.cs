using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockHandler : MonoBehaviour
{
    public UnlockHandler[] purchaseUnlocks;
    public GameObject lockObject;

    private bool isUnlocked;

    public void Unlock()
    {
        if (!isUnlocked)
        {
            lockObject.GetComponentInChildren<Animator>().SetBool("unlock", true);
            isUnlocked = true;
        }
    }
    
    public void InstantToggleLock(bool active)
    {
        lockObject.SetActive(active);
        isUnlocked = !active;
    }

    public void Purchase()
    {
        foreach(UnlockHandler u in purchaseUnlocks)
            if (u) u.Unlock();
    }
}

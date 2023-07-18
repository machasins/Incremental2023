using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorOptions : MonoBehaviour
{
    public GameObject triggerGameobject;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void ActivateBool(string param)
    {
        anim.SetBool(param, true);
    }
    
    void DeactivateBool(string param)
    {
        anim.SetBool(param, false);
    }

    public void ToggleGameObject(int active)
    {
        triggerGameobject.SetActive(active > 0);
    }
}

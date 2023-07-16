using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorOptions : MonoBehaviour
{
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
}

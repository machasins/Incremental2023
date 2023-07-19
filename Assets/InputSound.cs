using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSound : MonoBehaviour
{
    public InputActionReference action;

    private AnimatorOptions anim;

    void Start()
    {
        anim = GetComponent<AnimatorOptions>();
    }

    void Update()
    {
        if (action.ToInputAction().WasPressedThisFrame())
            anim.PlayRandomSound();
    }
}

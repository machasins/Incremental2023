using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using TMPro;

public class WorldButton : MonoBehaviour
{
    public UnityEvent action;
    public InputActionReference keyPress;

    public SpriteRenderer sprite;
    public Color disabledColor;

    [HideInInspector] public bool mouseHovered = false;
    [HideInInspector] public bool disabled = false;

    private Color enabledColor;

    void Start()
    {
        if (sprite)
        {
            enabledColor = sprite.color;
        }
    }

    void FixedUpdate()
    {
        if (sprite)
            sprite.color = (disabled) ? disabledColor : enabledColor;
    }

    void OnDisable()
    {
        mouseHovered = false;
    }

    void OnMouseOver()
    {
        mouseHovered = true;
        if(keyPress.ToInputAction().WasPressedThisFrame() && !disabled)
            action.Invoke();
    }

    void OnMouseExit()
    {
        mouseHovered = false;
    }
}

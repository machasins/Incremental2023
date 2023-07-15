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
    public Color disabledColor = Color.gray;
    public Color hoveredColor = Color.blue;
    public Color pressedColor = Color.white;

    [HideInInspector] public bool mouseHovered = false;
    [HideInInspector] public bool disabled = false;

    private Color normalColor;

    void Start()
    {
        if (sprite)
            normalColor = sprite.color;
    }

    public void Disable(bool newValue)
    {
        disabled = newValue;
        if (sprite)
            sprite.color = (disabled) ? disabledColor : normalColor;
    }

    void OnDisable()
    {
        mouseHovered = false;
    }

    void OnMouseOver()
    {
        mouseHovered = true;
        if (sprite && !disabled)
            sprite.color = hoveredColor;
        if(keyPress.ToInputAction().WasPressedThisFrame() && !disabled)
        {
            if (sprite)
                sprite.color = pressedColor;
            action.Invoke();
        }
    }

    void OnMouseExit()
    {
        mouseHovered = false;
        if (sprite && !disabled)
            sprite.color = normalColor;
    }
}

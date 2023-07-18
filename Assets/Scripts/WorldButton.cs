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

    public SpriteRenderer render;
    public Sprite disabledSprite;
    public Sprite hoveredSprite;

    [HideInInspector] public bool mouseHovered = false;
    [HideInInspector] public bool disabled = false;

    private Sprite normalSprite;

    void Start()
    {
        if (render)
            normalSprite = render.sprite;
    }

    public void Disable(bool newValue)
    {
        disabled = newValue;
        if (render && disabledSprite)
            render.sprite = (disabled) ? disabledSprite : normalSprite;
    }

    void OnDisable()
    {
        mouseHovered = false;
    }

    void OnMouseOver()
    {
        mouseHovered = true;
        if (render && !disabled && hoveredSprite)
            render.sprite = hoveredSprite;
        if(keyPress.ToInputAction().WasPressedThisFrame() && !disabled)
            action.Invoke();
    }

    void OnMouseExit()
    {
        mouseHovered = false;
        if (render && !disabled)
            render.sprite = normalSprite;
    }
}

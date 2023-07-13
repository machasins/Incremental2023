using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class BanMenuSetup : MonoBehaviour
{
    public Vector2 limitX;
    public Vector2 limitY;
    public Vector2 widthBounds;
    public Vector2 heightBounds;

    public SpriteRenderer icon;
    public TMP_Text username;

    public WorldButton banButton;

    [HideInInspector] public bool mouseHovered = false;

    private MessageSpawner handler;
    private DefaultInputActions input;

    void Start()
    {
        input = new DefaultInputActions();
        input.Enable();
    }

    void Update()
    {
        print(mouseHovered + " " +  banButton.mouseHovered);
        if ((input.UI.Click.WasPressedThisFrame() && (!mouseHovered && !banButton.mouseHovered)) || input.UI.Cancel.WasPressedThisFrame())
            gameObject.SetActive(false);
    }

    void OnDisable()
    {
        mouseHovered = false;
    }

    public void Create(Vector3 position, Sprite icon, Color userColor, string username, MessageSpawner caller)
    {
        this.icon.sprite = icon;

        if (icon)
            this.icon.size = icon.bounds.size / (Mathf.Min(icon.bounds.size.x, icon.bounds.size.y));

        this.username.text = username;
        this.username.color = userColor;

        if (position.x + widthBounds.y > limitX.y)
            transform.localPosition -= Vector3.right * widthBounds.y;
        if (position.y + heightBounds.x > limitY.x)
            transform.localPosition -= Vector3.up * (position.y + heightBounds.x - limitY.x);
        if (position.y - heightBounds.y < limitY.y)
            transform.localPosition -= Vector3.down * (limitY.y - (position.y - heightBounds.y));

        handler = caller;
    }

    public void BanUser()
    {
        handler.BanUser();
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }

    void OnMouseOver()
    {
        mouseHovered = true;
    }

    void OnMouseExit()
    {
        mouseHovered = false;
    }
}

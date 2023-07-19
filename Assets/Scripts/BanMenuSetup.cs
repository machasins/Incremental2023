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
    public SpriteRenderer role;
    public SpriteRenderer background;

    public Sprite[] roles;

    public WorldButton banButton;
    public Animator banhammer;
    public SpriteRenderer banButtonSprite;
    public Sprite unclickedBanButton;
    public GameObject inputBlocker;

    [HideInInspector] public bool mouseHovered = false;

    private MessageSpawner handler;
    private DefaultInputActions input;
    private bool forcedActive = false;

    void Start()
    {
        input = new DefaultInputActions();
        input.Enable();
    }

    void Update()
    {
        if ((input.UI.Click.WasPressedThisFrame() && (!mouseHovered && !banButton.mouseHovered)) || input.UI.Cancel.WasPressedThisFrame())
            CloseMenu();
    }

    void OnDisable()
    {
        mouseHovered = false;
        forcedActive = false;

        banButtonSprite.sprite = unclickedBanButton;
        inputBlocker.SetActive(false);
    }

    public void Create(Vector3 position, User user, bool bannable, MessageSpawner caller)
    {
        banButton.gameObject.SetActive(user.canBan);

        this.icon.sprite = user.userIcon;

        if (user.userIcon)
            this.icon.size = user.userIcon.bounds.size / (Mathf.Min(user.userIcon.bounds.size.x, user.userIcon.bounds.size.y));

        this.username.text = user.username;
        this.username.color = user.userColor;
        this.role.sprite = roles[(int)user.type];
        this.background.color = user.userColor;

        if (position.x + widthBounds.y > limitX.y)
            transform.localPosition += Vector3.left * widthBounds.y;
        if (position.y + heightBounds.x > limitY.y)
            transform.localPosition += Vector3.down * Mathf.Abs(position.y + heightBounds.x - limitY.y);
        if (position.y - heightBounds.y < limitY.x)
            transform.localPosition += Vector3.up * Mathf.Abs(limitY.x - (position.y - heightBounds.y));
        
        banhammer.SetBool("good", bannable);

        handler = caller;
    }

    public void BanUser()
    {
        forcedActive = true;
        banhammer.SetBool("triggered", true);
        handler.BanUser();

        inputBlocker.SetActive(true);
    }

    public void CloseMenu()
    {
        if (!forcedActive)
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

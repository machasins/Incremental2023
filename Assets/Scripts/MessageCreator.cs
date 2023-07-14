using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class MessageCreator : MonoBehaviour
{
    public SpriteRenderer icon;
    public TMP_Text username;
    public TMP_Text textBox;
    public SpriteRenderer image;
    public TMP_Text timeStamp;
    public SpriteRenderer banHighlight;
    public BoxCollider2D hitbox;

    [HideInInspector] public float height;
    public float minHeight;
    public float addHeight;
    public float maxImageHeight;

    [HideInInspector] static public bool visibleBanUpgrade = false;

    [HideInInspector] public User user;
    [HideInInspector] public bool bannable;

    private static DefaultInputActions input;

    void Start()
    {
        if (input == null)
        {
            input = new DefaultInputActions();
            input.Enable();
        }
    }
    
    void Create(Sprite icon, Color userColor, string username, string timestamp)
    {
        this.icon.sprite = icon;
        this.username.text = username;
        this.username.color = userColor;
        timeStamp.text = "Today at " + timestamp;

        if (icon)
            this.icon.size = icon.bounds.size / (Mathf.Min(icon.bounds.size.x, icon.bounds.size.y));

        hitbox.enabled = true;
        hitbox.size = new Vector2(hitbox.size.x, height * 2.0f);
        hitbox.offset = new Vector2(hitbox.offset.x, addHeight - height + addHeight / 2.0f);

        banHighlight.enabled = (visibleBanUpgrade) ? bannable : false;
        banHighlight.size = hitbox.size;
        banHighlight.transform.localPosition = hitbox.offset;
    }

    public void Create(User user, string message, string timestamp, bool bannable = false)
    {
        textBox.gameObject.SetActive(true);

        textBox.text = message;
        height = Mathf.Max((textBox.preferredHeight / 2.0f) + addHeight, minHeight);

        image.gameObject.SetActive(false);

        this.user = user;
        this.bannable = bannable;

        Create(user.userIcon, user.userColor, user.username, timestamp);
    }
    
    public void Create(User user, Sprite message, string timestamp, bool bannable = false)
    {
        image.gameObject.SetActive(true);

        image.sprite = message;
        image.size = message.bounds.size / (Mathf.Max(message.bounds.size.x, message.bounds.size.y));
        image.transform.localScale = Vector2.one * maxImageHeight;
        height = Mathf.Max((image.size.y * maxImageHeight / 2.0f) + addHeight, minHeight);

        textBox.gameObject.SetActive(false);

        this.user = user;
        this.bannable = bannable;

        Create(user.userIcon, user.userColor, user.username, timestamp);
    }

    public void Append(string message, bool bannable = false)
    {
        textBox.text = textBox.text + "\n\n" + message;
        height = Mathf.Max((textBox.preferredHeight / 2.0f) + addHeight, minHeight);
        
        this.bannable |= bannable;

        hitbox.size = new Vector2(hitbox.size.x, height * 2.0f);
        hitbox.offset = new Vector2(hitbox.offset.x, addHeight - height + addHeight / 2.0f);

        banHighlight.enabled = (visibleBanUpgrade) ? bannable : false;
        banHighlight.size = hitbox.size;
        banHighlight.transform.localPosition = hitbox.offset;
    }

    public void DisplayBanned(User invalid)
    {
        this.icon.sprite = invalid.userIcon;
        this.username.text = invalid.username;
        this.username.color = invalid.userColor;
        this.bannable = false;

        if (icon)
            this.icon.size = icon.bounds.size / (Mathf.Min(icon.bounds.size.x, icon.bounds.size.y));

        textBox.gameObject.SetActive(true);
        image.gameObject.SetActive(false);
        textBox.text = "[removed]";

        hitbox.enabled = false;
        banHighlight.enabled = false;
    }

    public void OnMouseOver()
    {
        if (input != null && input.UI.RightClick.WasPressedThisFrame())
        {
            transform.parent.GetComponent<MessageSpawner>().OnRightClick(icon.sprite, username.color, user.username, this);
        }
    }

    public void RefreshBanVision()
    {
        banHighlight.enabled = (visibleBanUpgrade) ? bannable : false;
    }

    public static void UnlockVisibleBanMessages()
    {
        visibleBanUpgrade = true;
    }
}

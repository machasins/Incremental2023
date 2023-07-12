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
    public BoxCollider2D hitbox;

    [HideInInspector] public float height;
    public float minHeight;
    public float addHeight;
    public float maxImageHeight;

    [HideInInspector] public string user;

    private static DefaultInputActions input;


    void Start()
    {
        if (input == null)
        {
            input = new DefaultInputActions();
            input.Enable();
        }
    }
    
    public void Create(Sprite icon, Color userColor, string username, string timestamp)
    {
        this.icon.sprite = icon != null ? icon : this.icon.sprite;
        this.username.text = username;
        this.username.color = userColor;
        timeStamp.text = "Today at " + timestamp;

        if (icon)
            this.icon.size = icon.bounds.size / (Mathf.Min(icon.bounds.size.x, icon.bounds.size.y));

        hitbox.size = new Vector2(hitbox.size.x, height * 2.0f);
        hitbox.offset = new Vector2(hitbox.offset.x, addHeight - height);

        user = username;
    }

    public void Create(Sprite icon, Color userColor, string username, string message, string timestamp)
    {
        textBox.gameObject.SetActive(true);

        textBox.text = message;
        height = Mathf.Max((textBox.preferredHeight / 2.0f) + addHeight, minHeight);

        image.gameObject.SetActive(false);

        Create(icon, userColor, username, timestamp);
    }
    
    public void Create(Sprite icon, Color userColor, string username, Sprite message, string timestamp)
    {
        image.gameObject.SetActive(true);

        image.sprite = message;
        image.size = message.bounds.size / (Mathf.Max(message.bounds.size.x, message.bounds.size.y));
        image.transform.localScale = Vector2.one * maxImageHeight;
        height = Mathf.Max((image.size.y * maxImageHeight / 2.0f) + addHeight, minHeight);

        textBox.gameObject.SetActive(false);

        Create(icon, userColor, username, timestamp);
    }

    public void Append(string message)
    {
        textBox.text = textBox.text + "\n\n" + message;
        height = Mathf.Max((textBox.preferredHeight / 2.0f) + addHeight, minHeight);
    }

    public void OnMouseOver()
    {
        if (input.UI.RightClick.WasPressedThisFrame())
        {
            transform.parent.GetComponent<MessageSpawner>().OnRightClick(icon.sprite, username.color, user, this);
        }
    }
}

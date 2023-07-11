using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageCreator : MonoBehaviour
{
    public SpriteRenderer messageIcon;
    public TMP_Text messageUsername;
    public TMP_Text messageTextBox;
    public SpriteRenderer messageImage;
    public TMP_Text messageTimeStamp;

    [HideInInspector] public float messageHeight;
    public float messageMinHeight;
    public float messageAddHeight;
    public float messageMaxImageHeight;

    [HideInInspector] public string user;

    public void Create(Sprite icon, Color userColor, string username, string timestamp)
    {
        messageIcon.sprite = icon != null ? icon : messageIcon.sprite;
        messageUsername.text = username;
        messageUsername.color = userColor;
        messageTimeStamp.text = "Today at " + timestamp;

        if (icon)
            messageIcon.size = icon.bounds.size / (Mathf.Min(icon.bounds.size.x, icon.bounds.size.y));

        user = username;
    }

    public void Create(Sprite icon, Color userColor, string username, string message, string timestamp)
    {
        Create(icon, userColor, username, timestamp);

        messageTextBox.gameObject.SetActive(true);

        messageTextBox.text = message;
        messageHeight = Mathf.Max((messageTextBox.preferredHeight / 2.0f) + messageAddHeight, messageMinHeight);

        messageImage.gameObject.SetActive(false);
    }
    
    public void Create(Sprite icon, Color userColor, string username, Sprite message, string timestamp)
    {
        Create(icon, userColor, username, timestamp);

        messageImage.gameObject.SetActive(true);

        messageImage.sprite = message;
        messageImage.size = message.bounds.size / (Mathf.Max(message.bounds.size.x, message.bounds.size.y));
        messageImage.transform.localScale = Vector2.one * messageMaxImageHeight;
        messageHeight = Mathf.Max((messageImage.size.y * messageMaxImageHeight / 2.0f) + messageAddHeight, messageMinHeight);

        messageTextBox.gameObject.SetActive(false);
    }

    public void Append(string message)
    {
        messageTextBox.text = messageTextBox.text + "\n\n" + message;
        messageHeight = Mathf.Max((messageTextBox.preferredHeight / 2.0f) + messageAddHeight, messageMinHeight);
    }
}

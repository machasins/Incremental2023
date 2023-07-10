using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageCreator : MonoBehaviour
{
    public SpriteRenderer messageIcon;
    public TMP_Text messageUsername;
    public TMP_Text messageTextBox;
    public TMP_Text messageTimeStamp;

    [HideInInspector] public float messageHeight;
    public float messageMinHeight;
    public float messageAddHeight;

    [HideInInspector] public string user;

    public void Create(Sprite icon, Color userColor, string username, string message, string timestamp)
    {
        messageIcon.sprite = icon != null ? icon : messageIcon.sprite;
        messageUsername.text = username;
        messageUsername.color = userColor;
        messageTimeStamp.text = "Today at " + timestamp;

        messageTextBox.text = message;
        messageHeight = Mathf.Max((messageTextBox.preferredHeight / 2.0f) + messageAddHeight, messageMinHeight);

        user = username;
    }

    public void Append(string message)
    {
        messageTextBox.text = messageTextBox.text + "\n\n" + message;
        messageHeight = Mathf.Max((messageTextBox.preferredHeight / 2.0f) + messageAddHeight, messageMinHeight);
    }
}

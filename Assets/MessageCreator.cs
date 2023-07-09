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

    public void Create(Sprite icon, Color userColor, string username, string message, string timestamp)
    {
        messageIcon.sprite = icon;
        messageUsername.text = username;
        messageUsername.color = userColor;
        messageTimeStamp.text = "Today at " + timestamp;

        messageTextBox.text = message;
        messageHeight = Mathf.Max(messageTextBox.preferredHeight + messageAddHeight, messageMinHeight);
    }

    public void Append(string message)
    {
        messageTextBox.text = messageTextBox.text + "\n" + message;
        messageHeight = Mathf.Max(messageTextBox.preferredHeight + messageAddHeight, messageMinHeight);
    }
}

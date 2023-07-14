using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MemberCreator : MonoBehaviour
{
    public SpriteRenderer userIcon;
    public TMP_Text userName;
    public SpriteRenderer userStatus;

    public Color[] statusColors;

    [HideInInspector] public User user;

    public void Create(User user)
    {
        this.user = user;

        userIcon.sprite = user.userIcon;
        userName.text = user.username;
        userName.color = user.userColor;
        userStatus.color = statusColors[(int)user.status];

        if (user.userIcon)
            userIcon.size = user.userIcon.bounds.size / (Mathf.Min(user.userIcon.bounds.size.x, user.userIcon.bounds.size.y));
    }
}

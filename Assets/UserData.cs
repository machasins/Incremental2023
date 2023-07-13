using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public enum userType
    {
        normal,
        furry,
        anime,
        kommando,
        consumer,
        maxUserType
    };

    public string username;
    public Color userColor;
    public Sprite userIcon;
    public System.Guid guid;
    public userType type;
}

public class UserData : MonoBehaviour
{
    public TextAsset usernameFile;
    public float usernameAddonChance;
    public float usernameNumberChance;
    public float usernameYearChance;


    [HideInInspector] public List<User> users;
    private List<List<string>> usernames;
    private List<List<Object>> userIcons;
    private Color[] userColors = 
    {
        Color.red,
        Color.cyan,
        Color.green,
        Color.white,
        Color.magenta,
        Color.yellow,
    };

    private List<List<string>> messages;
    private List<List<string>> banMessages;
    private List<List<Object>> messageImages;
    private List<List<Object>> banMessageImages;

    public User invalidUser;

    void Start()
    {
        List<string> list = new List<string>(usernameFile.text.Split("\n\n"));

        usernames = new List<List<string>>();
        foreach (string l in list)
            usernames.Add(new List<string>(l.Split('\n')));

        messages = new List<List<string>>();
        for (int i = 0; i < (int)User.userType.maxUserType; ++i)
            messages.Add(new List<string>((Instantiate(Resources.Load(((User.userType)i).ToString() + "/m")) as TextAsset).text.Split("\n\n")));

        banMessages = new List<List<string>>();
        for (int i = 0; i < (int)User.userType.maxUserType; ++i)
            banMessages.Add(new List<string>((Instantiate(Resources.Load(((User.userType)i).ToString() + "/m_b")) as TextAsset).text.Split("\n\n")));

        messageImages = new List<List<Object>>();
        for (int i = 0; i < (int)User.userType.maxUserType; ++i)
            messageImages.Add(new List<Object>(Resources.LoadAll(((User.userType)i).ToString() + "/mi", typeof(Sprite))));

        banMessageImages = new List<List<Object>>();
        for (int i = 0; i < (int)User.userType.maxUserType; ++i)
            banMessageImages.Add(new List<Object>(Resources.LoadAll(((User.userType)i).ToString() + "/mi_b", typeof(Sprite))));

        userIcons = new List<List<Object>>();
        for (int i = 0; i < (int)User.userType.maxUserType; ++i)
            userIcons.Add(new List<Object>(Resources.LoadAll(((User.userType)i).ToString() + "/a", typeof(Sprite))));

        users = new List<User>();

        invalidUser = new User();
        invalidUser.userColor = Color.gray;
        invalidUser.userIcon = null;
        invalidUser.username = "[removed]";
    }

    public void AddUser(int num)
    {
        for (int i = 0; i < num; i++)
            AddUser();
    }

    public void AddUser()
    {
        User u = new User();
        u.type = (User.userType)Random.Range(0, (int)User.userType.maxUserType);
        u.username = usernames[(int)u.type][Random.Range(0, usernames[(int)u.type].Count)];
        u.userColor = userColors[Random.Range(0, userColors.Length)];
        u.userIcon = Instantiate(userIcons[(int)u.type][Random.Range(0, userIcons[(int)u.type].Count)]) as Sprite;
        u.guid = System.Guid.NewGuid();
        
        if (Random.value <= usernameAddonChance)
        {
            float addon = Random.value;
            if (addon <= usernameNumberChance)
            {
                // Append a number at the end
                u.username += Random.Range(1,32).ToString();
            }
            else if (addon <= usernameNumberChance + usernameYearChance)
            {
                // Append a year at the end
                u.username += Random.Range(1959,2013).ToString();
            }
            else
            {
                // Append some flair
                u.username = "xXx" + u.username + "xXx";
            }
        }

        users.Add(u);
    }

    public void RemoveUser(User user)
    {
        users.Remove(user);
    }

    public User GetUser()
    {
        return users[Random.Range(0, users.Count)];
    }

    public string GetMessage(User.userType type)
    {
        return messages[(int)type][Random.Range(0, messages[(int)type].Count)];
    }

    public Sprite GetImageMessage(User.userType type)
    {
        return Instantiate(messageImages[(int)type][Random.Range(0, messageImages[(int)type].Count)]) as Sprite;
    }

    public string GetBannableMessage(User.userType type)
    {
        return banMessages[(int)type][Random.Range(0, banMessages[(int)type].Count)];
    }

    public Sprite GetBannableImageMessage(User.userType type)
    {
        return Instantiate(banMessageImages[(int)type][Random.Range(0, banMessageImages[(int)type].Count)]) as Sprite;
    }
}

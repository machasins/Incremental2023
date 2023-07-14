using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemberList : MonoBehaviour
{
    public UserData data;
    public Transform onlineHeading;
    public Transform offlineHeading;

    public int maxAmountShown;
    public Transform userPrefab;
    public float memberLength;

    private List<MemberCreator> userPool;
    private List<User> leftoverUsers;
    private List<MemberCreator> offlineUsers;

    void Awake()
    {
        data.addedUser += AddUserToList;
        data.removedUser += RemoveUserFromList;

        leftoverUsers = new List<User>();
        offlineUsers = new List<MemberCreator>();

        InitializeUserPool();
    }

    void OnEnable()
    {
        UpdateList();
    }

    void InitializeUserPool()
    {
        userPool = new List<MemberCreator>();
        for (int i = 0; i < maxAmountShown; ++i)
        {
            Transform user = Instantiate(userPrefab, transform);
            user.gameObject.SetActive(false);
            userPool.Add(user.GetComponent<MemberCreator>());
        }
    }

    MemberCreator GetNewUserFromPool()
    {
        foreach (MemberCreator user in userPool)
            if (!user.gameObject.activeInHierarchy)
                return user;
        return null;
    }
    
    MemberCreator GetUser(User user)
    {
        foreach (MemberCreator m in userPool)
            if (m.gameObject.activeInHierarchy && m.user.guid == user.guid)
                return m;
        return null;
    }

    void UpdateList()
    {
        List<MemberCreator> online = new List<MemberCreator>();
        List<MemberCreator> offline = new List<MemberCreator>();

        foreach (MemberCreator m in userPool)
        {
            if (!m.gameObject.activeInHierarchy)
                continue;
            if (m.user.status != User.Status.offline)
                online.Add(m);
            else
                offline.Add(m);
        }

        online.Sort((MemberCreator a, MemberCreator b) => string.Compare(a.user.username, b.user.username));
        offline.Sort((MemberCreator a, MemberCreator b) => string.Compare(a.user.username, b.user.username));

        float distance = 0.0f;
        Vector3 start = onlineHeading.transform.localPosition;
        foreach (MemberCreator m in online)
        {
            distance -= memberLength;
            m.transform.localPosition = new Vector3(start.x, start.y + distance, start.z);
        }

        distance -= memberLength;
        offlineHeading.transform.localPosition = start + Vector3.up * distance;
        foreach (MemberCreator m in offline)
        {
            distance -= memberLength;
            m.transform.localPosition = new Vector3(start.x, start.y + distance, start.z);
        }
    }

    public void AddUserToList(User user)
    {
        MemberCreator m = GetNewUserFromPool();
        if (!m)
        {
            if (offlineUsers.Count > 0 && user.status != User.Status.offline)
            {
                leftoverUsers.Add(offlineUsers[0].user);
                offlineUsers[0].Create(user);
                offlineUsers.RemoveAt(0);
            }
            return;
        }
        
        m.gameObject.SetActive(true);
        m.Create(user);

        if (user.status == User.Status.offline)
            offlineUsers.Add(m);

        UpdateList();
    }

    public void RemoveUserFromList(User user)
    {
        MemberCreator m = GetUser(user);
        if (!m)
        {
            leftoverUsers.Remove(user);
            return;
        }

        if (leftoverUsers.Count > 0)
        {
            User replacement = leftoverUsers.Find((User u) => u.status != User.Status.offline);

            m.Create(replacement);
            leftoverUsers.Remove(replacement);
        }
        else
            m.gameObject.SetActive(false);
        
        UpdateList();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ServerStatsVeiwer : MonoBehaviour
{
    public ServerHandler server;
    public UserData data;
    public TMP_Text serverPop;

    void FixedUpdate()
    {
        serverPop.text = data.users.Count + " / " + server.maxMembers + " members";
    }
}

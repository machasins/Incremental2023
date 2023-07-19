using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ServerStatsVeiwer : MonoBehaviour
{
    FollowerTracker follow;
    public TMP_Text serverPop;

    void Start()
    {
        follow = FindFirstObjectByType<FollowerTracker>();
    }

    void FixedUpdate()
    {
        serverPop.text = follow.amoFollowers + " followers";
    }
}

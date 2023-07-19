using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerRender : MonoBehaviour
{
    public RectTransform amo;
    public RectTransform train;

    public Vector3 minPos;
    public Vector3 maxPos;

    public float maxFollowers;

    FollowerTracker f;

    void Start()
    {
        f = GetComponent<FollowerTracker>();
        SetPositions();
    }

    void Update()
    {
        SetPositions();
    }

    void SetPositions()
    {
        amo.anchoredPosition = Vector3.Lerp(minPos, maxPos, f.amoFollowers / maxFollowers);
        train.anchoredPosition = Vector3.Lerp(minPos, maxPos, f.trainFollowers / maxFollowers);
    }
}

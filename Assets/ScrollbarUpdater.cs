using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollbarUpdater : MonoBehaviour
{
    public MessageSpawner spawner;
    public float topHeight;
    private Scrollbar scrollbar;

    void Start()
    {
        scrollbar = GetComponent<Scrollbar>();
    }

    void FixedUpdate()
    {
        //scrollbar.size = Mathf.Lerp(1.0f, 0.15f, Mathf.Clamp01(spawner.totalHeight - topHeight));
    }
}

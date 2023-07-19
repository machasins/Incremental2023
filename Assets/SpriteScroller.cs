using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteScroller : MonoBehaviour
{
    public float amount;
    public float minTime;
    public float maxTime;

    private Material _material;
    private float time = 0.0f;
    private float currentTime = 0.0f;
    private float currentscroll;
    void Start()
    {
        _material = GetComponent<SpriteRenderer>().material;
        currentTime = Random.Range(minTime, maxTime);
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time >= currentTime)
        {
            currentscroll += amount;
            _material.mainTextureOffset = new Vector2(0, -currentscroll);

            time = 0.0f;
            currentTime = Random.Range(minTime, maxTime);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobbing : MonoBehaviour
{
    public float distance;
    public float speed;

    float originalY;
    float randomOffset;

    void Start()
    {
        randomOffset = Random.value * 20.0f;
        originalY = transform.localPosition.y;
        Bob();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Bob();
    }

    void Bob()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, originalY + Mathf.Sin((Time.time + randomOffset) * speed) * distance, transform.localPosition.z); 
    }
}

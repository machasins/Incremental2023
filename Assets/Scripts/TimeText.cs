using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeText : MonoBehaviour
{
    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<TMP_Text>().text = MessageSpawner.GetTime();
    }
}

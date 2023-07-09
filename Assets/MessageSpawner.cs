using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageSpawner : MonoBehaviour
{
    public Transform message;

    void Start()
    {
        StartCoroutine(loop());
    }

    IEnumerator loop()
    {
        string note = "long message long message long message long message long message long message long message long message long message long message ";
        int count = 0;
        while (count < 20)
        {
            Transform m = Instantiate(message, transform);
            MessageCreator mc = m.GetComponent<MessageCreator>();
            mc.Create(null, Color.cyan, "spammerIRL2077", note, "12:59 PM");
            m.position += Vector3.up * mc.messageHeight * count;

            yield return new WaitForSeconds(1.0f);
            count++;
            note += "long message long message long message long message long message long message long message long message long message long message ";
        }

    }
}

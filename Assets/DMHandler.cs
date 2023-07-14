using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMHandler : MonoBehaviour
{
    public MessageSpawner dmServer;
    public TextAsset tutorialScript;
    public float delayBetweenMessages;

    public User dmSender;
    public Sprite senderIcon;
    public Color senderColor;
    public string senderUsername;

    private List<string> tutorialMessages;
    private int index = 0;
    private float time = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        tutorialMessages = new List<string>(tutorialScript.text.Split("\n\n"));

        dmSender = new User();
        dmSender.userIcon = senderIcon;
        dmSender.userColor = senderColor;
        dmSender.username = senderUsername;
    }

    // Update is called once per frame
    void Update()
    {
        if (index < tutorialMessages.Count)
        {
            time += Time.deltaTime;
            if (time >= delayBetweenMessages)
            {
                if (tutorialMessages[index] != "") 
                    dmServer.AddMessage(dmSender, tutorialMessages[index], false);
                index++;
                time = 0.0f;
            }
        }
    }
}

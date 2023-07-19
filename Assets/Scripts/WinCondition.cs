using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WinCondition : MonoBehaviour
{
    public UserData userData;
    public MomDistraction mom;
    public CameraController cam;
    public Fade fade;
    public string finalSceneName;
    public GameObject finalMessage;
    public AudioClip finalSound;
    public int followerGoal;

    private bool endingStarted = false;

    void Update()
    {
        if (userData.users.Count >= followerGoal && !endingStarted)
        {
            StartEnding();
            endingStarted = true;
        }
    }

    void StartEnding()
    {
        mom.ExitStuck();
        StartCoroutine(cam.objectZoom(cam.computer, cam.computerViewSize));
        finalMessage.SetActive(true);
    }

    public void EndGame()
    {
        StartCoroutine(GameWon());
    }

    IEnumerator GameWon()
    {
        cam.GetComponent<AudioSource>().PlayOneShot(finalSound);

        yield return StartCoroutine(cam.FinishingUnZoom());

        yield return new WaitForSeconds(0.5f);

        fade.SwitchScene(finalSceneName);
    }
}

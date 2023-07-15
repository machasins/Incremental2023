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
    public GameObject inputBlocker;
    public int followerGoal;

    void Update()
    {
        if (userData.users.Count >= followerGoal)
        {
            StartCoroutine(GameWon());
        }
    }

    IEnumerator GameWon()
    {
        inputBlocker.SetActive(true);

        mom.ExitStuck();

        yield return StartCoroutine(cam.FinishingUnZoom());

        yield return new WaitForSeconds(0.5f);

        fade.SwitchScene(finalSceneName);
    }
}

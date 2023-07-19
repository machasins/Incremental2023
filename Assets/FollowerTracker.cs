using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerTracker : MonoBehaviour
{
    // Train gains followers at a steady rate
    // Train loses followers when interacted with
    
    // Amo gains followers at a very slow rate
    // Amo gains many followers when donated to
    // Amo loses followers when a ban is missed
    // Amo gains more followers when interacted with
    // Amo's rate of follower gains goes up with the rate of correct bans, streak system

    public int trainStartingFollowers;
    public float trainGainRate;
    public float trainInteractRate;
    public float trainProtectPercent;
    public float trainContinuePercent;

    public int amoStartingFollowers;
    public float amoGainRate;
    public float amoInteractRate;
    public int amoDonoGain;
    public int amoBanLoss;

    public float amoStreakMaxMult;
    public int amoStreakMax;
    public float amoStreakMaxTime;

    [HideInInspector] public float trainFollowers;
    [HideInInspector] public float amoFollowers;

    private bool trainModProtection;
    private float trainContinueVal;
    private float trainMinVal;

    private int amoStreak;
    private float amoStreakTime = 0.0f;

    void Start()
    {
        trainFollowers = trainStartingFollowers;
        amoFollowers = amoStartingFollowers;
    }

    void FixedUpdate()
    {
        if (amoStreak > 0)
        {
            amoStreakTime += Time.fixedDeltaTime;
            if (amoStreakTime >= amoStreakMaxTime)
            {
                amoStreak = 0;
                amoStreakTime = 0.0f;
            }
        }

        amoFollowers += (amoGainRate * Mathf.Lerp(1.0f, amoStreakMaxMult, (float)amoStreak / amoStreakMax)) * Time.fixedDeltaTime;
        trainFollowers += trainGainRate * Time.fixedDeltaTime;

        if (trainModProtection && trainFollowers >= trainContinueVal)
        {
            trainModProtection = false;
            trainMinVal = 0;
        }
    }

    public void TrainInteract(float percentCharged)
    {
        if (!trainModProtection)
        {
            trainModProtection = true;
            trainContinueVal = trainFollowers * trainContinuePercent;
            trainMinVal = trainFollowers * trainProtectPercent;
        }

        trainFollowers -= percentCharged * trainInteractRate;
        trainFollowers = Mathf.Clamp(trainFollowers, trainMinVal, Mathf.Infinity);
    }

    public void AmoInteract(float percentCharged)
    {
        amoFollowers += percentCharged * amoInteractRate;
        amoFollowers = Mathf.Clamp(amoFollowers, 0, 1000);
    }

    public void AmoDono()
    {
        amoFollowers += amoDonoGain;
        amoFollowers = Mathf.Clamp(amoFollowers, 0, 1000);
    }

    public void AmoMissedBan()
    {
        amoFollowers -= amoBanLoss;
        amoFollowers = Mathf.Clamp(amoFollowers, 0, 1000);
        print("missed a banned message");
    }

    public void AmoStreakInc()
    {
        amoStreak++;
        amoStreakTime = 0.0f;
    }
}

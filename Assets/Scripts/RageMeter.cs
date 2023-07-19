using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageMeter : MonoBehaviour
{
    public PlayerData player;
    public StreamController stream;
    public Transform initialFire;
    public Transform passiveIncomeFire;
    public Animator skull;

    public Vector2 fireLimits;

    public float passiveIncomeAmount;
    public float passiveIncomeTime;
    public float passiveIncomeThreshold;
    public float interactAmount;
    public float decayRate;
    public float skullLaughCooldown;
    public float fireTime;

    private bool fireActive = false;
    private bool passiveIncomeFireActive = false;
    private float amount;
    private float time = 0.0f;
    private float skullTime = 0.0f;
    private MusicHandler music;
    private TwitchCoordinator twitch;
    private FollowerTracker follow;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 position = initialFire.localPosition;
        position.x = fireLimits.x;
        initialFire.localPosition = position;

        position = passiveIncomeFire.localPosition;
        position.x = fireLimits.x;
        passiveIncomeFire.localPosition = position;

        music = FindFirstObjectByType<MusicHandler>();
        twitch = FindFirstObjectByType<TwitchCoordinator>();
        follow = FindFirstObjectByType<FollowerTracker>();
    }

    void OnEnable()
    {
        amount = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        amount = Mathf.Clamp01(amount - decayRate * Time.fixedDeltaTime);

        if (amount > 0 && !fireActive)
        {
            fireActive = true;
            StartCoroutine(ActivateFire(fireLimits.x, fireLimits.y, initialFire));
        }
        if (amount <= 0 && fireActive)
        {
            fireActive = false;
            StartCoroutine(ActivateFire(fireLimits.y, fireLimits.x, initialFire));
        }
        if (amount > passiveIncomeThreshold - 0.1f && !passiveIncomeFireActive)
        {
            passiveIncomeFireActive = true;
            StartCoroutine(ActivateFire(fireLimits.x, fireLimits.y, passiveIncomeFire));
        }
        if (amount <= passiveIncomeThreshold - 0.2f && passiveIncomeFireActive)
        {
            passiveIncomeFireActive = false;
            StartCoroutine(ActivateFire(fireLimits.y, fireLimits.x, passiveIncomeFire));
        }

        if (amount > passiveIncomeThreshold)
        {
            time += Time.fixedDeltaTime;
            if (time >= passiveIncomeTime)
            {
                time = 0.0f;
                player.AddMoney(passiveIncomeAmount);
            }

            stream.Donate();

            if (!twitch.girlActive)
                music.CrossFade(music.hateHigh);

            if (amount > 0.9f && skullTime >= skullLaughCooldown)
            {
                skullTime = 0.0f;
                skull.SetBool("laugh", true);
            }
        }
        else if (!twitch.girlActive)
            music.CrossFade(music.hateLow);

        skullTime += Time.fixedDeltaTime;
    }

    IEnumerator ActivateFire(float from, float to, Transform fire)
    {
        float t = 0.0f;

        Vector3 fromPosition = fire.localPosition;
        Vector3 toPosition = fire.localPosition;
        fromPosition.x = from;
        toPosition.x = to;

        while (t < fireTime)
        {
            fire.localPosition = Vector3.Lerp(fromPosition, toPosition, Mathf.SmoothStep(0.0f, 1.0f, t / fireTime));

            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        fire.localPosition = toPosition;
    }

    public void Interact()
    {
        amount = Mathf.Clamp01(amount + interactAmount);

        follow.TrainInteract(amount);
    }
}

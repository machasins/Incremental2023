using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HypeMeter : MonoBehaviour
{
    public PlayerData player;
    public StreamController stream;
    public SpriteRenderer bar;
    public Animator heart;

    public float passiveIncomeAmount;
    public float passiveIncomeTime;
    public float passiveIncomeThreshold;
    public Color passiveIncomeColor;
    public float interactAmount;
    public float decayRate;
    public float maxScale;
    public Vector2 heartRate;

    private Color normalColor;
    private float minAmount;
    private float amount;
    private float time = 0.0f;
    private MusicHandler music;
    private TwitchCoordinator twitch;

    // Start is called before the first frame update
    void Start()
    {
        minAmount = transform.localScale.y;
        normalColor = bar.color;
        heart.speed = heartRate.x;

        music = FindFirstObjectByType<MusicHandler>();
        twitch = FindFirstObjectByType<TwitchCoordinator>();
    }

    void OnEnable()
    {
        amount = minAmount;
        transform.localScale = new Vector3(transform.localScale.x, Mathf.Lerp(minAmount, maxScale, amount), transform.localScale.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        amount = Mathf.Clamp01(amount - decayRate * Time.fixedDeltaTime);
        transform.localScale = new Vector3(transform.localScale.x, Mathf.Lerp(minAmount, maxScale, amount), transform.localScale.z);

        heart.speed = Mathf.Lerp(heartRate.x, heartRate.y, (amount - passiveIncomeThreshold) / (1.0f - passiveIncomeThreshold - .1f));

        if (amount > passiveIncomeThreshold)
        {
            time += Time.fixedDeltaTime;
            if (time >= passiveIncomeTime)
            {
                time = 0.0f;
                player.AddMoney(passiveIncomeAmount);
            }

            stream.Interact();
            StartCoroutine(LerpToColor(passiveIncomeColor));

            if (twitch.girlActive)
                music.CrossFade(music.loveHigh);
        }
        else
        {
            StartCoroutine(LerpToColor(normalColor));

            if (twitch.girlActive)
                music.CrossFade(music.loveLow);
        }
    }

    IEnumerator LerpToColor(Color c)
    {
        float time = 0.0f;
        Color from = bar.color;
        if (from != c)
        {
            while(time < 0.5f)
            {
                bar.color = Color.Lerp(from, c, time / 0.5f);

                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            bar.color = c;
        }
    }

    public void Interact()
    {
        amount = Mathf.Clamp01(amount + interactAmount);
    }
}

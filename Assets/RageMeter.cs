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

    private SpriteRenderer passiveIncomeFireSprite;
    private float amount;
    private float time = 0.0f;
    private float skullTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 position = initialFire.localPosition;
        position.x = fireLimits.x;
        initialFire.localPosition = position;

        position = passiveIncomeFire.localPosition;
        position.x = fireLimits.x;
        passiveIncomeFire.localPosition = position;

        passiveIncomeFireSprite = passiveIncomeFire.GetComponent<SpriteRenderer>();
        passiveIncomeFireSprite.color = Color.clear;
    }

    void OnEnable()
    {
        amount = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        amount = Mathf.Clamp01(amount - decayRate * Time.fixedDeltaTime);

        Vector3 position = initialFire.localPosition;
        position.x = Mathf.Lerp(fireLimits.x, fireLimits.y, amount / passiveIncomeThreshold);
        initialFire.localPosition = position;

        position = passiveIncomeFire.localPosition;
        position.x = Mathf.Lerp(fireLimits.x, fireLimits.y, (amount - passiveIncomeThreshold) / (1.0f - passiveIncomeThreshold - 0.1f));
        passiveIncomeFire.localPosition = position;

        passiveIncomeFireSprite.color = Color.Lerp(Color.clear, Color.white, (amount - passiveIncomeThreshold) / (1.0f - passiveIncomeThreshold - 0.1f));

        if (amount > passiveIncomeThreshold)
        {
            time += Time.fixedDeltaTime;
            if (time >= passiveIncomeTime)
            {
                time = 0.0f;
                player.AddMoney(passiveIncomeAmount);
            }

            stream.Interact();

            if (amount > 0.9f && skullTime >= skullLaughCooldown)
            {
                skullTime = 0.0f;
                skull.SetBool("laugh", true);
            }
        }

        skullTime += Time.fixedDeltaTime;
    }

    public void Interact()
    {
        amount = Mathf.Clamp01(amount + interactAmount);
    }
}

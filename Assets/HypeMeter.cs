using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HypeMeter : MonoBehaviour
{
    public PlayerData player;
    public StreamController stream;
    public SpriteRenderer bar;

    public float passiveIncomeAmount;
    public float passiveIncomeTime;
    public float passiveIncomeThreshold;
    public Color passiveIncomeColor;
    public float interactAmount;
    public float decayRate;
    public float maxScale;

    private Color normalColor;
    private float minAmount;
    private float amount;
    private float time = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        minAmount = transform.localScale.y;
        normalColor = bar.color;
    }

    void OnEnable()
    {
        amount = minAmount;
        transform.localScale = new Vector3(transform.localScale.x, amount, transform.localScale.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        amount = Mathf.Clamp(amount - decayRate * Time.fixedDeltaTime, minAmount, maxScale);
        transform.localScale = new Vector3(transform.localScale.x, amount, transform.localScale.z);

        if (amount > passiveIncomeThreshold)
        {
            time += Time.fixedDeltaTime;
            if (time >= passiveIncomeTime)
            {
                time = 0.0f;
                player.AddMoney(passiveIncomeAmount);
            }

            stream.Interact();
            bar.color = passiveIncomeColor;
        }
        else
            bar.color = normalColor;
    }

    public void Interact()
    {
        amount = Mathf.Clamp(amount + interactAmount, minAmount, maxScale);
    }
}

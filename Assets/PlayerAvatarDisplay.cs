using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatarDisplay : MonoBehaviour
{
    public FactionDisplay faction;
    public Sprite defaultSprite;
    public Sprite animeSprite;
    public Sprite furrySprite;
    public Sprite streamerSprite;
    public Sprite militarySprite;

    SpriteRenderer avatar;

    // Start is called before the first frame update
    void Start()
    {
        avatar = GetComponent<SpriteRenderer>();
        avatar.sprite = defaultSprite;
        avatar.size = defaultSprite.bounds.size / (Mathf.Min(defaultSprite.bounds.size.x, defaultSprite.bounds.size.y));
    }

    // Update is called once per frame
    void Update()
    {
        int maxValue = -1;
        int maxIndex = -1;

        for (int i = 0; i < faction.factionPoints.Length; ++i)
        {
            int value = faction.factionPoints[i];
            if (value > maxValue)
            {
                maxValue = value;
                maxIndex = i;
            }
            else if (value == maxValue)
                maxIndex = -1;
        }

        if (maxIndex == -1)
        {
            avatar.sprite = defaultSprite;
            avatar.size = defaultSprite.bounds.size / (Mathf.Min(defaultSprite.bounds.size.x, defaultSprite.bounds.size.y));
        }
        else
        {
            switch ((User.Type)maxIndex)
            {
                case User.Type.normal:
                avatar.sprite = streamerSprite;
                avatar.size = streamerSprite.bounds.size / (Mathf.Min(streamerSprite.bounds.size.x, streamerSprite.bounds.size.y));
                break;
                case User.Type.anime:
                avatar.sprite = animeSprite;
                avatar.size = animeSprite.bounds.size / (Mathf.Min(streamerSprite.bounds.size.x, streamerSprite.bounds.size.y));
                break;
                case User.Type.furry:
                avatar.sprite = furrySprite;
                avatar.size = furrySprite.bounds.size / (Mathf.Min(furrySprite.bounds.size.x, furrySprite.bounds.size.y));
                break;
                case User.Type.kommando:
                avatar.sprite = militarySprite;
                avatar.size = militarySprite.bounds.size / (Mathf.Min(militarySprite.bounds.size.x, militarySprite.bounds.size.y));
                break;
            }
        }
    }
}
